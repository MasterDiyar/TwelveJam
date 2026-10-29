using System;
using Godot;
using TwelveJam.scripts.game;
using TwelveJam.scripts.units;

namespace TwelveJam.scripts.buildings;
public partial class Building : StaticBody2D, IDamagable
{
    [Signal] public delegate void OnClickEventHandler(Building building);
    
    [Export] public Faction Faction;
    [Export] public float Hp = 100;
    [Export] public float RotationSpeed = Single.Pi;
    [Export] public Node2D RotatingNode;
    public float Ferrum, Titan, Silicon, Sulfur;
    public Faction GetFaction() => Faction;

    [Export]public Vector2 AmplitudeMinimal;
    public bool Clickable = true;

    private RandomNumberGenerator _rng = new();
    public override void _Ready()
    {
        base._Ready();
        _rng.Randomize();

        Ferrum =  _rng.RandiRange(20, 120);
        Titan =   _rng.RandiRange(0, 80);
        Silicon = _rng.RandiRange(10, 115);
        Sulfur =  _rng.RandiRange(0, 900);
    }

    public void TakeDamage(float amount, Faction attackerFaction)
    {
        if (Faction == attackerFaction) return;
        Hp -= amount;
        if (Hp <= 0)
            OnDie();
    }

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left } && Clickable) {
            GameManager.Instance.OpenInfoForBuilding(this);
            EmitSignal(SignalName.OnClick, this);
        }
    }

    public virtual void OnDie()
    {
        QueueFree();
    }

    public override void _Process(double delta)
    {
        RotatingNode.Rotation += RotationSpeed * (float)delta;
    }

    public Items GetItem()
    {
        return new Items(Ferrum, Titan, Silicon, Sulfur);
    }
}