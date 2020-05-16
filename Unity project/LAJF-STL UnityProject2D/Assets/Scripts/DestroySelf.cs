using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DestroySelf : MonoBehaviour
{
    [SerializeField]
    Light2D light2d;

    public void DestroyMe()
    {
        //Do nothing for now
    }

    public void StartReduceIntensity(float time)
    {
        StartCoroutine(ReduceIntensity(time));
    }

    public IEnumerator ReduceIntensity(float time)
    {
        float intensityOfLight = light2d.intensity;
        float maxTime = time;
        while (time > 0)
        {
            time -= Time.deltaTime;
            float mappedIntensity = map(time, maxTime, 0, intensityOfLight, 0);

            light2d.intensity = mappedIntensity;
            Debug.Log("time: " + time);
            yield return new WaitForSeconds(0);
        }
        Destroy(gameObject);
        yield return null;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
