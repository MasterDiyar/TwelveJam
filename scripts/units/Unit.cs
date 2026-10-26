using Godot;

namespace TwelveJam.scripts.units;

public partial class Unit : CharacterBody2D, IDamagable
{
    [Export] public Faction Faction;

    public Faction GetFaction() => Faction;

    public void TakeDamage(float amount, Faction attackerFaction)
    {
        if (attackerFaction == Faction) return;
    }
}