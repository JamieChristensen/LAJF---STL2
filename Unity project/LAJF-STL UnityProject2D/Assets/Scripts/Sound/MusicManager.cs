using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using NSubstitute.Core;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioMixer musicAudioMixer, masterAudioMixer, sfxAudioMixer, mainMenuAudioMixer;

    public MusicTheme[] musicThemes;

    private AudioSource pausedAudioSource;

    public ChoiceCategory runTimeChoices;

    public AudioClip[] battle, peace, ending;

    public List<AudioSource> sources;

    public SettingsScrObj gameSettings;

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

        InitializeMusicThemes();
    }

    void InitializeMusicThemes()
    {
        /* Ads an AudioSource for each music theme in the array musicThemes*/
        foreach (MusicTheme mt in musicThemes)
        {
            mt.source = gameObject.AddComponent<AudioSource>();
            mt.source.clip = mt.clip;
            mt.source.volume = mt.volume;
            mt.source.pitch = mt.pitch;
            mt.source.loop = mt.loop;
            mt.source.outputAudioMixerGroup = mt.audioMixerGroup;

            sources.Add(mt.source);

            if (mt.name == "MainMenu" && SceneManager.GetActiveScene().buildIndex == 0)
            {
                mt.source.Play();
            }
            else if (mt.name == "Choosing" && SceneManager.GetActiveScene().buildIndex != 0)
            {
                mt.source.Play();
            }
        }
    }


    private void Start()
    {
        SetMasterVolume(gameSettings.gameMasterVolume);
        SetMusicVolume(gameSettings.gameMusicVolume);
        SetSFXVolume(gameSettings.gameSFXVolume);
        SetMainMenuVolume(1);
       
        bool musicIsPlaying = false; 
        foreach (MusicTheme mt in musicThemes)
        {
            if (mt.source.isPlaying)
            {
                musicIsPlaying = true;
            }
        }
        if (musicIsPlaying)
        {
            return;
        }
        PlayMusic("MainMenu", 1);    
    }

    
    void OnLevelWasLoaded()
    {
        SetMasterVolume(gameSettings.gameMasterVolume);
        SetMusicVolume(gameSettings.gameMusicVolume);
        SetSFXVolume(gameSettings.gameSFXVolume);
        bool musicIsPlaying = false;
        foreach (MusicTheme mt in musicThemes)
        {
            if (mt.source.isPlaying)
            {
                musicIsPlaying = true;
            }
        }
        if (musicIsPlaying)
        {
            return;
        }
        PlayMusic("MainMenu", 1);
    }
    

    public void PlayMusic(string name, float targetVolume) // play a music theme (by name) after a specified delay (in float seconds)
    {
        for (int i = 0; i < musicThemes.Length; i++) // checks all Audio Sources (music themes)
        {
            if (musicThemes[i].source.isPlaying) // If the music theme is already playing
            {
                if (name != musicThemes[i].name) // if the requested music theme is not this one
                {
                    //  Debug.Log("name: " + name + " - musicThemes[i].name: " + musicThemes[i].name);
                    StartCoroutine(FadeMixerGroup.StartFade(musicThemes[i].source.outputAudioMixerGroup.audioMixer, musicThemes[i].name + "Vol", 5.5f, 0)); // turn down the volume in a fade
                    StartCoroutine(StopAfterDelay(musicThemes[i].source)); // stop the music theme
                }
            }
            else
            {
                if (name == "Battle")
                {
                    musicThemes[2].source.clip = battle[runTimeChoices.runTimeLoopCount - 1];
                }
                else if (name == "Peace")
                {
                    musicThemes[3].source.clip = peace[runTimeChoices.runTimeLoopCount - 1];
                }
                StartCoroutine(FadeMixerGroup.StartFade(musicThemes[i].source.outputAudioMixerGroup.audioMixer, musicThemes[i].name + "Vol", 5, targetVolume)); // turn up the volume in a fade
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

    public void PauseCurrentPlaying()
    {
        for (int i = 0; i < musicThemes.Length; i++) // checks all Audio Sources (music themes)
        {
            if (musicThemes[i].source.isPlaying) // If the music theme is playing
            {
                musicThemes[i].source.Pause();
                pausedAudioSource = musicThemes[i].source;
            }
        }
    }

    public void UnpauseCurrentPlaying()
    {
        pausedAudioSource.Play();
    }

    public void adjustCurrentPlayingVolume(float targetVolume)
    {
        for (int i = 0; i < musicThemes.Length; i++) // checks all Audio Sources (music themes)
        {
            if (musicThemes[i].source.isPlaying) // If the music theme is playing
            {
                StartCoroutine(FadeMixerGroup.StartFade(musicThemes[i].source.outputAudioMixerGroup.audioMixer, musicThemes[i].name + "Vol", 2, targetVolume)); // adjusts the volume in a fade
            }
        }
    }

    public void StopCurrentPlaying()
    {
        for (int i = 0; i < musicThemes.Length; i++) // checks all Audio Sources (music themes)
        {
            if (musicThemes[i].source.isPlaying) // If the music theme is playing
            {
                StartCoroutine(FadeMixerGroup.StartFade(musicThemes[i].source.outputAudioMixerGroup.audioMixer, musicThemes[i].name + "Vol", 5.5f, 0)); // turn down the volume in a fade
                StartCoroutine(StopAfterDelay(musicThemes[i].source)); // stop the music theme
            }
        }
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


    public void SetMasterVolume(float volume)
    {
        masterAudioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
       musicAudioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }

    public void SetMainMenuVolume(float volume)
    {
        mainMenuAudioMixer.SetFloat("MainMenuVol", Mathf.Log10(volume) * 20);
    }
}




