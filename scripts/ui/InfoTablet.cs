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
    
    // Фазы для каждой линии, чтобы они двигались не одновременно
    private float[] _phaseOffsets = new float[4] { 0.0f, 1.57f, 3.14f, 4.71f }; 

    private bool _isAnimating = false;
    private int _currentIndex = 0;
    private float _timer = 0.0f;

    private Vector2[][] _baseLinePoints = new Vector2[4][];

    public override void _Ready()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_lines[i] != null && _lines[i].GetPointCount() >= 2)
            {
                int count = _lines[i].GetPointCount();
                _baseLinePoints[i] = new Vector2[count];
                for (int p = 0; p < count; p++)
                {
                    _baseLinePoints[i][p] = _lines[i].GetPoints()[p];
                }
            }
            if (_labels[i] != null)
            {
                _labels[i].Text = "0";
            }
        }
        
        SetProcess(false); // Отключаем процесс до активации планшета
    }

    /// <dt>Запуск отображения данных из здания</dt>
    public void ShowData(float ferrum, float titan, float silicon, float sulfur)
    {
        _targetValues[0] = ferrum;
        _targetValues[1] = titan;
        _targetValues[2] = silicon;
        _targetValues[3] = sulfur;

        for (int i = 0; i < 4; i++)
        {
            _currentValues[i] = 0.0f;
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
            UpdateElementVisuals(_currentIndex, _currentValues[_currentIndex]);
            
            if (t >= 1.0f) {
                _currentValues[_currentIndex] = _targetValues[_currentIndex];
                UpdateElementVisuals(_currentIndex, _currentValues[_currentIndex]);
                _currentIndex++;
                _timer = 0.0f;
                if (_currentIndex >= 4)
                    _isAnimating = false; 
                
            }
        }

        ApplySineWaveToLines(fDelta);
    }

    private void UpdateElementVisuals(int index, float value)
    {
        if (_labels[index] != null)
        {
            _labels[index].Text = Mathf.RoundToInt(value).ToString();
        }

        // 2. Длина отрезка делится на 3 от количества ресурсов (value / 3.0f)
        // Модифицируем конечную точку Line2D на основе значения
        if (_lines[index] != null && _baseLinePoints[index] != null && _baseLinePoints[index].Length >= 2)
        {
            Vector2 startPoint = _baseLinePoints[index][0];
            Vector2 originalEnd = _baseLinePoints[index][1];
            Vector2 direction = (originalEnd - startPoint).Normalized();
            
            // Рассчитываем новую длину: базовая длина масштабируется от значения ресурса, деленного на 3
            float scaledLength = (value / 3.0f);
            Vector2 newEnd = startPoint + direction * scaledLength;
            
            _lines[index].SetPointPosition(1, newEnd);
        }
    }

    private void ApplySineWaveToLines(float time)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_lines[i] == null || _baseLinePoints[i] == null) continue;

            float sineOffset = Mathf.Sin(Time.GetTicksMsec() * 0.001f * _sineFrequency + _phaseOffsets[i]) * _sineAmplitude;

            // Сдвигаем точки линий по оси X
            for (int p = 0; p < _baseLinePoints[i].Length; p++)
            {
                Vector2 basePt = _baseLinePoints[i][p];
                Vector2 shiftedPt = new Vector2(basePt.X + sineOffset, basePt.Y);
                _lines[i].SetPointPosition(p, shiftedPt);
            }
        }
    }
}