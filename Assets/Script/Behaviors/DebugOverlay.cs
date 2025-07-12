using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugOverlay : MonoBehaviour
{

    [SerializeField]
    private TMP_Text t_actorsCount, t_marblesCount, t_avgTime;
    private List<ActorBehavior> _actors;
    private List<GameObject> _marbles;

    [SerializeField] private TargetSpawner targetSpawner;

    private float _averageUpdateTime;
    private readonly Stopwatch _stopwatch = new();
    public void SetActors(List<ActorBehavior> actors)
    {
        _actors= actors;
    }
    void Update()
    {
        _marbles = targetSpawner.activeObjects;

        if (_actors.Count == 0) return;

        _stopwatch.Restart();

        foreach (var actor in _actors)
        {
            actor.MeasureUpdate();
        }

        _stopwatch.Stop();
        _averageUpdateTime = (float)_stopwatch.Elapsed.TotalMilliseconds / _actors.Count;
        UpdateUI();
    }

    void UpdateUI()
    {
        t_actorsCount.text=$"Actor Count: {_actors?.Count ?? 0}";
        t_marblesCount.text=($"Marble Count: {_marbles?.Count ?? 0}");
        t_avgTime.text=($"Avg Update Time/Actor: {_averageUpdateTime:F4} ms");
    }
}
