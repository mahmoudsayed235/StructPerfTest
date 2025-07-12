using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Prefab Settings")]
    public TargetBehavior[] objectPrefabs;

    [Header("Spawn Settings")]
    public int initialCount = 500;
    public int maxCount = 10000;
    public int spawnPerFrame = 25;
    public float SpawnRadius = 10f;


    public List<GameObject> activeObjects = new List<GameObject>();
    private List<GameObject> inactiveObjects = new List<GameObject>();
    private int totalCreated = 0;

    void Start()
    {
        for (int i = 0; i < initialCount; i++)
        {
            CreateNewObject(true);
        }

        StartCoroutine(SpawnOverTime());
    }

    IEnumerator SpawnOverTime()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (totalCreated < maxCount)
            {
                for (int i = 0; i < GameConfig.Instance.SpawnRate && totalCreated < maxCount; i++)
                {
                    CreateNewObject(true);
                }
               
            }
            else
            {
                ShowInactive();
            }
        }
    }

    void CreateNewObject(bool makeActive)
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * SpawnRadius;
        GameObject obj = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Length)].gameObject, randomPos, Quaternion.identity);
        obj.SetActive(makeActive);

        if (makeActive)
        {
            activeObjects.Add(obj);
            obj.GetComponent<TargetBehavior>().Initialize();
            obj.GetComponent<TargetBehavior>().SetSpawner(this);
        }
        else
        {
            inactiveObjects.Add(obj);
        }
        totalCreated++;
    }

    public void ShowInactive()
    {
        int count = 0;
        for (int i = 0; i < inactiveObjects.Count && count < GameConfig.Instance.SpawnRate; i++)
        {
            GameObject obj = inactiveObjects[i];
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.GetComponent<TargetBehavior>().Initialize();
                activeObjects.Add(obj);
                inactiveObjects.RemoveAt(i);
                i--;
                count++;
            }
        }

    }

    public void DeactivateObject(TargetBehavior obj)
    {
        if (activeObjects.Contains(obj.gameObject))
        {
            obj.gameObject.SetActive(false);
            activeObjects.Remove(obj.gameObject);
            inactiveObjects.Add(obj.gameObject);
        }
    }
}
