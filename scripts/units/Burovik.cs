using Godot;
using System;
using TwelveJam.scripts.buildings;
using TwelveJam.scripts.units;

public partial class Burovik : Unit
{
    [Export] public Building building;
    [Export] private AnimationPlayer anim;
    private int WorkTime = 0;
    public override void _Ready()
    {
        base._Ready();
        
        anim.Play("mine");
        anim.AnimationFinished += AnimOnAnimationFinished;
    }

    private void AnimOnAnimationFinished(StringName animName)
    {
        if (!IsInstanceValid(building)) return;
        if (animName == "mine")
        {
            WorkTime++;
            if (WorkTime >= 6) {
                MoneyManager.Instance.Items += building.GetItem();
                anim.AnimationFinished -= AnimOnAnimationFinished;
                building.QueueFree();
                return;
            }
            anim.Play("mine");
        }
    }

    public override void OnDie()
    {
        building.QueueFree();
    }
}
