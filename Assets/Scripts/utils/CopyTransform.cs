using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField] Transform from;
    [SerializeField] bool copyPosition;
    [SerializeField] bool localPosition;
    [SerializeField] bool copyRotation;
    [SerializeField] bool localRotation;
    [SerializeField] bool copyScale;

    void Update()
    {
        if (copyPosition)
        {
            if (localPosition) transform.localPosition = from.localPosition;
            else transform.position = from.position;
        }
        if (copyRotation)
        {
            if(localRotation) transform.localRotation = from.localRotation;
            else transform.rotation = from.rotation;
        }
        if (copyScale) transform.localScale = from.localScale;
    }
}
