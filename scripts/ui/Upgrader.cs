using Godot;
using System.Collections.Generic;

public partial class Upgrader : Control
{
    [Export] private ColorRect rext;
    
    private List<UpgradeButton> _buttons = [];
    private List<Line2D> _connectionLines = [];

    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is not UpgradeButton btn) continue;
            _buttons.Add(btn);
                
            btn.Buy += () => OnUpgradeBought(btn);
        }

        RefreshButtonsAndLines();
    }

    private void OnUpgradeBought(UpgradeButton boughtButton)
    {
        if (boughtButton.Unlocker != null) {
            foreach (var targetBtn in boughtButton.Unlocker) {
                if (targetBtn != null)
                    targetBtn.isUnlocked = true;
            }
        }

        RefreshButtonsAndLines();
    }

    private void RefreshButtonsAndLines()
    {
        foreach (var btn in _buttons)
            btn.UpdateVisualState();
        
        foreach (var line in _connectionLines)
            line.QueueFree();
        
        _connectionLines.Clear();

        foreach (var btn in _buttons) {
            if (btn.Unlocker == null || !btn.Visible) continue;
            
            foreach (var targetBtn in btn.Unlocker) {
                if (targetBtn == null || !targetBtn.Visible) continue;

                Line2D line = new Line2D();
                line.Width = 3.0f;
                line.DefaultColor = new Color(0.4f, 0.7f, 1.0f, 0.6f);

                Vector2 startPos = btn.Position+ btn.Size / 2f;
                Vector2 endPos = targetBtn.Position + targetBtn.Size / 2f;

                line.AddPoint(startPos);
                line.AddPoint(endPos);

                rext.AddChild(line);
                _connectionLines.Add(line);
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("f"))
        {
            Visible = !Visible;
        }
    }
}