using Godot;
using System;
using System.Collections.Generic;
using TwelveJam.scripts.units;

public partial class Player : Unit
{
    public float Speed = 300.0f;

    [Export] public CpuParticles2D ThrusterParticles; 
    [Export] private Camera2D _camera;                 
    
    
    [Export] private float _minZoom = 1.0f;
    [Export] private float _maxZoom = 3.0f; 
    [Export] private float _zoomSpeed = 0.2f;
    [Export] private float _zoomStep = 0.1f;

    [Export] private float _rotationSpeed = 10.0f, _firelightPower = 0;

    [Export] public Sprite2D Sprite, Firelight;
    
    public List<Weapon> Weapons = [];

    private float _targetZoom;

    public override void _Ready()
    {
        base._Ready();

        if (_camera != null)
        {
            _targetZoom = Mathf.Clamp(_camera.Zoom.X, _minZoom, _maxZoom);
        }

        if (ThrusterParticles != null)
        {
            ThrusterParticles.Emitting = false;
        }

        GameManager.Instance.Player = this;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed) {
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
                _targetZoom += _zoomStep;
            
            else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
                _targetZoom -= _zoomStep;
            
            _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        Vector2 direction = Input.GetVector("a", "d", "w", "s");
        if (direction != Vector2.Zero)
        {
            velocity = direction * Speed;

            if (ThrusterParticles != null && !ThrusterParticles.Emitting)
                ThrusterParticles.Emitting = true;
            
            _firelightPower = Mathf.Clamp(_firelightPower+(float)delta, 0.0f, 1.0f);
            
        }else {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
            
            _firelightPower = Mathf.Clamp(_firelightPower-5*(float)delta, 0.0f, 1.0f);
            if (ThrusterParticles != null && ThrusterParticles.Emitting)
            {
                ThrusterParticles.Emitting = false;
            }
        }

        Firelight.Modulate = Firelight.Modulate with { A = _firelightPower };
        Velocity = velocity;
        MoveAndSlide();

        if (velocity.LengthSquared() > 1.0f) {
            float targetAngle = velocity.Angle();

            Rotation = Mathf.LerpAngle(Rotation, targetAngle, (float)(_rotationSpeed * delta));
        }

        if (_camera != null) {
            float currentZoomX = Mathf.Lerp(_camera.Zoom.X, _targetZoom, (float)(_zoomSpeed * delta * 10.0));
            _camera.Zoom = new Vector2(currentZoomX, currentZoomX);
        }
        var angle = (-GlobalPosition + GetGlobalMousePosition()).Angle();
        if (Input.IsActionPressed("lm")) foreach (var wep in Weapons) {
            wep.Attack(angle);        
        }
    }
}