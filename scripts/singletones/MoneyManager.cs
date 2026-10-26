using Godot;
using System;

public partial class MoneyManager : Node
{
	[Signal] public delegate void MoneyChangeEventHandler(float currentMoney, float consumed);

	public float Ferrum, Titan, Silicon, Sulfur;
	
	public static MoneyManager Instance;

	public float Money = 100;
	
	public override void _Ready()
	{
		Instance = this;
	}
	
	public string GetMoney() => $"{Money:F1}";

	public bool CanConsume(float count) => count >= Money;
	
}
