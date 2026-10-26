using Godot;
using System;

public partial class GameManager : Node2D
{
	public static GameManager Instance;

	public Game Game;
	public override void _Ready()
	{
		if (GameManager.Instance == null)
			GameManager.Instance = this;
		else QueueFree();
	}
	public override void _Process(double delta)
	{
		
	}
}
