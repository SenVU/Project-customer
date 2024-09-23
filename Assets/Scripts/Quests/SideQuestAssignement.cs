using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideQuestAssignement : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private SideQuestManager sideQuestManager;
    [SerializeField] private SideQuestSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false;
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
        if (otherCollider.CompareTag("Player"))
        {
            Debug.Log("ENTER");
            playerIsNear = true;
            sideQuestManager.AddSideQuest(questInfoForPoint);
        }
    }
}