using Godot;
using System;

public partial class Platform : Node2D
{
	[Export] private float _speed;
	[Export] private Node2D _position1;
	[Export] private Node2D _position2;

	private bool _movingTowards1;

	private Vector2 _oneToTwo;

	public override void _Ready()
	{
		Position = _position1.Position;
		_oneToTwo = (_position2.Position - _position1.Position).Normalized();
	}

	public override void _PhysicsProcess(double delta)
	{
		const float epsilon = 0.001f;
		
		if (_movingTowards1)
		{
			Position -= _oneToTwo * _speed;
			if (Position.DistanceSquaredTo(_position1.Position) < epsilon)
				_movingTowards1 = false;
		}
		else
		{
			Position += _oneToTwo * _speed;
			if (Position.DistanceSquaredTo(_position2.Position) < epsilon)
				_movingTowards1 = true;
		}
	}
}
