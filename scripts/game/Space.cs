using Godot;
using System;
using TwelveJam.scripts.buildings;

public partial class Space : Node2D
{
	[Export] private PackedScene MeteorScene, MoverScene;
	[Export] private AudioStreamPlayer SoundPlayer;

	[Export] private AudioStreamMP3[] pack1, pack2;
	
	[Export] private int RepeatsPerLoop = 4;
	private int _currentLoopIndex = 0, currentsound = 0;
	int _currentLoop = 0;
	private Timer t;
	public override void _Ready()
	{
		t = new Timer();
		AddChild(t);
		t.WaitTime = 4f/117f;
		t.OneShot = true;
		t.Timeout += () => SoundPlayer.Play();
		GameManager.Instance.DayChanged += InstanceOnDayChanged;
		SoundPlayer.Stream = pack1[currentsound];
		SoundPlayer.Play();
		SoundPlayer.Finished += Melody;
	}

	private void InstanceOnDayChanged(int day)
	{
		for (int i = 0; i < GameManager.Instance.MeteorPerDay; i++)
		{
			var meteor = MeteorScene.Instantiate<Building>();
			var mover = MoverScene.Instantiate<Mover>();
			mover.Setup(meteor);
			mover.moveSpeed = 100 + 100 * GD.Randf();
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

	void Melody()
	{
		if (_currentLoop == 0) {
			_currentLoopIndex++;
			if (currentsound == 1)
			{
				currentsound++;
				SoundPlayer.Stream = pack1[currentsound];
				_currentLoopIndex--;
			}
			if (_currentLoopIndex >= RepeatsPerLoop) {
				currentsound++;
				_currentLoopIndex = 0;

				if (currentsound >= pack1.Length) {
					_currentLoop = 1;
					currentsound = 0;
					SoundPlayer.Stream = pack2[currentsound]; 
				} else {
					SoundPlayer.Stream = pack1[currentsound];
				}
			}
			t.Start();
		}
		else if (_currentLoop == 1) {
			_currentLoopIndex++;
			if (_currentLoopIndex >= RepeatsPerLoop) {
				currentsound++;
				_currentLoopIndex = 0;
				if (currentsound >= pack2.Length) {
					_currentLoop = 0;
					currentsound = 0;
					SoundPlayer.Stream = pack1[currentsound];
				} else {
					SoundPlayer.Stream = pack2[currentsound];
				}
			}
			t.Start();
		}
	}
	
	
}
