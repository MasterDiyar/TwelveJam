using Godot;
using TwelveJam.scripts.units;

namespace TwelveJam.scripts.buildings;
public partial class Building : StaticBody2D, IDamagable
{
    [Signal] public delegate void OnClickEventHandler(Building building);
    
    [Export] public Faction Faction;
    [Export] public float Hp = 100;
    public float Ferrum, Titan, Silicon, Sulfur;
    public Faction GetFaction() => Faction;
    
    public void TakeDamage(float amount, Faction attackerFaction)
    {
        if (Faction == attackerFaction) return;
        Hp -= amount;
        if (Hp <= 0)
            OnDie();
    }

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
        {
            GameManager.Instance.Picker.GlobalPosition = GlobalPosition;
            GD.Print("Pressed");
            
            EmitSignal(SignalName.OnClick, this);
        }
        
    }

    public virtual void OnDie()
    {
        QueueFree();
    }
}