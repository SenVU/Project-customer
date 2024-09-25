using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class FoodManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int startingFood = 5;
    [SerializeField] private float foodLossInterval = 30f;
    [SerializeField] private float hungerDamage = 1f;

    public int currentFood { get; private set; }
    public int maxFood = 20;
    private TMP_Text myFood;

    private Coroutine foodLossCoroutine;

    private HealthManager healthManager;

    private void Awake()
    {
        myFood = GameObject.Find("FoodUI").GetComponent<TMP_Text>();
        healthManager = GetComponent<HealthManager>();
        currentFood = startingFood;
        UpdateFoodUI();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.foodEvents.onFoodGained += FoodGained;
        GameEventsManager.instance.foodEvents.onFoodLost += FoodLoosed;

        foodLossCoroutine = StartCoroutine(FoodLossRoutine());
    }

    private void OnDisable()
    {
        GameEventsManager.instance.foodEvents.onFoodGained -= FoodGained;
        GameEventsManager.instance.foodEvents.onFoodLost -= FoodLoosed;

        if (foodLossCoroutine != null)
        {
            StopCoroutine(foodLossCoroutine);
        }

        //deathManager.StopDeathCountdown();
    }

    private void Start()
    {
        GameEventsManager.instance.foodEvents.FoodChange(currentFood);
        UpdateFoodUI();
    }

    private void FoodGained(int food)
    {
        currentFood += food;
        GameEventsManager.instance.foodEvents.FoodChange(currentFood);
        UpdateFoodUI();
    }

    private void FoodLoosed(int food)
    {
        currentFood -= food;
        GameEventsManager.instance.foodEvents.FoodChange(currentFood);
        UpdateFoodUI();
    }

    private IEnumerator FoodLossRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(foodLossInterval);

            if (currentFood > 0)
            {
                FoodLoosed(1);
            }
            else
            {
                healthManager.Damage(hungerDamage, HealthManager.DamageSource.Hunger);
            }
        }
    }

    private void UpdateFoodUI()
    {
        if (myFood != null)
        {
            myFood.text = currentFood + "/" + maxFood;
        }
    }
}
