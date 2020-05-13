using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterSceneTrigger : MonoBehaviour
{
    private EnemyEntranceEffects entranceEffects;
    private CameraShake cameraShaker;
    private bool firstEntrance;


    void Start()
    {
        if (entranceEffects == null)
        {
            entranceEffects = FindObjectOfType<EnemyEntranceEffects>();
            cameraShaker = FindObjectOfType<CameraShake>();
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Monster") && !firstEntrance)
        {
            Debug.Log("FIRST ENTRANCE");
            firstEntrance = true;

            entranceEffects.StartChromaticAberration(0.3f, 0.9f);
            cameraShaker.StartShake(cameraShaker.shakePropertyOnMinionEnter);


        }
    }

    public void Reset()
    {
        firstEntrance = false;
    }
}
