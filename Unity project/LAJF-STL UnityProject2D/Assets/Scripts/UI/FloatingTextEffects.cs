using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextEffects : MonoBehaviour
{
    public Text red, orange;
    public void setText(string text)
    {
        StartCoroutine(TextEffects(text));
    }

    public IEnumerator TextEffects(string text) // text effects - which text object does it concern and what should it contain? 
    {
        orange.text = text; // the back text (orange) is set
        red.text = text; // the front text (red) is set
        Vector3 currentSize = Vector3.zero; // start the text size at 0
        float randomSizeScale = UnityEngine.Random.Range(0.5f, 1.1f); // random sizes!
        Vector3 targetSize = new Vector3(randomSizeScale, randomSizeScale, 1); // set the target size to the random one from above
        RectTransform textRectTransform = GetComponent<RectTransform>();
        float timer = 0;
        float timeToLerp = 0.3f;
        //scaling up
        while (timer < timeToLerp)
        {
            timer += Time.deltaTime;
            currentSize = Vector3.Lerp(currentSize, targetSize, 2 * Time.deltaTime / timeToLerp);
            textRectTransform.localScale = currentSize;
            yield return null;
        }
        yield return new WaitForSeconds(0.4f); // let the text stay for a little while
        currentSize = targetSize;
        targetSize = Vector3.zero;
        timer = 0;
        //scaling down
        while (timer < timeToLerp)
        {
            timer += Time.deltaTime;
            currentSize = Vector3.Lerp(currentSize, targetSize, 2 * Time.deltaTime / timeToLerp);
            textRectTransform.localScale = currentSize;
            yield return null;
        }

        Destroy(gameObject); // destroy the text object
    }

}
