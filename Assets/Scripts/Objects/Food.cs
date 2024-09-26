using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public string interactionKey = "e";
    private GameObject interactionUI;
    [SerializeField] private int foodGained = 1;
    [SerializeField] private float damage = 0;
    [SerializeField] private float healthGained = 1;
    private bool isEating;

    void Start()
    {
        interactionUI = GameObject.Find("FoodText");
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
        if (isPlayerInRange && Input.GetKeyDown(interactionKey) && !isEating)
        {
            isEating = true;
            StartCoroutine(CollectItem());
        }
    }

    private IEnumerator CollectItem()
    {
        Debug.Log("Object collected ! Food +" + foodGained);
        
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
        
        GameEventsManager.instance.foodEvents.FoodGained(foodGained);
        GameEventsManager.instance.miscEvents.FoodCollected();
        
        GameObject player = GameObject.Find("Player");
        if (healthGained > 0) 
            player.GetComponent<HealthManager>().Heal(healthGained);
        
        if (damage > 0) 
            player.GetComponent<HealthManager>().Damage(damage, HealthManager.DamageSource.AteGarbage);
        
        player.GetComponent<PlayerControler>().StartEatAnimation();
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
