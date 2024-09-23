using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeathManager : MonoBehaviour
{
    [Header("Messages")]
    [SerializeField] private PseudoDictionary<DeathReason, string> DeathMessages;

    [Header("Death Configuration")]
    [SerializeField] private float deathCountdownDuration = 10f;
    private CanvasGroup screenOverlay;
    private TypewriterEffect typeWritterEffect;

    private Coroutine deathCountdownCoroutine;

    public enum DeathReason
    {
        Starvation,
    }

    private void Awake()
    {
        typeWritterEffect=GameObject.Find("TypeWritterEffect").GetComponent<TypewriterEffect>();
        screenOverlay=GameObject.Find("DeathPanel").GetComponent<CanvasGroup>();
    }

        public void StartDeathCountdown(DeathReason reason)
    {
        if (deathCountdownCoroutine == null)
        {
            deathCountdownCoroutine = StartCoroutine(DeathCountdownRoutine(reason));
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

    private IEnumerator DeathCountdownRoutine(DeathReason reason)
    {
        string deathMessage = reason.ToString();
        if ( DeathMessages.ContainsKey(reason))
        {
            deathMessage = DeathMessages[reason];
        }
        
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
            typeWritterEffect.StartText(deathMessage);
        }
    }
}
