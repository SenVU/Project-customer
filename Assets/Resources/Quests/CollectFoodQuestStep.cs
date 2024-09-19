using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectFoodQuestStep : QuestStep
{
    private int foodsCollected = 0;
    private int foodsToComplete = 8;
    private TMPro.TMP_Text questText;

    private void Start()
    {
        GameObject textObject = GameObject.Find("QuestAdvancement");
        questText = textObject.GetComponent<TMPro.TMP_Text>();

        GameEventsManager.instance.miscEvents.onFoodColleted += FoodCollected;
        UpdateTextUI();
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onFoodColleted -= FoodCollected;
    }

    private void FoodCollected()
    {
        if (foodsCollected < foodsToComplete)
        {
            foodsCollected++;
            UpdateTextUI();
        }
        if (foodsCollected >= foodsToComplete)
        {
            FinishQuestStep();
        }
    }

    private void UpdateTextUI()
    {
        if (questText != null)
        {
            questText.text = "Food needed: " + foodsCollected + "/" + foodsToComplete;
        }
    }
}
