using Godot;
using System;

public partial class ParralaxCanvas : Sprite2D
{

	private Vector2 _lastPlayerPosition;

	public override void _Ready()
	{
		if (GameManager.Instance?.Player != null)
			_lastPlayerPosition = GameManager.Instance.Player.GlobalPosition;
		
	}

	public override void _Process(double delta)
	{
		if (GameManager.Instance?.Player == null) return;

		var currentPlayerPos = GameManager.Instance.Player.GlobalPosition;
		Vector2 playerDelta = currentPlayerPos - _lastPlayerPosition;
        
		Position += playerDelta * 0.92f;
		_lastPlayerPosition = currentPlayerPos;
	}
}
