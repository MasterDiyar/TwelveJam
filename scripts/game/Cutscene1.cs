using Godot;
using System;

public partial class Cutscene1 : Node2D
{
	[Export] AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		animationPlayer.Play("staart");
		animationPlayer.AnimationFinished += AnimationPlayerOnAnimationFinished;
	}

	private void AnimationPlayerOnAnimationFinished(StringName animName)
	{
		if (animName == "staart") {
			GameManager.Instance.Game.resourceUI.isStarted = true;
			QueueFree();
		}
	}
}
