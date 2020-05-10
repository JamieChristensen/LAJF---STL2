using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer masterAudioMixer, musicAudioMixer, sfxAudioMixer;
    public Slider masterVolumeSlider, musicVolumeSlider, sfxVolumeSlider;
    //public TMP_Dropdown resolutionDropdown;
    //public TMP_Dropdown qualityDropdown;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public SettingsScrObj gameSettings;

    Resolution[] resolutions;

    void Start()
    {

        SetMasterVolume(gameSettings.gameMasterVolume);
        SetMusicVolume(gameSettings.gameMusicVolume);
        SetSFXVolume(gameSettings.gameSFXVolume);

        masterVolumeSlider.value = gameSettings.gameMasterVolume;
        musicVolumeSlider.value = gameSettings.gameMusicVolume;
        sfxVolumeSlider.value = gameSettings.gameSFXVolume;

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume)
    {
        masterAudioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
        gameSettings.gameMasterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        gameSettings.gameMusicVolume = volume;
    }

    public void SetSFXVolume (float volume)
    {
        sfxAudioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        gameSettings.gameSFXVolume = volume;
    }

    public void SetQuality (int qualityIndex)
    {
        qualityIndex = qualityDropdown.value;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


}
