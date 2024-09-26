using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private bool gameIsFinished = false;
    private string questId;
    private string sideQuestId;

    private CanvasGroup screenOverlay;
    private GameObject endScreen;
    private TMP_Text textMeshPro;
    public float fadeDuration = 1.0f;
    public float displayDuration = 1.0f;

    private void Start()
    {
        screenOverlay = GameObject.Find("DeathPanel").GetComponent<CanvasGroup>();
        GameObject textObject = GameObject.Find("MainQuestText");
        textMeshPro = textObject.GetComponent<TMP_Text>();

        endScreen = GameObject.Find("Finish");
        endScreen.gameObject.SetActive(false);

        GameObject panel = GameObject.Find("DeathPanel");
        screenOverlay = panel.GetComponent<CanvasGroup>();

        Color color = textMeshPro.color;
        color.a = 0;
        textMeshPro.color = color;
    }

    public void InitializeQuestStep(string questId)
    {
        this.questId = questId;
        endScreen = GameObject.Find("Finish");
        if (endScreen != null)
        {
            endScreen.gameObject.SetActive(false);
        }

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
        this.sideQuestId = questId;
    }

    protected void VerificationQuestStep()
    {
        GameEventsManager.instance.questEvents.ProgressQuest(questId);
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

    protected void FinishGame()
    {
        if (!gameIsFinished)
        {
            gameIsFinished = true;

            if (endScreen != null)
            {
                endScreen.SetActive(true);
            }
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
    
    private IEnumerator FinishGameWithFade()
    {
        if (screenOverlay != null)
        {
            yield return ScreenFade.Fade(screenOverlay, 2f, 0f, 1f); 
        }
    
        endScreen.gameObject.SetActive(true);
        Destroy(this.gameObject);
        
    }
}