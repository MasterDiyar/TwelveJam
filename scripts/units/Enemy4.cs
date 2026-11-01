using Godot;
using System;
using TwelveJam.scripts.units;

public partial class Enemy4 : Unit
{
    [Export] float closeRange = 100f;
    [Export] private Weapon[] weps; 
    [Export] CpuParticles2D cpuParticles2D;
    [Export] Sprite2D sprite;
    private Player _player;
    private float _modulation = 0;
    
    public override void _Ready()
    {
        _player = GameManager.Instance.Player;
    }

    public override void _PhysicsProcess(double delta)
    {
        Rotation = (_player.GlobalPosition - GlobalPosition).Angle();
       
        if (GlobalPosition.DistanceTo(_player.GlobalPosition) > closeRange) {
            Velocity = Speed * Vector2.FromAngle(Rotation);
            cpuParticles2D.Emitting = true;
            _modulation = Mathf.Clamp(_modulation + (float)delta, 0f, 1f);
        }else {
            Velocity = Vector2.Zero; 
            if (weps != null)
            {
                foreach (var wep in weps)
                {
                    if (wep != null)
                    {
                        wep.Attack((_player.GlobalPosition - wep.GlobalPosition).Angle());
                    }
                }
            }
          
            cpuParticles2D.Emitting = false;
            _modulation = Mathf.Clamp(_modulation - 6 * (float)delta, 0f, 1f);
        }
       
        sprite.Modulate = sprite.Modulate with { A = _modulation };
       
        MoveAndSlide();
    }
}