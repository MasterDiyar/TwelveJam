using Godot;
using System;
using TwelveJam.scripts.units;

public partial class Enemy2 : Unit
{
	[Export] float closeRange = 20f;
	[Export] private AnimatedSprite2D sohal;
	[Export] CpuParticles2D cpuParticles2D, c2;
	[Export] Sprite2D sprite, s2;
	private Player _player;
	private float _modulation = 0;
	
	public override void _Ready()
	{
		_player = GameManager.Instance.Player;sohal.Play("default");
		Scale = (GD.Randf() / 2f + 0.5f)* Scale;
		closeRange *= Scale.X;
	}

	public override void _PhysicsProcess(double delta)
	{
		Rotation = (_player.GlobalPosition - GlobalPosition).Angle();
		if (GlobalPosition.DistanceTo(_player.GlobalPosition) > closeRange)
		{
			Velocity = Speed * Vector2.FromAngle(Rotation);
			cpuParticles2D.Emitting = true;
			c2.Emitting = true;
			_modulation = Mathf.Clamp(_modulation+(float)delta, 0f, 1f);
		}else {
			_player.TakeDamage(20*(float)delta, Faction.Giorno);
			cpuParticles2D.Emitting = false;
			c2.Emitting = false;
			_modulation = Mathf.Clamp(_modulation-6*(float)delta, 0f, 1f);
		}
		sprite.Modulate = sprite.Modulate with {A = _modulation};
		s2.Modulate = s2.Modulate with {A = _modulation};
		MoveAndSlide();
	}
	
	
}