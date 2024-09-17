using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideQuest
{
    public SideQuestSO info;
    public QuestState state;
    private int currentQuestStepIndex;

    public SideQuest(SideQuestSO questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    public void InstantiateCurrentSideQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform)
                .GetComponent<QuestStep>();
            questStep.InitializeSideQuestStep(info.id);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists()) {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        } else {
            Debug.LogWarning("Problem with Quest Prefab");
        }
        return questStepPrefab;
    }
}
