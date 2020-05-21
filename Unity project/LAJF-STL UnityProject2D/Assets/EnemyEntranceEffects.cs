using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.LookDev;
using UnityEngine.Experimental.Rendering.Universal;
using STL2;

public class EnemyEntranceEffects : MonoBehaviour
{
    public Volume volume;
    public VolumeProfile volumeProfile;

    private bool monsterJustSpawned;
    private float monsterLightTimer;

    [SerializeField]
    private float monsterLightMaxTime;
    public Transform lightTransform;

    private Transform monsterTransform;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StartChromaticAberration(0.5f, 0.8f);
        }

        if (monsterJustSpawned)
        {
            if (monsterTransform == null)
            {
                monsterTransform = FindObjectOfType<EnemyBehaviour>().transform;
            }
            monsterLightTimer -= Time.deltaTime;
            lightTransform.position = new Vector3(monsterTransform.position.x, lightTransform.position.y, lightTransform.position.z);
            lightTransform.gameObject.SetActive(true);

            if (monsterLightTimer < 0)
            {
                monsterJustSpawned = false;
                lightTransform.gameObject.SetActive(false);
            }
        }
    }
    public void MonsterSpawnedEventResponse()
    {
        monsterJustSpawned = true;
        monsterLightTimer = monsterLightMaxTime;
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
        Debug.Log("Bias: " + biasTowardEnd);
        float rampUpDuration = duration * (1 - biasTowardEnd);
        float rampDownDuration = duration * biasTowardEnd;


        ChromaticAberration chromComp = null;
        Debug.Log(volumeProfile.ToString());
        foreach (VolumeComponent component in volumeProfile.components)
        {
            if (component.GetType() == typeof(ChromaticAberration))
            {
                Debug.Log("Is chromatic aberration");
                chromComp = (ChromaticAberration)component;
                chromComp.intensity.Override(1f);
            }
        }

        while (timer < rampUpDuration)
        {
            timer += Time.deltaTime;

            if (chromComp == null)
            {
                timer += duration;
                yield return null;
            }

            float lerpT = Mathf.InverseLerp(0, rampUpDuration, timer);

            chromComp.intensity.Override(lerpT);

            yield return new WaitForSeconds(0f);

        }

        timer = 0;
        while (timer < rampDownDuration)
        {
            timer += Time.deltaTime;

            if (chromComp == null)
            {
                timer += duration;
                yield return null;
            }

            float lerpT = Mathf.InverseLerp(rampDownDuration, 0, timer);

            chromComp.intensity.Override(lerpT);

            yield return new WaitForSeconds(0f);

        }

        yield return null;
    }


}
