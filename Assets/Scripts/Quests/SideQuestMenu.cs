using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class SideQuestMenu : MonoBehaviour
{
    private GameObject questMenuPanel;
    private TextMeshProUGUI sideQuestsText;
    public SideQuestManager sideQuestManager;

    private bool isMenuVisible = false;

    private void Start()
    {
        questMenuPanel = GameObject.Find("SideQuestMenu");
        sideQuestsText = GameObject.Find("SideQuestList").GetComponent<TextMeshProUGUI>();
        questMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleQuestMenu();
        }
    }

    public void ToggleQuestMenu()
    {
        isMenuVisible = !isMenuVisible;
        questMenuPanel.SetActive(isMenuVisible);

        if (isMenuVisible)
        {
            DisplaySideQuests();
        }
    }

    private void DisplaySideQuests()
    {
        StringBuilder sb = new StringBuilder();
        List<SideQuest> activeSideQuests = sideQuestManager.GetActiveSideQuests();

        foreach (var quest in activeSideQuests)
        {
            sb.AppendLine($"<b>{quest.info.displayName}</b> - {quest.state}");
            sb.AppendLine($"{quest.info.Description}\n");
        }

        sideQuestsText.text = sb.ToString();
    }
}
