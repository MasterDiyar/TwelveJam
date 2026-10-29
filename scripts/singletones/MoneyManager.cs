using Godot;
using System;
using TwelveJam.scripts.game;

public partial class MoneyManager : Node
{
	[Signal] public delegate void MoneyChangeEventHandler(float currentMoney, float consumed);

	public Items Items = new Items();
	
	public Items QuoteItems = new Items(10,10,10,10), QuoteNeed = new Items(10,10,10,10);
	
	public static MoneyManager Instance;

	public float Money = 100;
	
	
	public override void _Ready()
	{
		Instance = this;
	}
	
	public string GetMoney() => $"{Money:F1}";

	public bool CanConsumeMoney(float count) => count <= Money;

	public bool CanConsumeResources(Items item)
	{
		return (Items >= item);
	}

	public void ConsumeResources(Items item)
	{
		Items -= item;
	}

	public void Daychanged()
	{
		Items -= QuoteNeed;
		if (Items.AnyNegative())
		{
			GameManager.Instance.ExecuteGameOver();
			return;
		}
		QuoteItems *= 2.5f/GameManager.Instance.CurrentDay+1;
		QuoteNeed = QuoteItems * 1;
	}
	
	public void ResetGame()
	{
		Money = 100f;

		Items =new Items(0,0,0,0); 
		QuoteItems = new Items(10,10,10,10); 
		QuoteNeed = QuoteItems*1;
		
	}
}
