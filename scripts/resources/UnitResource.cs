using Godot;

[GlobalClass]
public partial class UnitResource : Resource
{
    [Export] public PackedScene Unit;
    [Export] public int Count =1;
}