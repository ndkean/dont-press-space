using Godot;
using System;

public partial class PlayerController : CharacterBody2D, IPicksUpCollectables, IDies
{
	public static PlayerController Player { get; private set; }

	[Export] private float _speed;
	[Export] private float _jumpHeight;
	[Export] private float _hitDefaultKnockback = 100;

	private float JumpVelocity => Mathf.Sqrt(2 * GetGravity().Y * _jumpHeight);

	[Export] private CollisionShape2D _collisionShape;

	[Export] private JumpManager _jumpManager;
	[Export] private HealthManager _healthManager;

	public override void _EnterTree()
	{
		Player = this;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor() && _jumpManager.CanJump)
		{
			velocity.Y = -JumpVelocity;
			_jumpManager.OnJump();
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = new(Input.GetAxis("move_left", "move_right"), 0);
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * _speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public bool ReceiveCollectable(Collectable collectable)
	{
		switch (collectable.Type)
		{
			case CollectableType.Health:
				if (_healthManager.Health == _healthManager.MaxHealth) return false;
				_healthManager.Health += collectable.Value;
				break;
			case CollectableType.Jumps:
				_jumpManager.JumpsRemaining += collectable.Value;
				break;
			default: return false;
		}

		return true;
	}

	public void Kill()
	{
		GetTree().CallDeferred("reload_current_scene");
	}

	public void OnHitReceived(Hurtbox hurtbox)
	{
		Vector2 direction = (_collisionShape.GlobalPosition - hurtbox.GlobalPosition).Normalized();
		Vector2 vec = direction * hurtbox.KnockbackMagnitude + new Vector2(0, -_hitDefaultKnockback);
		Velocity = Velocity with {
			X = Velocity.X + vec.X, Y = vec.Y
		};
	}
}
