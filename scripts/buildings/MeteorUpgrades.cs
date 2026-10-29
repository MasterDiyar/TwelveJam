using Godot;
using System;
using TwelveJam.scripts.buildings;
using TwelveJam.scripts.game;

public partial class MeteorUpgrades : HBoxContainer
{
	private Building parent;
	[Export] private Button demolish, excavate, build;
	public override void _Ready()
	{
		parent =  (Building)GetParent();
		parent.OnClick += ParentOnOnClick;	
		
		demolish.Pressed += DemolishOnPressed;
		excavate.Pressed += ExcavateOnPressed;
		build.Pressed += BuildOnPressed;	
		
	}

	private void BuildOnPressed()
	{
		var Basement = GD.Load<PackedScene>("res://scenes/buildings/base.tscn").Instantiate<Base>();
		Basement.GlobalPosition = parent.GlobalPosition;
		
		GameManager.Instance.Picker.Visible = false;
		GameManager.Instance.InfoTablet.Visible = false;
		
		parent.GetParent().AddChild(Basement);
		
		parent.QueueFree();
	}


	private void ExcavateOnPressed()
	{
		var Drone = GD.Load<PackedScene>("res://scenes/units/burovik.tscn").Instantiate<Burovik>();
		Drone.building = parent;
		parent.AddChild(Drone);
		parent.Clickable = false;
		
		GameManager.Instance.Picker.Visible = false;
		GameManager.Instance.InfoTablet.Visible = false;
		
		Visible = false;
	}

	private void DemolishOnPressed()
	{
		MoneyManager.Instance.Items += parent.GetItem()*GameManager.Instance.DemolishMultiplier;

		var a = GD.Load<PackedScene>("res://scenes/particles/smoke.tscn").Instantiate<CpuParticles2D>();
		a.GlobalPosition = parent.GlobalPosition;
		
		GameManager.Instance.Picker.Visible = false;
		GameManager.Instance.InfoTablet.Visible = false;
		
		parent.GetParent().CallDeferred("add_child", a);
		
		parent.QueueFree();
	}
	
	

	private void ParentOnOnClick(Building building)
	{
		Visible = true;
		GameManager.Instance.BuildingChanged += undetec;
		excavate.TooltipText = $"Excavate up to {GameManager.Instance.DemolishMultiplier*100}% of all ore";
	}

	void undetec(Building building1)
	{
		if (!IsInstanceValid(this)) return;
		if (building1 != parent) Visible = false;
	}
}
