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
         forceFieldBegin,
         forceFieldEnd,
         forceFieldHit,
         forceFieldWhileUp,
         hurtEnemy,
         hurtHero,
         invulnerable,
         jump,
         land,
         lightningStrike,
         lightningZap,
         loseFx,
         narratorHit,
         narratorVoiceLines,
         resurrection,
         select,
         selectionPicked,
         textToSpeechSource,
         winFx;

    public AudioSource[] narratorVoiceFillers, narratorEnter, godSources, enemyDeathAnnouncement;

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

    public void OnForcefieldToggle(bool active)
    {
        switch (active)
        {
            case true:
                forceFieldBegin.Play();
                forceFieldWhileUp.Play();
                break;

            case false:
                PlayWithVariablePitch(forceFieldEnd);
                if (forceFieldBegin.isPlaying)
                {
                    forceFieldBegin.Stop();
                }
                if (forceFieldWhileUp.isPlaying)
                {
                    forceFieldWhileUp.Stop();
                }
                break;
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
        selectionPicked.clip = runtimeChoices.chosenHero.picked;
        selectionPicked.Play();
        deathHero.clip = runtimeChoices.chosenHero.death;
        hurtHero.clip = runtimeChoices.chosenHero.hurt;
        jump.clip = runtimeChoices.chosenHero.jump;
    }

    public void OnEnemyPicked()
    {
        selectionPicked.clip = runtimeChoices.enemies[runtimeChoices.runTimeLoopCount - 1].representationClip;
        selectionPicked.Play();
        deathEnemy.clip = runtimeChoices.enemies[runtimeChoices.runTimeLoopCount - 1].deathClip;
        hurtEnemy.clip = runtimeChoices.enemies[runtimeChoices.runTimeLoopCount - 1].HurtClip;
    }

    public void OnModifierPicked()
    {
        selectionPicked.clip = runtimeChoices.enemyModifiers[runtimeChoices.runTimeLoopCount - 1].representationClip;
        selectionPicked.Play();

    }

    public void OnGodPicked(int PlayerNumber)
    {
        godSources[PlayerNumber-2].clip = runtimeChoices.chosenGods[PlayerNumber - 2].representationClip;
        godSources[PlayerNumber - 2].Play();
    }

    public void AnnounceEnemyDeath()
    {
        StartCoroutine(EnemyDeathSequence());
    }

    IEnumerator EnemyDeathSequence()
    {
        enemyDeathAnnouncement[0].Play();
        while (enemyDeathAnnouncement[0].isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (runtimeChoices.enemyModifiers[runtimeChoices.runTimeLoopCount-1].name == "Angry_")
        {
            enemyDeathAnnouncement[1].clip = runtimeChoices.enemyModifiers[runtimeChoices.runTimeLoopCount - 1].nameClip; // modifier first
            enemyDeathAnnouncement[2].clip = runtimeChoices.enemies[runtimeChoices.runTimeLoopCount - 1].nameClip; // enemy second
        }
        else
        {
            enemyDeathAnnouncement[1].clip = runtimeChoices.enemies[runtimeChoices.runTimeLoopCount - 1].nameClip; // enemy first
            enemyDeathAnnouncement[2].clip = runtimeChoices.enemyModifiers[runtimeChoices.runTimeLoopCount - 1].nameClip; // modifier second
        }
        enemyDeathAnnouncement[1].Play();
        while (enemyDeathAnnouncement[1].isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        enemyDeathAnnouncement[2].Play();
    }


    #endregion NarratorVoiceLines



}


