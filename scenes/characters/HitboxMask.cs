using System;

[Flags]
public enum HitboxMask
{
	Ally = 1 << 0,
	Enemy = 1 << 1,
}
