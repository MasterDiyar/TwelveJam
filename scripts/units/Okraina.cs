using Godot;
using System.Collections.Generic;
using TwelveJam.scripts.units;

public partial class Okraina : Unit
{
    [Export] private CpuParticles2D _cpuParticles2D;
    [Export] private Sprite2D _sprite;
    [Export] private Area2D _scaner;

    private List<Unit> _targets = [];
    
    private float _circleTime = 0f;

    public override void _Ready()
    {
        base._Ready();

        if (_scaner != null) {
            _scaner.BodyEntered += OnBodyEntered;
            _scaner.BodyExited += OnBodyExited;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        _targets.RemoveAll(t => !GodotObject.IsInstanceValid(t));

        if (_targets.Count > 0) {
            Unit target = _targets[0];
            Vector2 direction = (target.GlobalPosition - GlobalPosition).Normalized();
            Velocity = direction * Speed;
        }else {
            float radius = (Speed * 3f) / (2f * Mathf.Pi);

            _circleTime += (float)delta;
            float angle = _circleTime * (2f * Mathf.Pi / 3f);

            Vector2 targetPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            Vector2 direction = (targetPos - GlobalPosition).Normalized();
            Velocity = direction * Speed;
        }
        MoveAndSlide();
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Unit unit && unit != this && unit.GetFaction() != Faction) {
            if (!_targets.Contains(unit))
                _targets.Add(unit);
            
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Unit unit)
        {
            _targets.Remove(unit);
        }
    }
}