using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioList : MonoBehaviour
{
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
         textToSpeechSource,
         winFx;

    [Header("Narrator Voicelines")]
    public AudioClip[] chooseSceneVoiceLines;
    public AudioClip[] transitionVoiceLines;
    public void PlayWithVariablePitch(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.92f, 1.08f);
        audioSource.Play();
    }

    /*
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 4 || SceneManager.sceneCount == 3)
        {
            narratorVoiceLines.clip = chooseSceneVoiceLines[voiceLineIndex];
            narratorVoiceLines.Play();
        }

    }
    */

    public void OnHeroOpenedBox()
    {
        winFx.Play();
    }

    public void OnHeroLose(int heroHP)
    {
        if (heroHP <= 0)
        {
            StartCoroutine(PlayOnDelay(loseFx, 1.5f));
            narratorVoiceLines.clip = transitionVoiceLines[9];
        } 
    }

    public void OnHeroWin()
    {
        narratorVoiceLines.clip = transitionVoiceLines[9];
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



}


