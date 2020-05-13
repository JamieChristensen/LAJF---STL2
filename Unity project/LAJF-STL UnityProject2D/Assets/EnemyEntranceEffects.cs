using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.LookDev;

public class EnemyEntranceEffects : MonoBehaviour
{
    public Volume volume;
    public VolumeProfile volumeProfile;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StartChromaticAberration(0.5f, 0.8f);
        }
    }

    public void StartChromaticAberration(float duration, float biasTowardEnd)
    {
        biasTowardEnd = Mathf.Clamp01(biasTowardEnd);
        StartCoroutine(ChromaticAberr(duration, biasTowardEnd));
    }

    public IEnumerator ChromaticAberr(float duration, float biasTowardEnd)
    {
        float timer = 0;
        biasTowardEnd = Mathf.Clamp01(biasTowardEnd);

        float rampUpDuration = duration * (1 - biasTowardEnd);
        float rampDownDuration = duration * biasTowardEnd;


        ChromaticAberration chromComp = null;

        foreach (VolumeComponent component in volumeProfile.components)
        {
            if (component.GetType() == typeof(ChromaticAberration))
            {
                Debug.Log("Is chromatic aberration");
                chromComp = (ChromaticAberration)component;
                chromComp.intensity.Override(1f);
            }
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (chromComp == null)
            {
                timer += duration;
                yield return null;
            }

            float lerpT = Mathf.InverseLerp(0, duration, timer);

            chromComp.intensity.Override(lerpT);

            yield return new WaitForSeconds(0f);

        }

        timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (chromComp == null)
            {
                timer += duration;
                yield return null;
            }

            float lerpT = Mathf.InverseLerp(duration, 0, timer);

            chromComp.intensity.Override(lerpT);

            yield return new WaitForSeconds(0f);

        }

        yield return null;
    }
}
