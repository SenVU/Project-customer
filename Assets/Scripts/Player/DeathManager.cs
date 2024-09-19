using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathManager : MonoBehaviour
{
    [Header("Death Configuration")]
    [SerializeField] private float deathCountdownDuration = 30f;
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
            screenOverlay.alpha = 0f;
            screenOverlay.gameObject.SetActive(false);
        }
    }

    private IEnumerator DeathCountdownRoutine()
    {
        float timeRemaining = deathCountdownDuration;
        float maxAlpha = 1f;
        float fadeStep = maxAlpha / deathCountdownDuration;

        if (screenOverlay != null)
        {
            screenOverlay.gameObject.SetActive(true);
            screenOverlay.alpha = 0f;
        }

        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;

            if (screenOverlay != null)
            {
                screenOverlay.alpha = Mathf.Clamp(screenOverlay.alpha + fadeStep, 0, 1f);
            }
        }

        if (typeWritterEffect != null)
        {
            typeWritterEffect.StartText(foodDeathMessage);
        }
    }

    private void PlayerDeath()
    {
        if (typeWritterEffect != null)
        {
            typeWritterEffect.StartText(foodDeathMessage);
        }

        //TODO: Stop everything in the game
    }
}
