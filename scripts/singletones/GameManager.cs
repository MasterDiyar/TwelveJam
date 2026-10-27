using Godot;
using System;
using TwelveJam.scripts.buildings;

public partial class GameManager : Node2D
{
	public static GameManager Instance;

	public Picker Picker;
	public InfoTablet InfoTablet;

	public Game Game;
	public Player Player;
	public override void _Ready()
	{
		if (GameManager.Instance == null)
			GameManager.Instance = this;
		else QueueFree();
	}

	public void OpenInfoForBuilding(Building building)
	{
		Picker?.SetPicker(building.GlobalPosition, building.AmplitudeMinimal.X, building.AmplitudeMinimal.Y);
		
		if (InfoTablet == null) return;
		InfoTablet.GlobalPosition = building.GlobalPosition + Vector2.Right * 64 * building.AmplitudeMinimal.Y; 
		InfoTablet.ShowData(building.Ferrum, building.Titan, building.Silicon, building.Sulfur);
	}
}
