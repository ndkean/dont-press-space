using Godot;
using System;

public partial class HealthCounter : Label
{
	public void HealthChanged(int health)
	{
		Text = $"Health: {health}";
	}
}
