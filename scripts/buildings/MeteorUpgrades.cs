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
		
		
	}

	private void ExcavateOnPressed()
	{
		
	}

	private void DemolishOnPressed()
	{
		MoneyManager.Instance.Items += parent.GetItem()*GameManager.Instance.DemolishMultiplier;
	}
	
	

	private void ParentOnOnClick(Building building)
	{
		Visible = true;
		GameManager.Instance.BuildingChanged += undetec;
		excavate.TooltipText = $"Excavate up to {GameManager.Instance.DemolishMultiplier*100}% of all ore";
	}

	void undetec(Building building1)
	{
		if (building1 != parent) Visible = false;
	}
}
