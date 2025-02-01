using Godot;
using System;
using System.Diagnostics;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class DrillGuy : CharacterBody2D, IDies
{
	[Export] private NodePath _animationPlayerPath = "AnimationPlayer";
	[Export] private NodePath _floorRayPath = "Rays/FloorRay";
	[Export] private NodePath _wallRayPath = "Rays/WallRay";
	[Export] private NodePath _spritePath = "AnimatedSprite2D";
	[Export] private NodePath _losRayPath = "LOSRay";
	
	[Export] private float _wanderingSpeed;
	[Export] private float _playerVisibleRange;
	[Export] private float _attackSpeed;

	[Export] private Timer _attackWindupTimer;
	[Export] private Timer _attackTimer;
	[Export] private Timer _attackResetTimer;

	private AnimationPlayer _animationPlayer;
	private RayCast2D _floorRay;
	private RayCast2D _wallRay;
	private AnimatedSprite2D _sprite;
	private Node2D _raysParent;
	private PlayerController _player;
	private RayCast2D _losRay;

	private const string _upright = "upright";
	private const string _drilling = "drilling";

	[Export(PropertyHint.Enum, "Left,Right")] private int _startingFacing;

	private Facing _facing;
	
	public Facing Facing
	{
		get => _facing;
		set
		{
			_facing = value;
			_sprite.FlipH = _facing == Facing.Left;
			_raysParent.Scale = new Vector2(_facing, 1);
		}
	}

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>(_animationPlayerPath);
		Debug.Assert(_animationPlayer != null, "Animation player should not be null");
		_animationPlayer.CurrentAnimation = _upright;

		_floorRay = GetNode<RayCast2D>(_floorRayPath);
		Debug.Assert(_floorRay != null, "FloorRay should not be null");
		
		_wallRay = GetNode<RayCast2D>(_wallRayPath);
		Debug.Assert(_wallRay != null, "WallRay should not be null");

		_sprite = GetNode<AnimatedSprite2D>(_spritePath);
		Debug.Assert(_sprite != null, "Sprite should not be null");

		_raysParent = _wallRay.GetParent<Node2D>();

		_losRay = GetNode<RayCast2D>(_losRayPath);
		
		Facing = _startingFacing == 0 ? Facing.Left : Facing.Right;
		_player = PlayerController.Player;

		_attackWindupTimer.Timeout += () =>
		{
			Velocity = new Vector2(Facing * _attackSpeed, 0);
			_animationPlayer.CurrentAnimation = _drilling;
			_attackTimer.Start();
		};

		_attackTimer.Timeout += () =>
		{
			Velocity = Vector2.Zero;
			_animationPlayer.CurrentAnimation = _upright;
			_attackResetTimer.Start();
		};
	}
	
	public override void _PhysicsProcess(double delta)
	{
		float d = (float)delta;
		bool attacking = IsAttacking();
		// GD.Print($"Winding up: {!_attackWindupTimer.IsStopped()} Attacking: {!_attackTimer.IsStopped()} Resetting: {!_attackResetTimer.IsStopped()}");
		if (!attacking)
		{
			if (!IsOnFloor())
			{
				Velocity += GetGravity() * d;
			}
			else
			{
				if (_attackResetTimer.IsStopped())
				{
					if (PlayerVisible())
					{
						// Start attacking
						FacePlayer();
						Velocity = Vector2.Zero;
						_attackWindupTimer.Start();
					}
					else
					{
						// Wander
						Velocity = Vector2.Right * _wanderingSpeed * (int)Facing;
						if (!_floorRay.IsColliding() || _wallRay.IsColliding())
							Facing = new Facing(-Facing);
					}
				}
				else
				{
					Velocity = Vector2.Zero;
				}
			}
		}

		MoveAndSlide();
	}

	private bool PlayerVisible()
	{
		_losRay.TargetPosition = _player.GlobalPosition - GlobalPosition;
		return 
			_losRay.TargetPosition.Length() < _playerVisibleRange
			&& !_losRay.IsColliding();
	}

	private void FacePlayer()
	{
		Facing = (_player.GlobalPosition - GlobalPosition).X < 0 ? Facing.Left : Facing.Right;
	}

	private bool IsAttacking()
	{
		return !_attackWindupTimer.IsStopped() || !_attackTimer.IsStopped();
	}

	private void OnHitReceived(Hurtbox h)
	{
		GD.Print(h.Name);
		Kill();
	}

	public void Kill()
	{
		QueueFree();
	}
}
