using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideQuestAssignement : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private SideQuestManager sideQuestManager;
    [SerializeField] private SideQuestSO questInfoForPoint;

    private bool playerIsNear = false;
    private bool isQuestAssign = false;
    private string questId;
    private QuestState currentQuestState;

    private void Awake() 
    {
        questId = questInfoForPoint.id;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player") && !isQuestAssign)
        {
            isQuestAssign = true;
            playerIsNear = true;
            sideQuestManager.AddSideQuest(questInfoForPoint);
        }
    }
}