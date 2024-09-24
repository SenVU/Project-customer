using System;

public class QuestEvents
{
    public event Action<string> onStartQuest;
    public void StartQuest(string id)
    {
        if (onStartQuest != null)
        {
            onStartQuest(id);
        }
    }

    public event Action<string> onProgressQuest;
    public void ProgressQuest(string id)
    {
        if (onProgressQuest != null)
        {
            onProgressQuest(id);
        }
    }

    public event Action<string> onFinishQuest;
    public void FinishQuest(string id)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id);
        }
    }

    public event Action<Quest> onQuestStateChange;
    public void QuestStateChange(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }

//////////////////// Side Quest ////////////////////

    public event Action<SideQuestSO> onStartSideQuest;
    public void StartSideQuest(SideQuestSO id)
    {
        if (onStartSideQuest != null)
        {
            onStartSideQuest(id);
        }
    }

    public event Action<string> onProgressSideQuest;
    public void ProgressSideQuest(string id)
    {
        if (onProgressSideQuest != null)
        {
            onProgressSideQuest(id);
        }
    }

    public event Action<string> onFinishSideQuest;
    public void FinishSideQuest(string id)
    {
        if (onFinishSideQuest != null)
        {
            onFinishSideQuest(id);
        }
    }

    public event Action<SideQuest> onSideQuestStateChange;
    public void SideQuestStateChange(SideQuest quest)
    {
        if (onQuestStateChange != null)
        {
            onSideQuestStateChange(quest);
        }
    }
}
