using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private bool isQuitting = false;
    private bool isPlayerInRange = false;

    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetMouseButtonDown(0))
        {
            HurtWolf();
        }
    }

    void HurtWolf()
    {
        GetComponent<HealthManager>().Damage(1, HealthManager.DamageSource.Unknown);
    }
}
