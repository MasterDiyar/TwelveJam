using TwelveJam.scripts.units;

namespace TwelveJam.scripts.weapons;

using Godot;
using System;

public partial class LaserBeamWeapon : Node2D
{
    [Export] private RayCast2D _rayCast;
    [Export] private Line2D _lineVisual;
    [Export] private Faction _myFaction = Faction.Player;
    [Export] private float _beamDuration = 2.0f; // Сколько секунд светит
    [Export] private float _cooldownDuration = 3.0f; // Перезарядка
    [Export] private float _damagePerSecond = 10.0f;
    [Export] private float _maxRange = 500f; // Максимальная длина луча

    private float _timer = 0f;
    private bool _isFiring = false;
    private bool _isCooldown = false;
    private float _currentAngle = 0f;

    public override void _Ready()
    {
        if (_rayCast != null)
        {
            _rayCast.Enabled = false;
            _rayCast.TargetPosition = new Vector2(_maxRange, 0);
        }
        if (_lineVisual != null) _lineVisual.Visible = false;
    }

    public override void _Process(double delta)
    {
        if (_isCooldown)
        {
            _timer -= (float)delta;
            if (_timer <= 0) _isCooldown = false;
            return;
        }

        if (_isFiring)
        {
            _timer -= (float)delta;
            UpdateBeam();

            if (_timer <= 0)
            {
                StopFiring();
                StartCooldown();
            }
        }
    }

    /// <summary>
    /// Запуск луча с заданным углом атаки
    /// </summary>
    public void StartFiring(float angleInDegrees)
    {
        if (_isCooldown || _isFiring) return;

        _isFiring = true;
        _timer = _beamDuration;
        _currentAngle = angleInDegrees;

        // Поворачиваем орудие или RayCast под переданный угол
        Rotation = Mathf.DegToRad(angleInDegrees);
        
        if (_rayCast != null) _rayCast.Enabled = true;
        if (_lineVisual != null) _lineVisual.Visible = true;
    }

    private void StopFiring()
    {
        _isFiring = false;
        if (_rayCast != null) _rayCast.Enabled = false;
        if (_lineVisual != null) _lineVisual.Visible = false;
    }

    private void StartCooldown()
    {
        _isCooldown = true;
        _timer = _cooldownDuration;
    }

    private void UpdateBeam()
    {
        if (_rayCast == null) return;

        // Логика сквозного пролета через одинаковый Faction
        // Так как стандартный RayCast2D возвращает первое же попавшееся тело,
        // для точного игнорирования союзников используем PhysicsDirectSpaceState2D или цикл исключений.
        
        var spaceState = GetWorld2D().DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GlobalPosition + Vector2.Right.Rotated(Rotation) * _maxRange);
        
        // Можем исключать сам корабль из запроса
        // query.Exclude = ... 

        // Итеративный пролет сквозь союзников (до 5 попыток за кадр, чтобы не уйти в бесконечный цикл)
        Godot.Collections.Dictionary result = default;
        for (int i = 0; i < 5; i++)
        {
            result = spaceState.IntersectRay(query);
            if (result.Count > 0)
            {
                var collider = result["collider"].As<Node>();

                if (collider is IDamagable factionObj)
                {
                    if (factionObj.GetFaction() == _myFaction)
                    //query.Exclude.Add(collider.);
                        continue;
                    else
                    {
                        factionObj.TakeDamage(_damagePerSecond, _myFaction);
                    }
                }
                
                // Если это враг или нейтральный объект — останавливаем луч здесь
                break;
            }
            else
            {
                break;
            }
        }

        // Отрисовка и нанесение урона по финальной точке
        if (result.Count > 0)
        {
            Vector2 collisionPoint = (Vector2)result["position"];
            var collider = result["collider"].As<Node>();

            // Наносим урон, если это враг
            // if (collider is Enemy enemy) enemy.TakeDamage(_damagePerSecond * (float)delta);

            DrawVisualBeam(collisionPoint);
        }
        else
        {
            // Ни в кого не попали, луч летит на максимум
            Vector2 maxPoint = GlobalPosition + Vector2.Right.Rotated(Rotation) * _maxRange;
            DrawVisualBeam(maxPoint);
        }
    }

    private void DrawVisualBeam(Vector2 targetGlobalPoint)
    {
        if (_lineVisual == null) return;

        _lineVisual.ClearPoints();
        _lineVisual.AddPoint(Vector2.Zero);
        _lineVisual.AddPoint(_lineVisual.ToLocal(targetGlobalPoint));
    }
}
