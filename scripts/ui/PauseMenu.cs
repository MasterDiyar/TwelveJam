using Godot;
using System;

public partial class PauseMenu : Control
{
	[Export] public Button resumeButton;
	[Export] public Button restartButton;
	[Export] public Button exitButton;
	
	[Signal] public delegate void ResumeEventHandler();
	[Signal] public delegate void RestartEventHandler();
	[Signal] public delegate void ExitEventHandler();
	public override void _Ready()
	{
		resumeButton.Pressed += ResumeButtonOnPressed;
		restartButton.Pressed += () => GameManager.Instance.ExecuteGameOver();
		exitButton.Pressed += () => GetTree().Quit();
	}

	private void ResumeButtonOnPressed()
	{
		EmitSignalResume();
	}
}
