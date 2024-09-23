using System.Collections;
using UnityEngine;

public class TipsMessage : MonoBehaviour
{
    private TMPro.TMP_Text tipsText;
    private string tipsToDisplay;
    private float speed = 10.0f;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    void Start()
    {
        GameObject tipsObject = GameObject.Find("TipsMessage");
        tipsText = tipsObject.GetComponent<TMPro.TMP_Text>();

        originalPosition = tipsObject.transform.position;
        targetPosition = originalPosition + new Vector3(850, 0, 0);
    }

    public void SetTipsMessage(string tips)
    {
        tipsToDisplay = tips;
        DisplayTips();
    }

    private void DisplayTips()
    {
        if (tipsText != null)
        {
            tipsText.text = tipsToDisplay;
            StartCoroutine(MoveTextCoroutine());
        }
    }

    private IEnumerator MoveTextCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            tipsText.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        tipsText.transform.position = targetPosition;

        yield return new WaitForSeconds(3f);

        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            tipsText.transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        tipsText.transform.position = originalPosition;
    }
}
