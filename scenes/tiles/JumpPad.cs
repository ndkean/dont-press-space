using Godot;
using System;

public partial class JumpPad : Node2D
{
	private static float _gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
	
	[Export] private Area2D _area;
	[Export] private float _jumpHeight;
	
	private float Impulse => Mathf.Sqrt(2 * _gravity * _jumpHeight);
	
	public override void _Ready()
	{
		_area.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is CharacterBody2D character)
		{
			character.Velocity = character.Velocity with { Y = -Impulse };
		}
	}
}
