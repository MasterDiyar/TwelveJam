using Godot;
using System;
using TwelveJam.scripts.buildings;
using TwelveJam.scripts.game;

public partial class BaseUpgrader : Control
{
	[Export] public Button Tower, Kazarma, Demolish;

	private Base parent;
	bool towerplaced = false;
	public override void _Ready()
	{
		parent =  (Base)GetParent();
		parent.OnClick += ParentOnOnClick;	
		
		Tower.Pressed += TowerOnPressed;
		Demolish.Pressed += DemolishOnPressed;
	}

	private void DemolishOnPressed()
	{
		MoneyManager.Instance.Items += parent.GetItem() + ((towerplaced) ? new Items(30, 15, 20, 0) : Items.One());
		var a = GD.Load<PackedScene>("res://scenes/particles/smoke.tscn").Instantiate<CpuParticles2D>();
		a.GlobalPosition = parent.GlobalPosition;
		
		GameManager.Instance.Picker.Visible = false;
		GameManager.Instance.InfoTablet.Visible = false;
		
		parent.GetParent().CallDeferred("add_child", a);
		
		parent.QueueFree();
	}

	private void TowerOnPressed()
	{
		if (towerplaced) return;
		if (MoneyManager.Instance.CanConsumeResources(new Items(30, 15, 20, 0))) 
			MoneyManager.Instance.ConsumeResources(new Items(30, 15, 20, 0));
		else return;
		towerplaced = true;
		var ep = GD.Load<PackedScene>("res://scenes/weapon/turret.tscn").Instantiate<Weapon>();
		parent.AddChild(ep);
		parent.weapon = ep;
		Tower.QueueFree();
		Tower.Pressed -= TowerOnPressed;
		Tower = null;
	}


	private void ParentOnOnClick(Building building)
	{
		Visible = true;
		GameManager.Instance.BuildingChanged += undetec;
	}
	
	void undetec(Building building1)
	{
		if (!IsInstanceValid(this)) return;
		if (building1 != parent) Visible = false;
	}
}
