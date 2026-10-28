using Godot;
using System;

public partial class ResourceUi : Control
{
	[Export] public Label Text;
	MoneyManager mm = MoneyManager.Instance;

	public override void _Ready()
	{
		mm??=MoneyManager.Instance;
	}

	public override void _Process(double delta)
	{
		
		Text.Text = $"Fe: {mm.Items.Ferrum}\n" +
		            $"Ti: {mm.Items.Titan}\n" +
		            $"Si: {mm.Items.Silicon}\n" +
		            $"S:  {mm.Items.Sulfur}";
	}
}
