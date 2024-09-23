using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolarBearWakeUp : MonoBehaviour
{
    private CanvasGroup screenOverlay;

    private void Awake()
    {
        screenOverlay = GameObject.Find("DeathPanel").GetComponent<CanvasGroup>();

        StartCoroutine(WakeUpCoroutine());
    }

    private IEnumerator WakeUpCoroutine()
    {
        if (screenOverlay != null)
        {
            screenOverlay.gameObject.SetActive(true);
            yield return ScreenFade.Fade(screenOverlay, 2.0f, 1f, 0f);
            screenOverlay.gameObject.SetActive(false);
        }
    }
}
