using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	[Export] private float _speed;
	[Export] private float _jumpVelocity;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = -_jumpVelocity;
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
}
