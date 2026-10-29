using Godot;
using System;
using TwelveJam.scripts.buildings;
using TwelveJam.scripts.game;
using TwelveJam.scripts.units;

public partial class Base : Building
{
    [Export] public Weapon weapon;
    [Export] protected Area2D area;
    [Export] public bool CanShoot = false;

    private Node2D _body;

    public void SetForConsume(float ferrum, float titan, float silicon, float sulfur)
    {
        Ferrum = ferrum;
        Titan = titan;
        Sulfur = sulfur;
        Silicon = silicon;
    }
    public override void _Ready()
    {
        area.BodyEntered += AreaOnBodyEntered;
    }

    private void AreaOnBodyEntered(Node2D body)
    {
        if (body is IDamagable f)
        {
            if (f.GetFaction() == Faction) return;
            _body = body;
        }
    }

    public override void _Process(double delta)
    {
        if (_body == null || !CanShoot) return;
        if (!IsInstanceValid(_body))
        {
            _body = null;
            return;
        }
        var a = (_body.GlobalPosition - GlobalPosition).Angle();
        weapon.Attack(a);
        weapon.Rotation = a;
    }
}
