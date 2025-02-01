using Godot;

public partial class HitDetector : Node
{
	[Export] private Area2D _area;
	[Export(PropertyHint.Layers2DPhysics)] private uint _damageMask;

	[Export] private float _invincibilityDuration;

	public bool Enabled { get; set; }

	[Signal]
	public delegate void HitEventHandler();
	
	[Signal]
	public delegate void InvincibleStateChangedEventHandler(bool invincible);
	
	private float _invincibilityTimer;

	public override void _Process(double delta)
	{
		if (Enabled)
		{
			DetectHits();	
		}
		else
		{
			_invincibilityTimer -= (float)delta;
			if (_invincibilityTimer <= 0)
			{
				Enabled = true;
				EmitSignal(SignalName.InvincibleStateChanged, false);
			}
		}
	}

	public void DetectHits()
	{
		foreach (Node2D body in _area.GetOverlappingBodies())
		{
			if (body is not CollisionObject2D collisionObject) continue;
			if ((collisionObject.CollisionLayer & _damageMask) == 0) continue;
			
			EmitSignal(SignalName.Hit);
			Enabled = false;
			_invincibilityTimer = _invincibilityDuration;
			EmitSignal(SignalName.InvincibleStateChanged, true);

			break;
		}
	}
}
