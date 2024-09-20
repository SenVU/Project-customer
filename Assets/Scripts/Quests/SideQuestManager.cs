using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideQuestManager : MonoBehaviour
{
    private List<SideQuest> activeSideQuests = new List<SideQuest>();

    private void Start()
    {
        LoadAllSideQuests();
    }

    private void LoadAllSideQuests()
    {
        SideQuestSO[] sideQuests = Resources.LoadAll<SideQuestSO>("SideQuests");

        foreach (var questSO in sideQuests)
        {
            SideQuest newQuest = new SideQuest(questSO);

            activeSideQuests.Add(newQuest);
        }

        Debug.Log($"{sideQuests.Length} side quests load");
    }

    public void AddSideQuest(SideQuestSO questInfo)
    {
        SideQuest newQuest = new SideQuest(questInfo);
        activeSideQuests.Add(newQuest);
    }

    public List<SideQuest> GetActiveSideQuests()
    {
        return activeSideQuests;
    }

    public void CompleteSideQuest(SideQuest quest)
    {
        if (quest.state == QuestState.FINISHED)
        {
            activeSideQuests.Remove(quest);
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

    private Dictionary<string, SideQuest> CreateSideQuestMap(string folder)
    {
        SideQuestSO[] allQuests = Resources.LoadAll<SideQuestSO>(folder);
        Dictionary<string, SideQuest> idToQuestMap = new Dictionary<string, SideQuest>();

        Debug.Log($"Total side quests loaded: {allQuests.Length}");

        foreach (SideQuestSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning($"Duplicate ID detected: {questInfo.id}");
            }
            else
            {
                SideQuest quest = new SideQuest(questInfo);
                idToQuestMap.Add(questInfo.id, quest);
                Debug.Log($"Added side quest with ID: {questInfo.id}");
            }
        }

        Debug.Log($"Total side quests in dictionary: {idToQuestMap.Count}");
        return idToQuestMap;
    }
}
