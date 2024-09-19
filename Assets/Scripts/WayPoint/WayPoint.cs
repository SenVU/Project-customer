using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] GameObject markerPrefab;


    void Update()
    {
        
    }

    public GameObject getPrefab() { return markerPrefab; }
}
