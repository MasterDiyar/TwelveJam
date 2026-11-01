using Godot;
using System;
using TwelveJam.scripts.units;

public partial class CircleBullet : Bullet
{
	private float _dt = 0;
	ShaderMaterial _shader;
	[Export] Sprite2D _sprite;

	public override void _Ready()
	{
		base._Ready();
		_shader = (ShaderMaterial)_sprite.Material;
	}

	public override void _Process(double delta)
	{
		float dt = (float)delta;
		_dt += dt;
		Scale = Vector2.One * Mathf.Clamp(Speed * _dt, 0.001f, 1.7415f);
		Speed += dt * Acceleration;
		float progressFactor = Mathf.Clamp(_dt / LifeTime, 0.0f, 1.0f);

		float targetProgress = progressFactor * 0.6f;
		_shader.SetShaderParameter("progress", targetProgress);
		if (_dt > LifeTime)
			QueueFree();
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body is IDamagable damagable)
		{
			if (damagable.GetFaction() == this.Faction) return;
			damagable.TakeDamage(Damage, Faction);
		}
	}
}
