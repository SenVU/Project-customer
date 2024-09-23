using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTheCubQuestStep : QuestStep
{
    public string interactionKey = "g";

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
            tipsScript.SetTipsMessage("You can press \"g\" to pick up yours cub");
        }

        UpdateTextUI();
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Test");
        float xPos = player.transform.position.x;
        float yPos = player.transform.position.y;
        float zPos = player.transform.position.z;

        bool isXInRange = xPos >= cub.transform.position.x - 5.0f && xPos <= cub.transform.position.x + 5.0f;
        bool isYInRange = yPos >= cub.transform.position.y - 5.0f && yPos <= cub.transform.position.y + 5.0f;
        bool isZInRange = zPos >= cub.transform.position.z - 5.0f && zPos <= cub.transform.position.z + 5.0f;

        if (isXInRange && isYInRange && isZInRange && Input.GetKeyDown(interactionKey))
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
