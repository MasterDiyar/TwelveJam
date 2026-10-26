using Godot;
using System;
using TwelveJam.scripts.game;

public partial class UpgradeButton : Button
{
	[Signal] public delegate void BuyEventHandler();
	[Export] public UpgradeButton[] Unlocker;
	[Export] public bool isUnlocked = false;
	[Export] public int Id = 0; 
	[Export] public float Ferrum, Titan, Silicon, Sulfur;
	[Export] public float Money;
	public override void _Ready()
	{
		Pressed += OnPress;
	}

	void OnPress()
	{
		var cons = new Items {Ferrum = Ferrum, Titan= Titan, Silicon = Silicon, Sulfur = Sulfur};
		if (!MoneyManager.Instance.CanConsumeResources(cons))return;
		if (!MoneyManager.Instance.CanConsumeMoney(Money))return;
		MoneyManager.Instance.ConsumeResources(cons);
		MoneyManager.Instance.Money -= Money;
			
		AfterBuy();
		EmitSignalBuy();
	}
	
	
	

	void AfterBuy()
	{
		switch (Id)
		{
			case 1: 
				break;
		}
	}
}
