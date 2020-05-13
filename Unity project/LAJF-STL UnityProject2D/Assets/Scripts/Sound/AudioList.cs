using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioList : MonoBehaviour
{
    public ChoiceCategory runtimeChoices;
    public int voiceLineIndex;

    // SFX 
    public AudioSource
         attack1,
         attack2,
         choiceHasBeenMade,
         deathEnemy,
         deathHero,
         explosion,
         hurt,
         jump,
         land,
         lightningStrike,
         lightningZap,
         loseFx,
         narratorRead,
         narratorHit,
         narratorVoiceLines,
         select,
         selectionPicked,
         textToSpeechSource,
         winFx;

    public AudioSource[] narratorVoiceFillers;

    public void PlayWithVariablePitch(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.92f, 1.08f);
        audioSource.Play();
    }

    public void OnHeroOpenedBox()
    {
        winFx.Play();
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

    public void OnNarratorHit()
    {
        narratorHit.Play();
        if (textToSpeechSource.isPlaying)
        {
            textToSpeechSource.Stop();
        }
    }


    #region NarratorVoiceLines

    public void OnHeroLose(int heroHP)
    {
        if (heroHP <= 0)
        {
            StartCoroutine(PlayOnDelay(loseFx, 1.5f));
        }
    }

    public void OnHeroPicked()
    {
        Debug.Log("GOAT");
        selectionPicked.clip = runtimeChoices.chosenHero.picked;
        selectionPicked.Play();
    }

    #endregion NarratorVoiceLines



}


