using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] GameObject markerPrefab;
    [SerializeField] float maxDisplayDistance;

    public GameObject getPrefab() { return markerPrefab; }
    public float getMaxDisplayDistance() { return maxDisplayDistance;}
}
