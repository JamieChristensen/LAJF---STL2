using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class CameraShaker : MonoBehaviour
{

    public float duration;
    public float magnitude;
    public void StartShake()
    {
        StartCoroutine(CameraShake(duration, magnitude));
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1f) * magnitude;
            float y = Random.Range(-1, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPos;
        Debug.Log("Here in shake");

    }

}
