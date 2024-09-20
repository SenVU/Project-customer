using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public static class ScreenFade
{
    public static IEnumerator Fade(CanvasGroup screenOverlay, float fadeDuration, float startAlpha, float endAlpha)
    {
        if (screenOverlay == null)
            yield break;

        float timeElapsed = 0f;
        screenOverlay.alpha = startAlpha;
        screenOverlay.gameObject.SetActive(true);

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float progress = timeElapsed / fadeDuration;
            screenOverlay.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            yield return null;
        }

        screenOverlay.alpha = endAlpha;
    }
}
