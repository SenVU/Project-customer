using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectFoodQuestStep : QuestStep
{
    private int foodsCollected = 0;
    private int foodsToComplete = 4;
    private TMPro.TMP_Text questText;
    private TipsMessage tipsScript;

    private void Start()
    {
        GameObject textObject = GameObject.Find("QuestAdvancement");
        questText = textObject.GetComponent<TMPro.TMP_Text>();

        GameObject tipsObject = GameObject.Find("TipsManager");
        tipsScript = tipsObject.GetComponent<TipsMessage>();

        if (tipsScript != null)
        {
            tipsScript.SetTipsMessage("You can press \"e\" to eat seal or anything else");
        }

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
            questText.text = "Find food to eat! Seal are really good! In order to fill your hunger you must find at least " + foodsCollected + "/" + foodsToComplete + " pieces of food, be quick and mindful!";
        }
    }
}
