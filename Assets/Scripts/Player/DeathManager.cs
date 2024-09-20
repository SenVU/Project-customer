using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathManager : MonoBehaviour
{
    [Header("Death Configuration")]
    [SerializeField] private float deathCountdownDuration = 10f;
    [SerializeField] private CanvasGroup screenOverlay;
    [SerializeField] private TypewriterEffect typeWritterEffect;
    public string foodDeathMessage;

    private Coroutine deathCountdownCoroutine;

    public void StartDeathCountdown()
    {
        if (deathCountdownCoroutine == null)
        {
            deathCountdownCoroutine = StartCoroutine(DeathCountdownRoutine());
        }
    }

    public void StopDeathCountdown()
    {
        if (deathCountdownCoroutine != null)
        {
            StopCoroutine(deathCountdownCoroutine);
            deathCountdownCoroutine = null;

            if (screenOverlay != null)
            {
                screenOverlay.alpha = 0f;
                screenOverlay.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator DeathCountdownRoutine()
    {
        float timeRemaining = deathCountdownDuration;
        float halfDuration = deathCountdownDuration / 2f;

        while (timeRemaining > halfDuration)
        {
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        if (screenOverlay != null)
        {
            screenOverlay.gameObject.SetActive(true);
            yield return ScreenFade.Fade(screenOverlay, halfDuration, 0f, 1f);
        }

        if (typeWritterEffect != null)
        {
            typeWritterEffect.StartText(foodDeathMessage);
        }
    }
}
