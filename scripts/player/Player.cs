using Godot;
using System;
using TwelveJam.scripts.units;

public partial class Player : Unit
{
    public const float Speed = 300.0f;

    [Export] private CpuParticles2D _thrusterParticles; 
    [Export] private Camera2D _camera;                 

    
    [Export] private float _minZoom = 1.0f;
    [Export] private float _maxZoom = 3.0f; 
    [Export] private float _zoomSpeed = 0.2f;
    [Export] private float _zoomStep = 0.1f;

    [Export] private float _rotationSpeed = 10.0f;

    private float _targetZoom;

    public override void _Ready()
    {
        base._Ready();

        if (_camera != null)
        {
            _targetZoom = Mathf.Clamp(_camera.Zoom.X, _minZoom, _maxZoom);
        }

        if (_thrusterParticles != null)
        {
            _thrusterParticles.Emitting = false;
        }

        GameManager.Instance.Player = this;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
            {
                _targetZoom += _zoomStep;
            }
            else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
            {
                _targetZoom -= _zoomStep;
            }

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

            if (_thrusterParticles != null && !_thrusterParticles.Emitting)
                _thrusterParticles.Emitting = true;
            
        }else {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);

            if (_thrusterParticles != null && _thrusterParticles.Emitting)
            {
                _thrusterParticles.Emitting = false;
            }
        }

        Velocity = velocity;
        MoveAndSlide();

        if (velocity.LengthSquared() > 1.0f)
        {
            float targetAngle = velocity.Angle();

            Rotation = Mathf.LerpAngle(Rotation, targetAngle, (float)(_rotationSpeed * delta));
        }

        if (_camera != null)
        {
            float currentZoomX = Mathf.Lerp(_camera.Zoom.X, _targetZoom, (float)(_zoomSpeed * delta * 10.0));
            _camera.Zoom = new Vector2(currentZoomX, currentZoomX);
        }
    }
}