using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BackToYourDen : QuestStep
{
    public GameObject pointToJoin;
    private GameObject player;
    private TMPro.TMP_Text questText;
    private TipsMessage tipsScript;

    public float tolerance = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject textObject = GameObject.Find("QuestAdvancement");
        questText = textObject.GetComponent<TMPro.TMP_Text>();

        GameObject tipsObject = GameObject.Find("TipsManager");
        tipsScript = tipsObject.GetComponent<TipsMessage>();

        player = GameObject.Find("Player");
        pointToJoin = GameObject.Find("Old_tent_0002");

        if (tipsScript != null)
        {
            tipsScript.SetTipsMessage("You can press \"x\" to display the map waypoint");
        }
        UpdateTextUI();
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = player.transform.position.x;
        float yPos = player.transform.position.y;
        float zPos = player.transform.position.z;

        bool isXInRange = xPos >= pointToJoin.transform.position.x - tolerance && xPos <= pointToJoin.transform.position.x + tolerance;
        bool isYInRange = yPos >= pointToJoin.transform.position.y - tolerance && yPos <= pointToJoin.transform.position.y + tolerance;
        bool isZInRange = zPos >= pointToJoin.transform.position.z - tolerance && zPos <= pointToJoin.transform.position.z + tolerance;

        if (isXInRange && isYInRange && isZInRange)
        {
            FinishGame();
        }
        UpdateTextUI();
    }

    private void UpdateTextUI()
    {
        if (questText != null)
        {
            questText.text = "Now you have eat and you are with your cub, you can go back to your den.";
        }
    }
}
