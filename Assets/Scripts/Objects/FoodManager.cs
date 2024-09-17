using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int startingFood = 5;
    [SerializeField] private float foodLossInterval = 10f;
    [SerializeField] private float deathCountdownDuration = 10f;

    public int currentFood { get; private set; }
    [SerializeField] private TMPro.TMP_Text myFood;

    private Coroutine foodLossCoroutine;
    private Coroutine deathCountdownCoroutine;

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
        if (deathCountdownCoroutine != null)
        {
            StopCoroutine(deathCountdownCoroutine);
        }
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

        if (deathCountdownCoroutine != null)
        {
            StopCoroutine(deathCountdownCoroutine);
            deathCountdownCoroutine = null;
        }
    }

    private void FoodLoosed(int food)
    {
        currentFood -= food;
        GameEventsManager.instance.foodEvents.FoodChange(currentFood);
        UpdateFoodUI();

        if (currentFood <= 0 && deathCountdownCoroutine == null)
        {
            deathCountdownCoroutine = StartCoroutine(DeathCountdownRoutine());
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

    private IEnumerator DeathCountdownRoutine()
    {
        float timeRemaining = deathCountdownDuration;
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;

            if (currentFood > 0)
            {
                yield break;
            }
        }

        Debug.Log("No food collected in time. Game over!");
        //TODO: Not done rn, I think we want a game over screen or something else
    }
}