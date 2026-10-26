using Godot;
using System;

public partial class GameManager : Node2D
{
	public static GameManager Instance;

	public Sprite2D Picker;

	public Game Game;
	public override void _Ready()
	{
		if (GameManager.Instance == null)
			GameManager.Instance = this;
		else QueueFree();
	}

	public void SetPicker(Vector2 scale, Vector2 position)
	{
		Picker.GlobalPosition = position;
		Picker.GlobalScale = scale;
	}
}
