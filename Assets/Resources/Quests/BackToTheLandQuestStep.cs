using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToTheLandQuestStep : QuestStep
{
    public GameObject pointToJoin;
    private GameObject player;
    private TipsMessage tipsScript;
    private TMPro.TMP_Text questText;

    public float tolerance = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject textObject = GameObject.Find("QuestAdvancement");
        questText = textObject.GetComponent<TMPro.TMP_Text>();

        tipsScript = GameObject.Find("TipsManager").GetComponent<TipsMessage>();

        player = GameObject.Find("Player");
        pointToJoin = GameObject.Find("Land");

        if (tipsScript != null)
        {
            tipsScript.SetTipsMessage("You can press \"tab\" to see yourself and you cub");
        }
        UpdateTextUI();
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = player.transform.position.x;
        float zPos = player.transform.position.z;

        bool isXInRange = xPos >= pointToJoin.transform.position.x - tolerance && xPos <= pointToJoin.transform.position.x + tolerance;
        bool isZInRange = zPos >= pointToJoin.transform.position.z - tolerance && zPos <= pointToJoin.transform.position.z + tolerance;

        if (isXInRange && isZInRange)
        {
            FinishQuestStep();
        }
        UpdateTextUI();
    }

    private void UpdateTextUI()
    {
        if (questText != null)
        {
            questText.text = "Swim to join the land with your cub to save him !";
        }
    }
}
