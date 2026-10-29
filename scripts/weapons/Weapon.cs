using Godot;
using System;
using TwelveJam.scripts.units;
using TwelveJam.scripts.buildings; // Если фракция берется отсюда, либо подключите свой namespace

public partial class Weapon : Node2D
{
    [Export] public PackedScene BulletScene;
    [Export] public Faction WeaponFaction = Faction.Player;

    [ExportGroup("Attack Stats")]
    [Export] public float Damage = 10f;
    [Export] public float Speed = 560f;
    [Export] public float Acceleration = 0f;
    [Export] public float LifeTime = 2f;
    
    [ExportGroup("Burst & Spread")]
    [Export] public int BulletsCount = 1;          // Сколько патронов за 1 атаку
    [Export] public float AngleStep = 0.05f;       // Угол между патронами в очереди (в радианах)
    [Export] public float BetweenShotTime = 0.0f;  // Время между патронами (0 = берст в 1 кадр)
    [Export] public Vector2 SpawnOffset = new(20f, 0f); // Оффсет спавна относительно оружия

    [ExportGroup("Timers")]
    [Export] public Timer BetweenAttackTimer;

    [Export] public float AttackSpeed = 1;
    [Export] public GpuParticles2D Particle;
    
    public bool CanShoot = true;

    private float _currentBaseAngle;
    private int _bulletsLeftInBurst = 0;
    private float _shotTimer = 0f;

    public override void _Ready()
    {
        if (BetweenAttackTimer != null)
        {
            BetweenAttackTimer.Timeout += OnAttackTimerTimeout;
            BetweenAttackTimer.WaitTime = AttackSpeed;
        }
    }

    public override void _Process(double dt)
    {
        if (_bulletsLeftInBurst > 0)
        {
            _shotTimer -= (float)dt;
            if (_shotTimer <= 0)
            {
                ShootSingleBullet(_currentBaseAngle, _bulletsLeftInBurst);
                _bulletsLeftInBurst--;
                _shotTimer = BetweenShotTime;
            }
        }
    }

    public void Attack(float angle)
    {
        if (!CanShoot) return;
        CanShoot = false;
        if (Particle != null)
            Particle.Emitting = true;
        

        if (BetweenAttackTimer != null)
        {
            BetweenAttackTimer.Start();
        }

        _currentBaseAngle = angle;
        if (BetweenShotTime <= 0f)
        {
            for (int i = 0; i < BulletsCount; i++)
                ShootSingleBullet(_currentBaseAngle, i);
            
        }else{
            _bulletsLeftInBurst = BulletsCount;
            _shotTimer = 0f;
        }
    }

    private void ShootSingleBullet(float baseAngle, int bulletIndex)
    {
        if (BulletScene == null) return;
        float offsetAngle = (bulletIndex - (BulletsCount - 1) / 2f) * AngleStep;
        float finalAngle = baseAngle + offsetAngle;

        var bulletNode = BulletScene.Instantiate();
        if (bulletNode is Bullet bullet)
        {
            Vector2 rotatedOffset = SpawnOffset.Rotated(Rotation + finalAngle);
            bullet.GlobalPosition = GlobalPosition + rotatedOffset;

            bullet.SetWeapon(WeaponFaction, Damage, Speed, Acceleration, Rotation + finalAngle, LifeTime);

            GameManager.Instance.Game.Pausable.AddChild(bullet);
        }
    }

    private void OnAttackTimerTimeout()
    {
        CanShoot = true;
    }
}