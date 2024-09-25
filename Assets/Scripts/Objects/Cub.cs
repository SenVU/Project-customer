using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cub : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindTwoCubs findTwoCubs = FindObjectOfType<FindTwoCubs>();
            if (findTwoCubs != null)
            {
                findTwoCubs.OnCubFound();
                Destroy(gameObject);
            }
        }
    }
}
