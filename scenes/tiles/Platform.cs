using Godot;
using System;
using System.Diagnostics;

[Tool]
public partial class Platform : Node2D
{
	[Export] private float _speed;

	[Export] private NodePath _position1Path = "../Position1";

	[Export] private NodePath _position2Path = "../Position2";
	
	private Node2D _position1;

	private Node2D _position2;

	[Export(PropertyHint.Range, "0,1")] private float _startingDistance;

	private bool _movingTowards1;

	private Vector2 _oneToTwoDir;
	private Vector2 _oneToTwo;

	public override void _Ready()
	{
		_position1 = GetNode<Node2D>(_position1Path);
		_position2 = GetNode<Node2D>(_position2Path);
		
		Debug.Assert(_position1 != null, "Position 1 should not be null");
		Debug.Assert(_position2 != null, "Position 2 should not be null");
		
		_oneToTwo = _position2.Position - _position1.Position;
		_oneToTwoDir = _oneToTwo.Normalized();
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
			UpdatePosition();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Engine.IsEditorHint()) return;
		
		float distancePercentage = (Position - _position1.Position).Length() / _oneToTwo.Length();
		
		if (_movingTowards1)
		{
			Position -= _oneToTwoDir * _speed;
			if (distancePercentage <= 0)
				_movingTowards1 = false;
		}
		else
		{
			Position += _oneToTwoDir * _speed;
			if (distancePercentage >= 1)
				_movingTowards1 = true;
		}
	}

	private void UpdatePosition()
	{
		if (_position1 == null || _position2 == null) return;
		
		Position = _position1.Position + _oneToTwo * _startingDistance;
	}
}
