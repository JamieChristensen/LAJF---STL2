using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioMixer audioMixer;

    public MusicTheme[] musicThemes;


    private void Awake()
    {
        /* Singleton pattern*/
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        /* Ads an AudioSource for each music theme in the array musicThemes*/
        foreach (MusicTheme mt in musicThemes)
        {
            mt.source = gameObject.AddComponent<AudioSource>();
            mt.source.clip = mt.clip;

            mt.source.volume = mt.volume;
            mt.source.pitch = mt.pitch;
            mt.source.loop = mt.loop;
            mt.source.outputAudioMixerGroup = mt.audioMixerGroup;

            if (mt.name == "MainMenu")
            {
                mt.source.Play();
            }
        }

    }

    public void PlayMusic(string name) // play a music theme (by name) after a specified delay (in float seconds)
    {
        for (int i = 0; i< musicThemes.Length;i++) // checks all Audio Sources (music themes)
        {
            if (musicThemes[i].source.isPlaying) // If the music theme is already playing
            {
                if (name != musicThemes[i].name) // if the requested music theme is not this one
                {
                  //  Debug.Log("name: " + name + " - musicThemes[i].name: " + musicThemes[i].name);
                    StartCoroutine(FadeMixerGroup.StartFade(musicThemes[i].source.outputAudioMixerGroup.audioMixer, musicThemes[i].name+"Vol", 6, -80)); // turn down the volume in a fade
                    StartCoroutine(StopAfterDelay(musicThemes[i].source)); // stop the music theme
                }
            }
            else
            {
                StartCoroutine(FadeMixerGroup.StartFade(musicThemes[i].source.outputAudioMixerGroup.audioMixer, musicThemes[i].name + "Vol", 5, 1)); // turn up the volume in a fade
            }
            
        }

        MusicTheme mt = Array.Find(musicThemes, musicTheme => musicTheme.name == name);
        if (mt == null)
        {
            Debug.LogWarning("Music Theme: " + name + " not found!");
            return;
        }

            StartCoroutine(PlayAfterDelay(mt.source));
        



    }

    IEnumerator PlayAfterDelay(AudioSource musicThemeAudioSource)
    {
        yield return new WaitForSeconds(5);
        musicThemeAudioSource.Play();
    }

    IEnumerator StopAfterDelay(AudioSource musicThemeAudioSource)
    {
        yield return new WaitForSeconds(6);
        musicThemeAudioSource.Stop();
    }

   

}




