using Godot;

namespace TwelveJam.scripts.units;

public partial class Unit : CharacterBody2D, IDamagable
{
    [Export] public Faction Faction;

    [Export] public float Hp = 100;
    
    
    public Faction GetFaction() => Faction;

    public void TakeDamage(float amount, Faction attackerFaction)
    {
        if (attackerFaction == Faction) return;
        Hp -= amount;
        if (Hp <= 0)
            OnDie();
    }

    public virtual void OnDie()
    {
        QueueFree();
    }
    
}