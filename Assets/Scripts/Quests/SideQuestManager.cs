using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class SideQuestManager : MonoBehaviour
{
    private Dictionary<string, SideQuest> sideQuestMap = new Dictionary<string, SideQuest>();
    private SideQuest currentSideQuest;

    private void Start()
    {
    }

    public void LoadAllSideQuests()
    {
        SideQuestSO[] sideQuests = Resources.LoadAll<SideQuestSO>("SideQuests");

        foreach (var questSO in sideQuests)
        {
            if (sideQuestMap.ContainsKey(questSO.id))
            {
                Debug.LogWarning($"Duplicate ID detected for side quest: {questSO.id}");
            }
            else
            {
                SideQuest newQuest = new SideQuest(questSO);
                sideQuestMap.Add(questSO.id, newQuest);
                Debug.Log($"Added side quest with ID: {questSO.id}");
            }
        }

        Debug.Log($"{sideQuests.Length} side quests loaded.");
    }

    public void AddSideQuest(SideQuestSO questInfo)
    {
        if (!sideQuestMap.ContainsKey(questInfo.id))
        {
            SideQuest newQuest = new SideQuest(questInfo);
            sideQuestMap.Add(questInfo.id, newQuest);
        }
        else
        {
            Debug.LogWarning($"Side quest with ID {questInfo.id} already exists.");
        }
    }

    public List<SideQuest> GetActiveSideQuests()
    {
        return new List<SideQuest>(sideQuestMap.Values);
    }

    public void CompleteSideQuest(SideQuest quest)
    {
        if (quest.state == QuestState.FINISHED)
        {
            if (sideQuestMap.ContainsKey(quest.info.id))
            {
                sideQuestMap.Remove(quest.info.id);
                Debug.Log($"Side quest {quest.info.id} completed and removed from active quests.");
            }
        }
        else
        {
            Debug.LogWarning("Cannot complete side quest that isn't finished.");
        }
    }

    public void ProgressQuest(SideQuest quest)
    {
        if (quest.CurrentStepExists())
        {
            quest.MoveToNextStep();
        }
        else
        {
            quest.state = QuestState.FINISHED;
        }
    }

    public void ChangeSideQuestState(string id, QuestState newState)
    {
        if (sideQuestMap.TryGetValue(id, out SideQuest quest))
        {
            quest.state = newState;
            Debug.Log($"Side quest {id} state changed to {newState}");
        }
        else
        {
            Debug.LogError($"Side quest with ID {id} not found.");
        }
    }

    private Dictionary<string, SideQuest> CreateSideQuestMap(string folder)
    {
        SideQuestSO[] allSideQuests = Resources.LoadAll<SideQuestSO>(folder);
        Dictionary<string, SideQuest> idToSideQuestMap = new Dictionary<string, SideQuest>();

        Debug.Log($"Total side quests loaded: {allSideQuests.Length}");

        foreach (SideQuestSO questInfo in allSideQuests)
        {
            if (idToSideQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning($"Duplicate side quest ID detected: {questInfo.id}");
            }
            else
            {
                SideQuest quest = new SideQuest(questInfo);
                idToSideQuestMap.Add(questInfo.id, quest);
                Debug.Log($"Added side quest with ID: {questInfo.id}");
            }
        }

        Debug.Log($"Total side quests in dictionary: {idToSideQuestMap.Count}");
        return idToSideQuestMap;
    }

    public SideQuest GetSideQuestById(string id)
    {
        if (sideQuestMap.TryGetValue(id, out SideQuest quest))
        {
            return quest;
        }
        else
        {
            Debug.LogError("ID not found in the Side Quest Map: " + id);
            return null;
        }
    }
}
