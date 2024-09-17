using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBossQuestStep : QuestStep
{
    public GameObject bossMob;
    // Start is called before the first frame update
    void Start()
    {
        bossMob = GameObject.Find("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        if (bossMob == null)
        {
            FinishQuestStep();
        }
    }
}
