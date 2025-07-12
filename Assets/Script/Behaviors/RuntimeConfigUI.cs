using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuntimeConfigUI : MonoBehaviour
{
    public Slider actorSpeedSlider;
    public Slider spawnRateInput;
    public Toggle aggressiveToggle;

    void Start()
    {
        actorSpeedSlider.onValueChanged.AddListener(OnActorSpeedChanged);
        spawnRateInput.onValueChanged.AddListener(OnSpawnRateChanged);
        aggressiveToggle.onValueChanged.AddListener(OnMarbleBehaviorChanged);

        actorSpeedSlider.value = GameConfig.Instance.ActorSpeed;
        spawnRateInput.value = GameConfig.Instance.SpawnRate;
        aggressiveToggle.isOn = GameConfig.Instance.AggressiveMarbles;
    }

    void OnActorSpeedChanged(float value)
    {
        GameConfig.Instance.ActorSpeed = value;
    }

    void OnSpawnRateChanged(float value)
    {
        GameConfig.Instance.SpawnRate =(int) value;
    }

    void OnMarbleBehaviorChanged(bool isAggressive)
    {
        GameConfig.Instance.AggressiveMarbles = isAggressive;
    }
}
