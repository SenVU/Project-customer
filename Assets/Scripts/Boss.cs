using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private bool isQuitting = false;
    private bool isPlayerInRange = false;
    public string interactionKey = "e";
    public GameObject interactionUI;
    public GameObject drop;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionUI != null)
            {
                interactionUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            KillMob();
        }
    }

    void KillMob()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            spawnedObjects.Add(Instantiate(drop, transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), drop.transform.rotation));
            spawnedObjects.Add(Instantiate(drop, transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), drop.transform.rotation));
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }
}
