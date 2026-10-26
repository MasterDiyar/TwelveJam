using Godot;
using System;
using TwelveJam.scripts.units;

public partial class Player : Unit
{
    public const float Speed = 300.0f;

    // Экспортируемые переменные для настройки в инспекторе
    [Export] private CpuParticles2D _thrusterParticles; // Перетащите сюда ваш GPUParticles2D
    [Export] private Camera2D _camera;                 // Перетащите сюда вашу Camera2D

    // Настройки зума (от x3 до x2)
    [Export] private float _minZoom = 1.0f; // x2 (минимальное приближение / отдалено)
    [Export] private float _maxZoom = 3.0f; // x3 (максимальное приближение / крупно)
    [Export] private float _zoomSpeed = 0.2f;
    [Export] private float _zoomStep = 0.1f;

    // Скорость поворота корабля (чем больше число, тем быстрее поворачивается)
    [Export] private float _rotationSpeed = 10.0f;

    private float _targetZoom;

    public override void _Ready()
    {
        base._Ready();

        // Инициализируем зум текущим значением камеры
        if (_camera != null)
        {
            _targetZoom = Mathf.Clamp(_camera.Zoom.X, _minZoom, _maxZoom);
        }

        // Выключаем партиклы на старте
        if (_thrusterParticles != null)
        {
            _thrusterParticles.Emitting = false;
        }
    }

    public override void _Input(InputEvent @event)
    {
        // Обработка колесика мыши для зума камеры
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

            // Ограничиваем зум в диапазоне от x2 до x3
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