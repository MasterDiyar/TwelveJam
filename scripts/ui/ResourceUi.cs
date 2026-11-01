using Godot;
using System;

public partial class ResourceUi : Control
{
	[Export] public Label Text, QuoteText, HpNum, TimerLabel, Alert, Day;
	[Export] public Line2D HpLine;
	[Export] public Color NormalHp, RadiationalHp;
	MoneyManager mm = MoneyManager.Instance;
	Player player;
	public float Time = 40, _radT = 0;
	public bool isStarted = false;
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
		
		if (GameManager.Instance.Game.Pausable.ProcessMode != ProcessModeEnum.Disabled)
			if (isStarted) Time -= (float)delta;
		TimerLabel.Text = $"{Time:N0}";
		Day.Text = $"Day: {GameManager.Instance.CurrentDay}";

		if (Time < 0)
		{
			GameManager.Instance.ExecuteChangeDay();
			Time = 30 + 5*GameManager.Instance.CurrentDay;
		}
		
		HpLine.DefaultColor = player.OnRadiation ? RadiationalHp : NormalHp;
		Alert.Visible = player.OnRadiation;
		if (!player.OnRadiation) return;
		_radT += 5* (float)delta;
		float alpha = Mathf.Lerp(0.01f, 1.0f, (MathF.Cos(_radT) + 1.0f) * 0.5f);
		Alert.Modulate = Alert.Modulate with {A = Mathf.Cos(alpha)};
	}
}
