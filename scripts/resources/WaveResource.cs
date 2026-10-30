using Godot;

[GlobalClass]
public partial class WaveResource : Resource
{
    [Export] public UnitResource[] Resource;
    [Export] public int[] BetweenSpawnTime;
}