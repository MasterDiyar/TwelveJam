using Godot;
using System;

public partial class Game : Node2D
{
	[Export] public Node2D Pausable;
	[Export] public CanvasLayer UI;
	[Export] public PauseMenu PauseMenu;
	[Export] public Picker Picker;
	[Export] public InfoTablet InfoTablet;
	[Export] public CanvasModulate Modulation;
	[Export] public Color Normal, LowHp, clr;
	[Export] public Player Player;
	public override void _Ready()
	{
		GameManager.Instance.Game = this;
		GameManager.Instance.Picker = Picker;
		GameManager.Instance.InfoTablet = InfoTablet;
		PauseMenu.Resume += () => { PauseMenu.Hide(); Pausable.ProcessMode = ProcessModeEnum.Inherit; };
		Player??=GameManager.Instance.Player;
	}
	
	

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("esc"))
		{
			Pausable.ProcessMode = ProcessModeEnum.Disabled;
			PauseMenu.Show();
		}
	}

	public override void _Process(double delta)
	{
		float hpPercent = Mathf.Clamp(Player.Hp / Player.MaxHp, 0, 1);
		clr = LowHp.Lerp(Normal, hpPercent);
		Modulation.Color = clr;
	}
}
