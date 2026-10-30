using Godot;
using System;
using TwelveJam.scripts.buildings;

public partial class Space : Node2D
{
	[Export] private PackedScene MeteorScene, MoverScene;

	//[Export] private MobSpawner spawner;
	public override void _Ready()
	{
		GameManager.Instance.DayChanged += InstanceOnDayChanged;
	}

	private void InstanceOnDayChanged(int day)
	{
		for (int i = 0; i < GameManager.Instance.MeteorPerDay; i++)
		{
			var meteor = MeteorScene.Instantiate<Building>();
			var mover = MoverScene.Instantiate<Mover>();
			meteor.Position = new Vector2(-200 + -400 *GD.Randf(), 1000 + -1200 * GD.Randf());
			var angle = GD.Randf() * Mathf.Tau;
			
			float distanceFactor = Mathf.Sqrt(GD.Randf()); 
			float targetX = 1000 + (700 * distanceFactor) * Mathf.Cos(angle);
			float targetY = 500 + (500 * distanceFactor) * Mathf.Sin(angle);
			mover.movePosition = new Vector2(targetX, targetY);
			
			meteor.AddChild(mover);
			AddChild(meteor);
		}
	}

	public override void _Process(double delta)
	{
	}
}
