namespace TwelveJam.scripts.units;

public interface IDamagable
{
    Faction GetFaction();
    void TakeDamage(float amount, Faction attackerFaction);
}