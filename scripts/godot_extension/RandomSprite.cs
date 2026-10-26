using System.Linq;
using Godot;

[GlobalClass]
public partial class RandomSprite : Sprite2D
{
    [Export] private Texture2D[] sprites;
    [Export] private float[] chances;

    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        if (sprites == null || sprites.Length == 0)
            return;
        
        if (chances == null || chances.Length == 0 || chances.Length != sprites.Length)
            Texture = sprites[_rng.Randi() % sprites.Length];
        else {
            float totalChance = chances.Sum();

            if (totalChance <= 0f)
            {
                Texture = sprites[_rng.Randi() % sprites.Length];
                return;
            }

            float randomValue = _rng.RandfRange(0, totalChance);
            float currentSum = 0f;

            for (int i = 0; i < sprites.Length; i++) {
                currentSum += chances[i];
                if (!(randomValue <= currentSum)) continue;
                Texture = sprites[i];
                break;
            }
        }
    }
}