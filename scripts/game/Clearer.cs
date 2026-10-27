using Godot;
using System;

public partial class Clearer : Timer
{
	public override void _Ready()
	{
		Start();
		Timeout += () => GetParent().QueueFree();
	}
}
