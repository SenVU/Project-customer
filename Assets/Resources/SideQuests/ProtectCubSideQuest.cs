using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectCubSideQuest : QuestStep
{
    public GameObject wolfPrefab;
    public GameObject triggerZone;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public float spawnRadius = 20.0f;

    private bool wolvesSpawned = false;
    private List<GameObject> spawnedWolves = new List<GameObject>();

    void Start()
    {
        if (triggerZone == null)
        {
            triggerZone = GameObject.Find("WolfZone");
            wolfPrefab = GameObject.Find("Wolf");
        }
    }

    void Update()
    {
        if (!wolvesSpawned)
        {
            SpawnWolvesInRandomPositions();
        }
        else
        {
            CheckIfWolvesAreDead();
        }
    }

    private void SpawnWolvesInRandomPositions()
    {
        Vector3 randomPosition1 = GetRandomPositionAroundTriggerZone();
        Vector3 randomPosition2 = GetRandomPositionAroundTriggerZone();

        GameObject wolf1 = Instantiate(wolfPrefab, randomPosition1, Quaternion.identity);
        GameObject wolf2 = Instantiate(wolfPrefab, randomPosition2, Quaternion.identity);

        spawnedWolves.Add(wolf1);
        spawnedWolves.Add(wolf2);

        wolvesSpawned = true;
    }

    private Vector3 GetRandomPositionAroundTriggerZone()
    {
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);

        Vector3 randomPosition = new Vector3(
            triggerZone.transform.position.x + randomX,
            triggerZone.transform.position.y,
            triggerZone.transform.position.z + randomZ
        );

        return randomPosition;
    }

    private void CheckIfWolvesAreDead()
    {
        for (int i = spawnedWolves.Count - 1; i >= 0; i--)
        {
            if (spawnedWolves[i] == null)
            {
                spawnedWolves.RemoveAt(i);
            }
        }

        if (spawnedWolves.Count == 0)
        {
            wolvesSpawned = false;
            FinishSideQuestStep();
        }
    }
}
