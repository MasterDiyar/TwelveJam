using Godot;
using System;
using TwelveJam.scripts.buildings;

public partial class Mover : Node2D
{
	[Export] public Node2D movingObject;
	[Export] public Vector2 movePosition;
	[Export] public float moveSpeed;
	[Export] public float distance = 5;
	[Export] private bool isMeteor = true;
	public override void _Ready()
	{
		if (movingObject is Building bld)
			bld.Clickable = false;
	}

	public override void _Process(double delta)
	{
		if (movingObject == null || !IsInstanceValid(movingObject))
		{
			QueueFree();
			return;
		}
		var direction = movePosition - movingObject.GlobalPosition;
		if ((direction).Length() < distance) {
			if (isMeteor)
				if (movingObject is Building bld)
					bld.Clickable = true;

			QueueFree();
		}else
			movingObject.GlobalPosition += direction.Normalized() * moveSpeed * (float)delta;
			
		
	}
}
