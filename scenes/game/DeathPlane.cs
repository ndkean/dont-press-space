using Godot;
using System;

public partial class DeathPlane : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is IDies receiver)
			receiver.Kill();
	}
}
