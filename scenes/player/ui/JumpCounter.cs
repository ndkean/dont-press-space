using Godot;
using System;
using System.ComponentModel;

public partial class JumpCounter : Label
{
	public void OnJumpsRemainingChanged(int jumpsRemaining)
	{
		Text = $"Jumps Remaining: {jumpsRemaining}";
	}
}
