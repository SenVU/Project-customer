using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private string sideQuestId;

    private TMP_Text textMeshPro;
    public float fadeDuration = 1.0f;
    public float displayDuration = 1.0f;

    private void Start()
    {
        GameObject textObject = GameObject.Find("MainQuestText");
        textMeshPro = textObject.GetComponent<TMPro.TMP_Text>();

        Color color = textMeshPro.color;
        color.a = 0;
        textMeshPro.color = color;
    } 

    public void InitializeQuestStep(string questId)
    {
        this.questId = questId;

        if (textMeshPro == null)
        {
            GameObject textObject = GameObject.Find("MainQuestText");
            if (textObject != null)
            {
                textMeshPro = textObject.GetComponent<TMP_Text>();
            }
        }

        QuestStartUI();

        StartCoroutine(FadeTextCoroutine());
    }

    public void InitializeSideQuestStep(string questId)
    {
        Debug.Log("I\'m here");
        this.sideQuestId = questId;
    }

    protected void VerificationQuestStep()
    {
        GameEventsManager.instance.questEvents.ProgressQuest(questId);
    }

    protected void VerificationSideQuestStep()
    {
        GameEventsManager.instance.questEvents.ProgressSideQuest(sideQuestId);
    }


    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            GameEventsManager.instance.questEvents.FinishQuest(questId);
            Destroy(this.gameObject);
        }
    }

    protected void FinishSideQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            GameEventsManager.instance.questEvents.FinishSideQuest(sideQuestId);
            Destroy(this.gameObject);
        }
    }

    private void QuestStartUI()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = "New Quest Start: " + questId;
        }
    }

    IEnumerator FadeTextCoroutine()
    {
        yield return StartCoroutine(Fade(0f, 1f));

        yield return new WaitForSeconds(displayDuration);

        yield return StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = textMeshPro.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            textMeshPro.color = color;
            yield return null;
        }

        color.a = endAlpha;
        textMeshPro.color = color;
    }
}