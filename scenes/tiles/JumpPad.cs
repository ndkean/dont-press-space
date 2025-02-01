using Godot;
using System;

public partial class JumpPad : Node2D
{
	[Export] private Area2D _area;
	[Export] private float _impulse;

	public override void _Ready()
	{
		_area.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is CharacterBody2D character)
		{
			character.Velocity = character.Velocity with { Y = -_impulse };
		}
	}
}
