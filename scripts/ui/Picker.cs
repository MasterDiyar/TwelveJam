using Godot;
using System;

public partial class Picker : Sprite2D
{
    private float dt =0;

    private float Amplitude;

    public float Minimal = 0.25f;
    public override void _Process(double delta)
    {
        dt += (float)delta;
        Scale = Vector2.One * Mathf.Cos(dt) * Amplitude + Vector2.One*Minimal;
    }

    public void SetPicker(Vector2 position, float amplitude, float minimal)
    {
        Amplitude = amplitude;
        Position = position;
        Minimal = minimal;
    }
}
