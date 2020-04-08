using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioList : MonoBehaviour
{

    // SFX 
    public AudioSource 
         attack1, 
         attack2,
         choiceHasBeenMade,
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

    public void OnHeroWin()
    {
        winFx.Play();
    }

    public void OnHeroLose(int heroHP)
    {
        if (heroHP <= 0)
        {
            StartCoroutine(PlayOnDelay(loseFx, 1.5f));
        }
        
    }

    IEnumerator PlayOnDelay(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.Play();
    }

    public void OnChoiceMade()
    {
        choiceHasBeenMade.Play();
    }

}


