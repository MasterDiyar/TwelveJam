using TwelveJam.scripts.units;

namespace TwelveJam.scripts.weapons;

using Godot;
using System;

public partial class LaserBeamWeapon : Node2D
{
    [Export] private RayCast2D _rayCast;
    [Export] private Line2D _lineVisual;    
    [Export] private Line2D _coreLineVisual; 
    
    [Export] private Faction _myFaction = Faction.Player;
    [Export] private float _beamDuration = 2.0f;
    [Export] private float _cooldownDuration = 3.0f; 
    [Export] private float _damagePerSecond = 10.0f;
    [Export] private float _maxRange = 500f; 

    [Export] private float _minWidth = 2.0f;
    [Export] private float _maxWidth = 12.0f;
    [Export] public Sprite2D lighter;
    private float lightStrength = 0;

    private float _timer = 0f;
    private bool _isFiring = false;
    private bool _isCooldown = false;
    private float _currentAngle = 0f;

    public override void _Ready()
    {
        if (_rayCast != null) {
            _rayCast.Enabled = false;
            _rayCast.TargetPosition = new Vector2(_maxRange, 0);
        }
        if (_lineVisual != null) _lineVisual.Visible = false;
        if (_coreLineVisual != null) _coreLineVisual.Visible = false;
    }

    public override void _Process(double delta)
    {
        if (_isCooldown) {
            _timer -= (float)delta;
            if (_timer <= 0) _isCooldown = false;
            return;
        }

        if (_isFiring) {
            _timer -= (float)delta;
            UpdateBeam(delta);
            lighter.Modulate = lighter.Modulate with { A = lightStrength};
            lightStrength += 6* (float)delta;
            if (_timer <= 0) {
                StopFiring();
                StartCooldown();
                lighter.Visible = false;
            }
        }
        Rotation = GetParent<Node2D>().Rotation;
    }

    public void StartFiring(float angleInDegrees)
    {
        if (_isCooldown || _isFiring) return;
        
        lighter.Visible = true;
        lighter.Scale = Vector2.One * .9f;
        lightStrength = 2.6f;

        _isFiring = true;
        _timer = _beamDuration;
        _currentAngle = angleInDegrees;

        Rotation = _currentAngle;
        
        if (_rayCast != null) _rayCast.Enabled = true;
        if (_lineVisual != null) _lineVisual.Visible = true;
        if (_coreLineVisual != null) _coreLineVisual.Visible = true;
    }

    private void StopFiring()
    {
        _isFiring = false;
        if (_rayCast != null) _rayCast.Enabled = false;
        if (_lineVisual != null) _lineVisual.Visible = false;
        if (_coreLineVisual != null) _coreLineVisual.Visible = false;
    }

    private void StartCooldown()
    {
        _isCooldown = true;
        _timer = _cooldownDuration;
    }

    private void UpdateBeam(double delta)
    {
        if (_rayCast == null) return;

        var spaceState = GetWorld2D().DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GlobalPosition + Vector2.Right.Rotated(Rotation) * _maxRange);
        
        Godot.Collections.Dictionary result = default;
        
        for (int i = 0; i < 5; i++) {
            result = spaceState.IntersectRay(query);
            if (result.Count > 0) {
                var collider = result["collider"].As<Node>();

                if (collider is IDamagable factionObj) {
                    if (factionObj.GetFaction() == _myFaction) {
                        query.Exclude = new Godot.Collections.Array<Rid> { (Rid)result["rid"] };
                        continue;
                    }
                } break;
            }else { break; }
        }

        Vector2 targetPoint;
        if (result.Count > 0)
        {
            targetPoint = (Vector2)result["position"];
            var collider = result["collider"].As<Node>();

            if (collider is IDamagable enemy) 
            {
                enemy.TakeDamage(_damagePerSecond * (float)delta, _myFaction);
            }
        }
        else
        {
            targetPoint = GlobalPosition + Vector2.Right.Rotated(Rotation) * _maxRange;
        }

        // Вычисляем прогресс стрельбы от 0 до 1 (чтобы ширина росла по мере удержания луча)
        float progress = 1.0f - (_timer / _beamDuration);
        float currentDynamicWidth = Mathf.Lerp(_minWidth, _maxWidth, progress);
        lighter.Scale += Vector2.One *  (float) delta;

        DrawVisualBeam(targetPoint, currentDynamicWidth);
    }

    private void DrawVisualBeam(Vector2 targetGlobalPoint, float dynamicWidth)
    {
        if (_lineVisual != null)
        {
            _lineVisual.Width = dynamicWidth;
            _lineVisual.ClearPoints();
            _lineVisual.AddPoint(Vector2.Zero);
            _lineVisual.AddPoint(_lineVisual.ToLocal(targetGlobalPoint));
        }

        if (_coreLineVisual != null)
        {
            _coreLineVisual.Width = Mathf.Max(1.0f, dynamicWidth * 0.4f);
            _coreLineVisual.ClearPoints();
            _coreLineVisual.AddPoint(Vector2.Zero);
            _coreLineVisual.AddPoint(_coreLineVisual.ToLocal(targetGlobalPoint));
        }
    }
}