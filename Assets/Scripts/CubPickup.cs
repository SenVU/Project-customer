using UnityEngine;

public class CubPickup : MonoBehaviour
{
    private GameObject cub;
    private AIPlayerCubTargeting cubAI;
    private GameObject pickupText;
    private Collider cubCollider;

    [SerializeField] KeyCode pickupKey;
    [SerializeField] float maxPickupDistance;
    [SerializeField] Vector3 cubPositionOffset;

    bool onBack;
    bool wasOnBack;
    void Start()
    {
        cub = GameObject.Find("PlayerCub");
        if (cub == null || cub.GetComponent<AIPlayerCubTargeting>() == null)
        {
            Debug.Log("no cub found, destroying pickup script");
            Destroy(this);
            return;
        }
        cubAI = cub.GetComponent<AIPlayerCubTargeting>();
        cubCollider = cub.GetComponent<Collider>();
        pickupText = GameObject.Find("CubPickupText");
    }


    void Update()
    {
        TestForKey();
        HandleCub();
    }

    void TestForKey()
    {
        if ((transform.position - cub.transform.position).magnitude <= maxPickupDistance || onBack)
        {
            pickupText.SetActive(!onBack);
            if (Input.GetKeyDown(pickupKey))
            {
                onBack = !onBack;
            }
        }
        else
        {
            pickupText?.SetActive(false);
        }
    }

    void HandleCub()
    {
        if (onBack)
        {
            if (!wasOnBack)
            {
                cub.transform.SetParent(transform);
                cubCollider.enabled = false;
                cubAI.DeactivateAI();
                wasOnBack = true;
            }

            cub.transform.localPosition = cubPositionOffset;
            cub.transform.localRotation = Quaternion.Euler(Vector3.zero);
            cub.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (!onBack && wasOnBack)
        {
            cub.transform.SetParent(null);
            cubCollider.enabled = true;
            cubAI.ActivateAI();
            wasOnBack = false;
            cub.transform.position=transform.position+Vector3.up+transform.forward*-2;
        }
    }
    public bool IsOnBack() {  return onBack; }
}
