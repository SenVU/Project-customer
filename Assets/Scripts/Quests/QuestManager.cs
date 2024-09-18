using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;
    private Dictionary<string, SideQuest> sideQuestMap;
    //============================================================//
    private Quest currentQuest;
    private SideQuest currentSideQuest;
    //============================================================//
    private int questIndex = 0;
    private int sideQuestIndex = 0;
    //============================================================//
    private QuestState currentQuestState;
    private QuestState currentSideQuestState;
    //============================================================//
    [SerializeField] private TMPro.TMP_Text questText;
    [SerializeField] private TMPro.TMP_Text sideQuestText;
    //============================================================//

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onProgressQuest -= ProgressQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.instance.questEvents.onStartSideQuest -= StartSideQuest;
        GameEventsManager.instance.questEvents.onProgressSideQuest -= ProgressSideQuest;
        GameEventsManager.instance.questEvents.onFinishSideQuest -= FinishSideQuest;
    }

    private void Awake()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onProgressQuest += ProgressQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;

        GameEventsManager.instance.questEvents.onStartSideQuest += StartSideQuest;
        GameEventsManager.instance.questEvents.onProgressSideQuest += ProgressSideQuest;
        GameEventsManager.instance.questEvents.onFinishSideQuest += FinishSideQuest;
    
        questMap = CreateQuestMap("Quests");
        sideQuestMap = CreateSideQuestMap("SideQuests");
    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }

        foreach (SideQuest quest in sideQuestMap.Values)
        {
            GameEventsManager.instance.questEvents.SideQuestStateChange(quest);
        }
    }

    private void Update()
    {
        List<Quest> questList = questMap.Values.ToList();
        List<SideQuest> sideQuestList = sideQuestMap.Values.ToList();

        if (questList.Count > 0)
        {
            Quest firstQuest = questList[questIndex];
            currentQuest = GetQuestById(firstQuest.info.id);
        }
        currentQuestState = currentQuest.state;

        if (sideQuestList.Count > 0)
        {
            SideQuest firstSideQuest = sideQuestList[sideQuestIndex];
            currentSideQuest = GetSideQuestById(firstSideQuest.info.id);
        }
        currentSideQuestState = currentSideQuest.state;

        if (currentQuestState.Equals(QuestState.REQUIREMENTS_NOT_MET))
        {
            GameEventsManager.instance.questEvents.StartQuest(currentQuest.info.id);
        }
        else if (currentQuestState.Equals(QuestState.FINISHED))
        {
            questIndex++;
        }
        UpdateQuestUI();

        if (currentSideQuestState.Equals(QuestState.REQUIREMENTS_NOT_MET))
        {
            GameEventsManager.instance.questEvents.StartSideQuest(currentSideQuest.info.id);
        }
        else if (currentSideQuestState.Equals(QuestState.FINISHED))
        {
            sideQuestIndex++;
        }
        UpdateSideQuestUI();
    }


////////// Quest Update //////////

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
        quest.InstantiateCurrentQuestStep(this.transform);
    }

    private void ProgressQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ChangeQuestState(quest.info.id, QuestState.AFTER_PROGRESS);
        quest.InstantiateCurrentQuestStep(this.transform);
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }
    
    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }

    private void StartSideQuest(string id)
    {
        SideQuest quest = GetSideQuestById(id);
        ChangeSideQuestState(quest.info.id, QuestState.IN_PROGRESS);
        quest.InstantiateCurrentSideQuestStep(this.transform);
    }

    private void ProgressSideQuest(string id)
    {
        SideQuest quest = GetSideQuestById(id);
        ChangeSideQuestState(quest.info.id, QuestState.AFTER_PROGRESS);
        quest.InstantiateCurrentSideQuestStep(this.transform);
    }

    private void FinishSideQuest(string id)
    {
        SideQuest quest = GetSideQuestById(id);
        ChangeSideQuestState(quest.info.id, QuestState.FINISHED);
    }
    
    private void ChangeSideQuestState(string id, QuestState state)
    {
        SideQuest quest = GetSideQuestById(id);
        quest.state = state;
        GameEventsManager.instance.questEvents.SideQuestStateChange(quest);
    }

////////// Quest Dictionnary //////////
    private Dictionary<string, Quest> CreateQuestMap(string folder)
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>(folder);
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        Debug.Log($"Total quests loaded: {allQuests.Length}");

        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning($"Duplicate ID detected: {questInfo.id}");
            }
            else
            {
                Quest quest = new Quest(questInfo);
                idToQuestMap.Add(questInfo.id, quest);
                Debug.Log($"Added quest with ID: {questInfo.id}");
            }
        }

        Debug.Log($"Total quests in dictionary: {idToQuestMap.Count}");
        return idToQuestMap;
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

////////// Quest Dictionnary //////////

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }

    public void SetQuest(string id)
    {
        if (questMap.TryGetValue(id, out Quest quest))
        {
            currentQuest = quest;
            UpdateQuestUI();
        }
        else
        {
            Debug.LogError("Quest ID not found: " + id);
        }
    }

    private void UpdateQuestUI()
    {
        if (questText != null)
        {
            questText.text = currentQuest != null ? "Main Quest : " + currentQuest.info.displayName : "No quest available";
        }
    }

////////// Side Quest Dictionnary //////////

    private SideQuest GetSideQuestById(string id)
    {
        SideQuest quest = sideQuestMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }

    public void SetSideQuest(string id)
    {
        if (sideQuestMap.TryGetValue(id, out SideQuest quest))
        {
            currentSideQuest = quest;
            UpdateSideQuestUI();
        }
        else
        {
            Debug.LogError("Side Quest ID not found: " + id);
        }
    }

    private void UpdateSideQuestUI()
    {
        if (sideQuestText != null)
        {
            sideQuestText.text = currentSideQuest != null ? "Side Quest : " + currentSideQuest.info.displayName : "No side quest available";
        }
    }
}
