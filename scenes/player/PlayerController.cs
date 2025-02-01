using Godot;
using System;

public partial class PlayerController : CharacterBody2D, IPicksUpCollectables
{
	[Export] private float _speed;
	[Export] private float _jumpVelocity;

	[Export] private JumpManager _jumpManager;
	[Export] private HealthManager _healthManager;

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
			velocity.Y = -_jumpVelocity;
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
}
