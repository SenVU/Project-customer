using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigQuestStep : QuestStep
{
    private int dig = 0;
    private int digToComplete = 1;
    private TMPro.TMP_Text questText;

    private void Start()
    {
        GameObject textObject = GameObject.Find("QuestAdvancement");
        questText = textObject.GetComponent<TMPro.TMP_Text>();

        GameEventsManager.instance.miscEvents.onFoodColleted += Dig;
        UpdateTextUI();
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onFoodColleted -= Dig;
    }

    private void Dig()
    {
        if (dig < digToComplete)
        {
            dig++;
        }
        if (dig >= digToComplete)
        {
            FinishQuestStep();
        }
    }

    private void UpdateTextUI()
    {
        if (questText != null)
        {
            questText.text = "Collect one more Food: " + dig + "/" + digToComplete;
        }
    }
}
