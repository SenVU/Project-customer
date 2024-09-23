using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTheCubQuestStep : QuestStep
{
    private TMPro.TMP_Text questText;
    private TipsMessage tipsScript;

    private void Start()
    {
        GameObject textObject = GameObject.Find("QuestAdvancement");
        questText = textObject.GetComponent<TMPro.TMP_Text>();

        GameObject tipsObject = GameObject.Find("TipsManager");
        tipsScript = tipsObject.GetComponent<TipsMessage>();

        UpdateTextUI();

        if (tipsScript != null)
        {
            tipsScript.SetTipsMessage("You can press \"k\" to pick up the cub");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

/// <summary>
/// UI
/// </summary>
    private void UpdateTextUI()
    {
        if (questText != null)
        {
            questText.text = "Take your cub on your bag to help him";
        }
    }
}
