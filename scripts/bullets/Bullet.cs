using Godot;
using System;
using TwelveJam.scripts.units;

public partial class Bullet : Area2D
{
	[Export] public Faction Faction;

	public float Speed = 560f, Damage = 10f, Acceleration = 0f, LifeTime = 1;
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is IDamagable damagable)
		{
			if (damagable.GetFaction() == this.Faction) return;
			damagable.TakeDamage(Damage, Faction);
			QueueFree();
		}
	}

	public void SetWeapon(Faction faction, float damage, float speed, float accel, float angle, float lifeTime =1)
	{
		this.Faction = faction;
		this.Damage = damage;
		this.Speed = speed;
		this.Acceleration = accel;
		this.Rotation = angle;
		this.LifeTime = lifeTime;
	}

	public override void _Process(double delta)
	{
		float dt = (float)delta;
		LifeTime -= dt;
		Position += Vector2.FromAngle(Rotation) * Speed * dt;
		Speed += dt * Acceleration;
		
		if (LifeTime <= 0)
			QueueFree();
	}
}
