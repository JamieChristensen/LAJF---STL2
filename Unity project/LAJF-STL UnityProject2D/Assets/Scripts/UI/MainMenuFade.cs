using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuFade : MonoBehaviour
{
    public void StartFade()
    {
        StartCoroutine(FadingToBeginning());
    }

    IEnumerator FadingToBeginning()
    {
        yield return new WaitForSeconds(1.5f);
        float timer = 0;
        float timeToLerp = 3;
        float alpha = 0;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (timer < timeToLerp)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Lerp(alpha, 1, Time.deltaTime / 2*timeToLerp);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }
}
