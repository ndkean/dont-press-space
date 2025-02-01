using Godot;
using System;

public partial class Hurtbox : Area2D
{
	[Export] public int Damage { get; private set; }

	[Export] public bool Enabled { get; set; } = true;

	[Export] public float KnockbackMagnitude { get; private set; } = 100;

	[Export] public HitboxMask Mask { get; set; }
}
	