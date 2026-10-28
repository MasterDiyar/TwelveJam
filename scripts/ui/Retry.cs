using Godot;
using System;

public partial class Retry : Control
{
	public override void _Ready()
	{
		var btn = GetNode<Button>("Button");
		btn.Pressed += () =>
		{
			GameManager.Instance.ResetGame();
         			MoneyManager.Instance.ResetGame();
			GetTree().Root.AddChild( GD.Load<PackedScene>("res://scenes/game/game.tscn").Instantiate());
			QueueFree();
		};
	}
}
