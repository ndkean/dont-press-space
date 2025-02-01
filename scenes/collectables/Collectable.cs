using Godot;

public partial class Collectable : Node2D
{
	[Export] public CollectableType Type { get; set; }

	[Export] public int Value { get; set; }

	[Export] private NodePath _areaPath = "Area2D";

	private Area2D _area;

	[Signal]
	public delegate void CollectEventHandler();

	public override void _Ready()
	{
		_area = GetNode<Area2D>(_areaPath);
		_area.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is not IPicksUpCollectables receiver) return;
		if (!receiver.ReceiveCollectable(this)) return;
		
		EmitSignal(SignalName.Collect);
		QueueFree();
	}
}
