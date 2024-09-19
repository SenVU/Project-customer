using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectBabyQuestStep : QuestStep
{
    public GameObject enemy;
    public GameObject player;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Wolf");
        player = GameObject.Find("PlayerCub");
        spawnedObjects.Add(Instantiate(enemy, player.transform.position + new Vector3(Random.Range(-10f, 10f), 1, Random.Range(-10f, 10f)), enemy.transform.rotation));
        spawnedObjects.Add(Instantiate(enemy, player.transform.position + new Vector3(Random.Range(-10f, 10f), 1, Random.Range(-10f, 10f)), enemy.transform.rotation));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
            }
        }

        if (spawnedObjects.Count == 0)
        {
            FinishQuestStep();
        }
    }

    private void OnApplicationQuit()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }
}
