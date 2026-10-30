using Godot;
using System;
using TwelveJam.scripts.game;
using TwelveJam.scripts.units;

public partial class Enemy3 : Unit
{
	[Export] float closeRange = 30f;
	[Export] CpuParticles2D cpuParticles2D;
	[Export] Sprite2D sprite;
	
	private float _modulation = 0;
	private Player _player;
	bool isStealed = false;
	private Items stealed;
	public override void _Ready()
	{
		_player = GameManager.Instance.Player;
	}

	public override void _PhysicsProcess(double delta)
	{
		Rotation = (isStealed?Single.Pi:0) + (_player.GlobalPosition - GlobalPosition).Angle();
		var dist = GlobalPosition.DistanceTo(_player.GlobalPosition);
		if (dist > closeRange)
		{
			Velocity = Speed * Vector2.FromAngle(Rotation);
			cpuParticles2D.Emitting = true;
			_modulation = Mathf.Clamp(_modulation+(float)delta, 0f, 1f);
			if (dist > 1488f)
				QueueFree();
		}else {
			isStealed = true;
			stealed = MoneyManager.Instance.Items * 0.1f;
			MoneyManager.Instance.Items *= 0.9f;
		}
		sprite.Modulate = sprite.Modulate with {A = _modulation};
		
		MoveAndSlide();
	}

	public override void OnDie()
	{
		MoneyManager.Instance.Items += stealed;
	}
}
