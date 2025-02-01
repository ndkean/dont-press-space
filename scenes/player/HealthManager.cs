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
		}
	}

	[Signal]
	public delegate void HealthChangedEventHandler(int health);

	public override void _EnterTree()
	{
		Health = _startingHealth;
	}

	public override void _Ready()
	{
		EmitSignal(SignalName.HealthChanged, Health);
	}

	public void OnHit()
	{
		Health--;
	}
}
