using Godot;
using System;
using TwelveJam.scripts.units;

public partial class MobSpawner : Node2D
{
    [Export] private Timer timer;
    [Export] private WaveResource[] waveResources;

    private int currentDay;
    private int spawnIndex;
    private WaveResource activeWave;

    private Node2D parent;

    public override void _Ready()
    {
        parent = GetParent<Node2D>();
        GameManager.Instance.DayChanged += InstanceOnDayChanged;
        timer.Timeout += TimerOnTimeout;
        
        InstanceOnDayChanged(GameManager.Instance.CurrentDay);
    }

    private void InstanceOnDayChanged(int t_day)
    {
        currentDay = t_day - 1;

        if (currentDay < 0 || currentDay >= waveResources.Length) {
            timer.Stop();
            return;
        }

        activeWave = waveResources[currentDay];
        spawnIndex = 0;

        if (activeWave.Resource != null && activeWave.Resource.Length > 0)
        {
            SpawnCurrentBatch();
            ScheduleNextSpawn();
        }
    }

    private void TimerOnTimeout()
    {
        SpawnCurrentBatch();
        ScheduleNextSpawn();
    }

    private void SpawnCurrentBatch()
    {
        if (activeWave == null || spawnIndex >= activeWave.Resource.Length) return;

        var unitRes = activeWave.Resource[spawnIndex];
        if (unitRes?.Unit == null) return;

        for (int i = 0; i < unitRes.Count; i++)
        {
            var unitInstance = (Unit)unitRes.Unit.Instantiate();
            
            Vector2 randomOffset = GetRandomPointInRadius(2000f);
            unitInstance.GlobalPosition = GlobalPosition + randomOffset;
            
            parent.CallDeferred("add_child",unitInstance);
        }

        spawnIndex++;
    }

    private void ScheduleNextSpawn()
    {
        int delayIndex = spawnIndex - 1;

        if (spawnIndex < activeWave.Resource.Length && 
            activeWave.BetweenSpawnTime != null && 
            delayIndex >= 0 && delayIndex < activeWave.BetweenSpawnTime.Length) {
            timer.WaitTime = activeWave.BetweenSpawnTime[delayIndex];
            timer.Start();
        }else 
            timer.Stop();
    }

    private Vector2 GetRandomPointInRadius(float radius)
    {
        float angle = (float)GD.RandRange(0, MathF.Tau); // Случайный угол от 0 до 2*PI
        float distance = (float)GD.RandRange(0, radius); // Случайная дистанция от 0 до 2000
        
        return new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * distance;
    }
}