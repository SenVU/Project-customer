using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTheCubQuestStep : QuestStep
{
    private GameObject player;
    private GameObject cub;

    private TMPro.TMP_Text questText;
    private TipsMessage tipsScript;

    void Start()
    {
        questText = GameObject.Find("QuestAdvancement").GetComponent<TMPro.TMP_Text>();
        tipsScript = GameObject.Find("TipsManager").GetComponent<TipsMessage>();

        player = GameObject.Find("Player");
        cub = GameObject.Find("PlayerCub");

        if (tipsScript != null)
        {
            tipsScript.SetTipsMessage("You can press \"e\" to pick up yours cub");
        }

        UpdateTextUI();
    }
    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<CubPickup>().IsOnBack())
        {
            FinishQuestStep();
        }
    }

    private void UpdateTextUI()
    {
        if (questText != null)
        {
            questText.text = "Take your cub on your bag to help him";
        }
    }
}
