using Godot;
using TwelveJam.scripts.units;

namespace TwelveJam.scripts.buildings;

public partial class Kazarma : Sprite2D
{
    [Export] public Timer tima;
    [Export] public PackedScene unit;

    private int CreatedCount = 0;
    private Node parent, grandParent;
    public override void _Ready()
    {
        tima.Start();
        parent = GetParent();
        grandParent = parent.GetParent();
        
        tima.Timeout += TimaOnTimeout;
    }

    private void TimaOnTimeout()
    {
        CreatedCount++;

        Unit mshkfrede = unit.Instantiate<Unit>();
        mshkfrede.GlobalPosition = GlobalPosition + Vector2.Right * 32;
        grandParent.AddChild(mshkfrede);
        
        
        if (CreatedCount > GameManager.Instance.MaxUnitsFromKazarma) {
            tima.Timeout -= TimaOnTimeout;
            tima.Stop();
            parent.QueueFree();
        }
    }
}