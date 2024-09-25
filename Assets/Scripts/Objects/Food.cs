using UnityEngine;

public class Food : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public string interactionKey = "e";
    public GameObject interactionUI;
    [SerializeField] private int foodGained = 1;
    [SerializeField] private float damage = 0;

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
        if (damage>0) GameObject.Find("Player").GetComponent<HealthManager>().Damage(damage, HealthManager.DamageSource.AteGarbage);
        Destroy(gameObject);
    }
}
