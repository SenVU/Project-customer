using UnityEngine;

public class Food : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public string interactionKey = "e";
    public GameObject interactionUI;
    // public GameObject drop;
    [SerializeField] private int foodGained = 1;

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
            CollectItem();
        }
    }

    void CollectItem()
    {
        Debug.Log("Object collected ! Food +" + foodGained);
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
        GameEventsManager.instance.foodEvents.FoodGained(foodGained);
        GameEventsManager.instance.miscEvents.FoodCollected();
        Destroy(gameObject);
    }

    // Drop an Item when a mob die
    // private void OnDestroy()
    // {
    //     Instantiate(drop, transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), drop.transform.rotation);
    //     Instantiate(drop, transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), drop.transform.rotation);
    // }
}
