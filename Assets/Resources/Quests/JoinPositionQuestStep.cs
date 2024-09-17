using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoinPositionQuestStep : QuestStep
{
    public GameObject pointToJoin;
    private GameObject player;
    private TMPro.TMP_Text questText;

    public float tolerance = 1.0f;
    public float xPos;
    public float yPos;
    public float zPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject textObject = GameObject.Find("QuestAdvancement");
        questText = textObject.GetComponent<TMPro.TMP_Text>();

        player = GameObject.Find("Player");
        pointToJoin = GameObject.Find("Cylinder");
        UpdateTextUI();
    }

    // Update is called once per frame
    void Update()
    {
        xPos = player.transform.position.x;
        yPos = player.transform.position.y;
        zPos = player.transform.position.y;

        bool isXInRange = xPos >= pointToJoin.transform.position.x - tolerance && xPos <= pointToJoin.transform.position.x + tolerance;
        bool isYInRange = yPos >= pointToJoin.transform.position.y - tolerance && yPos <= pointToJoin.transform.position.y + tolerance;
        bool isZInRange = zPos >= pointToJoin.transform.position.z - tolerance && zPos <= pointToJoin.transform.position.z + tolerance;

        if (isXInRange && isYInRange)
        {
            FinishQuestStep();
        }
        UpdateTextUI();
    }

    private void UpdateTextUI()
    {
        if (questText != null)
        {
            questText.text = "Join the Cylinder";
        }
    }
}
