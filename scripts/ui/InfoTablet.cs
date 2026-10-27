using Godot;
using System;

public partial class InfoTablet : Node2D
{
    [Export] private Line2D[] _lines = new Line2D[4];
    [Export] private Label[] _labels = new Label[4];

    [ExportGroup("Animation Settings")]
    [Export] private float _animationDurationPerItem = 0.4f; 
    [Export] private float _sineAmplitude = 2.0f;            
    [Export] private float _sineFrequency = 5.0f;           

    private float[] _targetValues = new float[4];
    private float[] _currentValues = new float[4];
    
    private float[] _phaseOffsets = new float[4] { 0.0f, 1.57f, 3.14f, 4.71f }; 

    private bool _isAnimating = false;
    private int _currentIndex = 0;
    private float _timer = 0.0f;

    private Vector2[][] _baseLinePoints = new Vector2[4][];
    
    private float[] _currentScaledLengths = new float[4];

    public override void _Ready()
    {
        for (int i = 0; i < 4; i++) 
        {
            if (_lines[i] != null && _lines[i].GetPointCount() >= 2)
                _baseLinePoints[i] = _lines[i].GetPoints();
            
            if (_labels[i] != null) 
            {
                _labels[i].Text = "0";
            }
        }
        
        SetProcess(false); 
    }

    public void ShowData(float ferrum, float titan, float silicon, float sulfur)
    {
        _targetValues[0] = ferrum;
        _targetValues[1] = titan;
        _targetValues[2] = silicon;
        _targetValues[3] = sulfur;

        for (int i = 0; i < 4; i++) {
            _currentValues[i] = 0.0f;
            _currentScaledLengths[i] = 0.0f;
            if (_labels[i] != null)
                _labels[i].Text = "0";
        }

        _currentIndex = 0;
        _timer = 0.0f;
        _isAnimating = true;
        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        float fDelta = (float)delta;

        if (_isAnimating)
        {
            _timer += fDelta;
            float t = Mathf.Clamp(_timer / _animationDurationPerItem, 0.0f, 1.0f);
            
            _currentValues[_currentIndex] = Mathf.Lerp(0.0f, _targetValues[_currentIndex], t);
            _currentScaledLengths[_currentIndex] = _currentValues[_currentIndex] / 3.0f;

            if (_labels[_currentIndex] != null) {
                _labels[_currentIndex].Text = Mathf.RoundToInt(_currentValues[_currentIndex]).ToString();
            }
            
            if (t >= 1.0f) 
            {
                _currentValues[_currentIndex] = _targetValues[_currentIndex];
                _currentScaledLengths[_currentIndex] = _targetValues[_currentIndex] / 3.0f;
                
                if (_labels[_currentIndex] != null) {
                    _labels[_currentIndex].Text = Mathf.RoundToInt(_currentValues[_currentIndex]).ToString();
                }

                _currentIndex++;
                _timer = 0.0f;
                if (_currentIndex >= 4)
                    _isAnimating = false; 
            }
        }

        UpdateAndWaveLines();
    }

    private void UpdateAndWaveLines()
    {
        float timeSec = Time.GetTicksMsec() * 0.001f;

        for (int i = 0; i < 4; i++)
        {
            if (_lines[i] == null || _baseLinePoints[i] == null || _baseLinePoints[i].Length < 2) 
                continue;

            Vector2 startPoint = _baseLinePoints[i][0];
            Vector2 originalEnd = _baseLinePoints[i][1];
            Vector2 direction = (originalEnd - startPoint).Normalized();

            float length = _currentScaledLengths[i];
            Vector2 scaledEnd = startPoint + direction * length;

            float sineOffset = Mathf.Sin(timeSec * _sineFrequency + _phaseOffsets[i]) * _sineAmplitude;

            Vector2 finalStart = new Vector2(startPoint.X + sineOffset, startPoint.Y);
            Vector2 finalEnd = new Vector2(scaledEnd.X + sineOffset, scaledEnd.Y);

            _lines[i].SetPointPosition(0, finalStart);
            _lines[i].SetPointPosition(1, finalEnd);
        }
    }
}