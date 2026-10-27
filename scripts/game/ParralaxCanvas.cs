using Godot;
using System;

public partial class ParralaxCanvas : Sprite2D
{

	public float Speed = 300f;
	public override void _Process(double delta)
	{
		
		Vector2 direction = Input.GetVector("a", "d", "w", "s");

		if (direction != Vector2.Zero)
		{
			Speed = GameManager.Instance.Player.Speed;
			Position += direction * (float)delta * Speed * .82f ;
		}
	}
}
