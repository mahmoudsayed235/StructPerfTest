using System;
using System.Collections;
using UnityEngine;

public class TargetBehavior : MonoBehaviour, ITargetable
{
    public Guid Id { get; private set; }
    public float Value { get; private set; }
    public bool IsChosen { get; set; }
    public string ColorTag { get; private set; }
    public string TypeTag { get; private set; }
    public bool WasClaimed { get; private set; }


    private Transform _textboxContainer;
    private TextMesh _textMesh;
    [SerializeField] private TypeTagEnum typeTag;
    [SerializeField] private TypeColorEnum typeColor;
    enum TypeTagEnum
    {
        Marble,
        Gem
    }
    enum TypeColorEnum
    {
        Blue,
        Red
    }
    private TargetSpawner _spawner;

    
    void Awake()
    {
        _textMesh = transform.Find("TextboxContainer/Textbox/ScoreText").GetComponent<TextMesh>();
        _textboxContainer = transform.Find("TextboxContainer");
        _textboxContainer.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        GetComponent<MeshRenderer>().enabled = true;
        Id = Guid.NewGuid();
        Value = UnityEngine.Random.value * 100f - 25f;
        TypeTag = typeTag.ToString();
        ColorTag = typeColor.ToString();

        WasClaimed = false;
        _textMesh.text = Value.ToString("##.#");
        _textboxContainer.localScale = Vector3.zero;
        _textboxContainer.gameObject.SetActive(false);
        TargetContainer.Instance.RegisterTarget(this);
        StartWandering();
    }

    public void SetSpawner(TargetSpawner spawner)
    {
        _spawner = spawner;
    }
    public Vector3 Position => transform.position;

    public float ChangeDirectionInterval = 2f;

    private Coroutine wanderCoroutine;

    public void StartWandering()
    {
        if (wanderCoroutine != null)
            StopCoroutine(wanderCoroutine);

        wanderCoroutine = StartCoroutine(RandomWander());
    }

    private IEnumerator RandomWander()
    {
        while (!WasClaimed && GameConfig.Instance.AggressiveMarbles)
        {
            Vector3 newDirection = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                0f,
                UnityEngine.Random.Range(-1f, 1f)
            ).normalized;

            float moveTime = UnityEngine.Random.Range(1f, 3f); 

            float timer = 0f;
            while (timer < moveTime && !WasClaimed && GameConfig.Instance.AggressiveMarbles)
            {
                transform.position += newDirection * 1f * Time.deltaTime;
                timer += Time.deltaTime;
                yield return null;
            }

            yield return null;
        }
    }


    public void Claim()
    {
        if (WasClaimed) return;

        WasClaimed = true;
        TargetContainer.Instance.UnregisterTarget(this);
        StartCoroutine(DisplayScoreAndRespawn());
    }
    private IEnumerator DisplayScoreAndRespawn()
    {
        GetComponent<MeshRenderer>().enabled = false;
        const int steps = 60;
        _textboxContainer.localScale = Vector3.zero;
        _textboxContainer.gameObject.SetActive(true);

        for (int i = 0; i < steps; i++)
        {
            _textboxContainer.localScale += Vector3.one / steps;
        }

        yield return new WaitForSeconds(0.5f);
        _textboxContainer.gameObject.SetActive(false);

        _spawner.DeactivateObject(this); 
    }

    
}
