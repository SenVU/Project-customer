using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawner : MonoBehaviour
{
    [SerializeField] GameObject wolfPefab;
    GameObject player;

    [SerializeField] int maxSpawnCount;
    [SerializeField] float minPlayerDistance;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position - transform.position).magnitude>minPlayerDistance && transform.childCount<maxSpawnCount)
        {
           while (transform.childCount<maxSpawnCount)
            {
                GameObject spawnedWolf = GameObject.Instantiate(wolfPefab);
                spawnedWolf.transform.position = transform.position;
                spawnedWolf.transform.SetParent(transform);
            }
        }
    }
}
