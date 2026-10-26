using Godot;
using System;

public partial class Game : Node2D
{
	[Export] public Node2D Pausable;
	[Export] public CanvasLayer UI;
	[Export] public PauseMenu PauseMenu;
	[Export] public Sprite2D Picker;
	public override void _Ready()
	{
		GameManager.Instance.Game = this;
		GameManager.Instance.Picker = Picker;
		PauseMenu.Resume += () => { PauseMenu.Hide(); Pausable.ProcessMode = ProcessModeEnum.Inherit; };
		
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("esc"))
		{
			Pausable.ProcessMode = ProcessModeEnum.Disabled;
			PauseMenu.Show();
		}
	}
}
