using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectFoodQuestStep : QuestStep
{
    private int foodsCollected = 0;
    private int foodsToComplete = 8;
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
            tipsScript.SetTipsMessage("You can press \"e\" to eat some things you found on the floor");
        }

        // move to the first side quest the player get
        // if (tipsScript != null)
        // {
        //     tipsScript.SetTipsMessage("You can press \"r\" to see all the side quest you can do");
        // }

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
            questText.text = "Find food to eat! In order to fill your hunger you must find " + foodsCollected + "/" + foodsToComplete + "pieces of food, be quick and mindful!";
        }
    }
}
