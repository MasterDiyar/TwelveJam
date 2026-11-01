using Godot;
using System;
using TwelveJam.scripts.game;

public partial class UpgradeButton : Button
{
	[Signal] public delegate void BuyEventHandler();

	[Export] public AudioStreamPlayer BuySound;
	[Export] public UpgradeButton[] Unlocker;
	[Export] public bool isUnlocked = false;
	[Export] public int Id = 0; 
	[Export] public float Ferrum, Titan, Silicon, Sulfur;
	[Export] public float Money;
	private Upgrader papa;
	bool isClicked = false;
	public override void _Ready()
	{
		papa = GetParent<Upgrader>();
		Pressed += OnPress;
		TooltipText = $"cost: {Money}\niron: {Ferrum}\ntitan: {Titan}\nsilicon: {Silicon}\nsulfur: {Sulfur}";
	}

	public void UpdateVisualState()
	{
		Visible = isUnlocked;
		Disabled = !isUnlocked;
	}

	void OnPress()
	{
		if (isClicked) return;
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
		BuySound.Play();
		isClicked =  true;
		Disabled = true;
		var player = GameManager.Instance.Player;
		switch (Id)
		{
			case -47:
				papa.Laser1.Damage += 10;
				papa.Laser2.Damage += 10;
				break;
			case 1:
				player.Speed += 50;
				break;
			case 2:
				var laser = GD.Load<PackedScene>("res://scenes/weapon/laser_weapon.tscn");
				var laser1 = laser.Instantiate<Weapon>();
				laser1.Position = new Vector2(15, -22);
				player.AddChild(laser1);
				player.Weapons.Add(laser1);
				papa.Laser1 = laser1;
				
				var laser2 = laser.Instantiate<Weapon>();
				laser2.Position = new Vector2(15, 22);
				player.AddChild(laser2);
				player.Weapons.Add(laser2);
				papa.Laser2 = laser2;
				
				player.Sprite.Texture = GD.Load<Texture2D>("res://assets/textures/player/player_test2.png");
				player.ThrusterParticles.Position += Vector2.Left * 14; 
				break;
			case 3:
				papa.Laser1.BulletsCount = 2;
				papa.Laser1.BetweenShotTime = 0.2f;
				papa.Laser1.BetweenAttackTimer.WaitTime += .1f;
				
				papa.Laser2.BulletsCount = 2;
				papa.Laser2.BetweenShotTime = 0.2f;
				papa.Laser2.BetweenAttackTimer.WaitTime += .1f;
				
				break;
			case 4:
				player.Sprite.Texture = GD.Load<Texture2D>("res://assets/textures/player/player_test4.png");
				player.MaxHp += 250f;
				player.accel += 0.1f;
				break;
			
			case 5:
				var spher = GD.Load<PackedScene>("res://scenes/weapon/circle_weapon.tscn").Instantiate<Weapon>();
				player.AddChild(spher);
				player.Weapons.Add(spher);
				papa.Spher = spher;
				break;
			
			case 7:
				player.accel += 0.1f;
				player.Speed += 40f;
				break;
			
			case 10:
				GameManager.Instance.MeteorPerDay++;
				break;
			case 11:
				GameManager.Instance.MeteorPerDay+=3;
				break;
			case 12:
				GameManager.Instance.DemolishMultiplier = .33f;
				break;
			
			case 897:
				player.Repair += 5f;
				break;
			
			case 15:
				papa.Spher.BetweenShotTime = 0.2f;
				papa.Spher.BulletsCount = 2;
				break;
			
			case 117:
				player.Repair += 10;
				break;
			
			case 1488:
				GameManager.Instance.MeteorPerDay+= 7;
				break;
		}
	}
}
