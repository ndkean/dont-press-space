using System;
using System.Diagnostics;

public readonly struct Facing
{
	public static readonly Facing Left = new(-1);

	public static readonly Facing Right = new(1);

	public int Value { get; }

	public Facing(int value)
	{
		if (value is not (-1 or 1)) throw new ArgumentException($"Value must be 1 or -1: {value}");
		Value = value;
	}

	public static implicit operator Int32(Facing x)
	{
		return x.Value;
	}

	public override string ToString()
	{
		return this == Left ? "Left" : "Right";
	}
}
