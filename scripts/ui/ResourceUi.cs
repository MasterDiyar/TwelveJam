using Godot;
using System;

public partial class ResourceUi : Control
{
	[Export] public Label Text, QuoteText, HpNum, TimerLabel;
	[Export] public Line2D HpLine;
	MoneyManager mm = MoneyManager.Instance;
	Player player;
	public float Time = 80;
	public override void _Ready()
	{
		mm??=MoneyManager.Instance;
		player = GameManager.Instance.Player;
	}

	public override void _Process(double delta)
	{
		
		Text.Text = $"Fe: {mm.Items.Ferrum}\n" +
		            $"Ti: {mm.Items.Titan}\n" +
		            $"Si: {mm.Items.Silicon}\n" +
		            $"S:  {mm.Items.Sulfur}";
		
		QuoteText.Text = "Quote:\n" +
					$"Fe: {mm.QuoteItems.Ferrum}\n" +
		            $"Ti: {mm.QuoteItems.Titan}\n" +
		            $"Si: {mm.QuoteItems.Silicon}\n" +
		            $"S:  {mm.QuoteItems.Sulfur}";
		
		HpLine.SetPointPosition(1, new Vector2(Mathf.Clamp(player.Hp/player.MaxHp, 0, 1)*200+10,25));
		HpNum.Text = $"{player.Hp:F1}/{player.MaxHp}";
		
		Time -= (float)delta;
		TimerLabel.Text = $"{Time:N0}";

		if (Time < 0)
		{
			GameManager.Instance.CurrentDay++;
			mm.Daychanged();
			Time = 60 + 10*GameManager.Instance.CurrentDay;
		}
	}
}
