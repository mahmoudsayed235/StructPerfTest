using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public static GameConfig Instance { get; private set; }

    [Header("Actor Settings")]
    public float ActorSpeed = 1f;

    [Header("Spawner Settings")]
    public int SpawnRate = 25;

    [Header("Marble Behavior")]
    public bool AggressiveMarbles = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
