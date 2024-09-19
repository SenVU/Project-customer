using System.Collections;
using TMPro;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int startingFood = 5;
    [SerializeField] private float foodLossInterval = 30f;

    public int currentFood { get; private set; }
    [SerializeField] private TMP_Text myFood;

    private Coroutine foodLossCoroutine;

    [SerializeField] private DeathManager deathManager;

    private void Awake()
    {
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

        deathManager.StopDeathCountdown();
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

        deathManager.StopDeathCountdown();
    }

    private void FoodLoosed(int food)
    {
        currentFood -= food;
        GameEventsManager.instance.foodEvents.FoodChange(currentFood);
        UpdateFoodUI();

        if (currentFood <= 0)
        {
            deathManager.StartDeathCountdown();
        }
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
        }
    }

    private void UpdateFoodUI()
    {
        if (myFood != null)
        {
            string meatEmoji = "|";
            string foodDisplay = "Food : ";

            for (int i = 0; i < currentFood; i++)
            {
                foodDisplay += meatEmoji;
            }
            myFood.text = foodDisplay;
        }
    }
}
