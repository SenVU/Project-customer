using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindTwoCubs : QuestStep
{
    private int cubSave = 0;
    private int cubToComplete = 2;


    private void Start()
    {

    }

    public void OnCubFound()
    {
        cubSave++;

        if (cubSave >= cubToComplete)
        {
            QuestCompleted();
        }
    }

    private void QuestCompleted()
    {
        FinishSideQuestStep();
    }
}
