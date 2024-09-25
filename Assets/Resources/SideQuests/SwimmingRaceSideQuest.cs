using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingRaceSideQuest : QuestStep
{
    public bool isRacing = false;
    public GameObject opponent;
    private GameObject player;
    private TipsMessage tipsScript;
    private CourseAI courseAi;

    // Start is called before the first frame update
    void Start()
    {
        opponent = GameObject.Find("Running Race");

        GameObject tipsObject = GameObject.Find("TipsManager");
        tipsScript = tipsObject.GetComponent<TipsMessage>();

        courseAi = opponent.GetComponent<CourseAI>();

        player = GameObject.Find("Player");

        if (tipsScript != null)
        {
            tipsScript.SetTipsMessage("You can press \"ENTER\" to start the race");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRacing)
        {
            Race();
        }
        else
        {
            WaitForTheStart();
        }
    }
        

    private void WaitForTheStart()
    {
        float xPos = player.transform.position.x;
        float yPos = player.transform.position.y;
        float zPos = player.transform.position.z;

        bool isXInRange = xPos >= opponent.transform.position.x - 3.0f && xPos <= opponent.transform.position.x + 3.0f;
        bool isYInRange = yPos >= opponent.transform.position.y - 3.0f && yPos <= opponent.transform.position.y + 3.0f;
        bool isZInRange = zPos >= opponent.transform.position.z - 3.0f && zPos <= opponent.transform.position.z + 3.0f;

        if (isXInRange && isYInRange && isZInRange && Input.GetKeyDown(KeyCode.Return))
        {
            isRacing = true;
            courseAi.StartQuest();
        }
    }

    private void Race()
    {
        int result = courseAi.CheckIfSomeoneArrived();

        if (result == 0) {
            isRacing = false;
            FinishSideQuestStep();
        }
        else if (result == 1)
        {
            isRacing = false;
        }
    }
}
