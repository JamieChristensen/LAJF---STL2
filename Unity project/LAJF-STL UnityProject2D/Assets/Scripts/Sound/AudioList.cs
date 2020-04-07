using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioList : MonoBehaviour
{
    // SFX 
    public AudioSource choiceHasBeenMade,
         deathEnemy,
         deathHero,
         hurt,
         jump,
         land,
         loseFx,
         narratorHit,
         select,
         winFx;


public void PlayWithVariablePitch(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.80f, 1.11f);
        audioSource.Play();
    }




}


