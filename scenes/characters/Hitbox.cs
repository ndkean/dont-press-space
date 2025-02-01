using Godot;
using System;
using DontPressSpace.scenes.characters;

public partial class Hitbox : Area2D
{
	[Signal]
	public delegate void HitReceivedEventHandler(Hurtbox hurtbox);
	
	[Signal]
	public delegate void InvincibleStateChangedEventHandler(bool invincible);
	
	[Export] private float _invincibilityDuration;

	[Export] public bool Enabled { get; set; } = true;

	[Export] public HitboxType Type { get; set; }

	private float _invincibilityTimer;

	public override void _PhysicsProcess(double delta)
	{
		if (!Enabled) return;
		
		if (Monitoring)
		{
			foreach (Area2D area in GetOverlappingAreas())
			{
				if (area is not Hurtbox hurtbox) continue;
				if (!hurtbox.Enabled) continue;
				if ((hurtbox.Mask & (HitboxMask)(1 << (int)Type)) == 0) continue;
				ReceiveHit(hurtbox);

				_invincibilityTimer = _invincibilityDuration;
				Monitoring = false;
				EmitSignal(SignalName.InvincibleStateChanged, true);

				break;
			}
		}
		else
		{
			_invincibilityTimer -= (float)delta;

			if (_invincibilityTimer <= 0)
			{
				Monitoring = true;
				EmitSignal(SignalName.InvincibleStateChanged, false);
			}
		}
	}

	public void ReceiveHit(Hurtbox hurtbox)
	{
		EmitSignal(SignalName.HitReceived, hurtbox);
	}
}
