using Godot;
using System.ComponentModel;

public partial class JumpManager : Node
{
	[Export] private int _startingJumps;

	[Signal]
	public delegate void JumpsRemainingChangedEventHandler(int jumpsRemaining);

	private int _jumpsRemaining;

	public int JumpsRemaining
	{
		get => _jumpsRemaining;
		set
		{
			_jumpsRemaining = value;
			EmitSignal(SignalName.JumpsRemainingChanged, JumpsRemaining);
		}
	}

	public bool CanJump => JumpsRemaining > 0;

	public override void _EnterTree()
	{
		JumpsRemaining = _startingJumps;
	}

	public override void _Ready()
	{
		EmitSignal(SignalName.JumpsRemainingChanged, JumpsRemaining);
	}

	public void OnJump()
	{
		JumpsRemaining--;
	}
}
