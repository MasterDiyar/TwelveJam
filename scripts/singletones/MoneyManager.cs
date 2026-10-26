using Godot;
using System;
using TwelveJam.scripts.game;

public partial class MoneyManager : Node
{
	[Signal] public delegate void MoneyChangeEventHandler(float currentMoney, float consumed);

	public Items Items;
	
	public static MoneyManager Instance;

	public float Money = 100;
	
	public override void _Ready()
	{
		Instance = this;
	}
	
	public string GetMoney() => $"{Money:F1}";

	public bool CanConsumeMoney(float count) => count >= Money;

	public bool CanConsumeResources(Items item)
	{
		return (Items > item);
	}

	public void ConsumeResources(Items item)
	{
		Items -= item;
	}
	
}
