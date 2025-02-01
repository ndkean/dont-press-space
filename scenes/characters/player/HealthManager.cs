using Godot;

public partial class HealthManager : Node
{
	[Export] private int _startingHealth;

	[Export] public int MaxHealth { get; set; }

	private int _health;
	public int Health { 
		get => _health;
		set
		{
			_health = Mathf.Clamp(value, 0, MaxHealth);
			EmitSignal(SignalName.HealthChanged, Health);

			if (_health <= 0)
				EmitSignal(SignalName.HealthDepleted);
		}
	}

	[Signal]
	public delegate void HealthChangedEventHandler(int health);

	[Signal]
	public delegate void HealthDepletedEventHandler();

	public override void _EnterTree()
	{
		Health = _startingHealth;
	}

	public override void _Ready()
	{
		EmitSignal(SignalName.HealthChanged, Health);
	}

	public void OnHitReceived(Hurtbox hurtbox)
	{
		Health -= hurtbox.Damage;
	}
}
