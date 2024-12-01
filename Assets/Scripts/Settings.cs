//using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    public Slider[] audioSliders = new Slider[3];

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        float masterVol = 1.0f;
        if (PlayerPrefs.HasKey("MasterVolumePref")) {
            masterVol = PlayerPrefs.GetFloat("MasterVolumePref");
        }
        SetMasterVolume(masterVol);
        audioSliders[0].value = masterVol;

        float sfxVol = 1.0f;
        if (PlayerPrefs.HasKey("SFXVolumePref")) {
            sfxVol = PlayerPrefs.GetFloat("SFXVolumePref");
        }
        SetSFXVolume(sfxVol);
        audioSliders[1].value = sfxVol;

        float musicVol = 1.0f;
        if (PlayerPrefs.HasKey("MusicVolumePref")) {
            musicVol = PlayerPrefs.GetFloat("MusicVolumePref");
        }
        SetMusicVolume(musicVol);
        audioSliders[2].value = musicVol;

       
    }
    
    public void SetMasterVolume (float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("MasterVolumePref", volume);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("SFXVolumePref", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("MusicVolumePref", volume);
    }
    /*
    public float GetMasterVolume()
    {
        float volume = 0f;
        audioMixer.GetFloat("MasterVolume", out volume);
        Debug.Log("Master Volume: " + volume.ToString());
        return volume;
    }
    public float GetSFXVolume()
    {
        float volume = 0f;
        audioMixer.GetFloat("SFXVolume", out volume);
        return volume;
    }
    public float GetMusicVolume()
    {
        float volume = 0f;
        audioMixer.GetFloat("MusicVolume", out volume);
        return volume;
    }
    */
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    /*
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("MasterVolumePref", GetMasterVolume());
        PlayerPrefs.SetFloat("SFXVolumePref", GetSFXVolume());
        PlayerPrefs.SetFloat("MusicVolumePref", GetMusicVolume());
    }
    */
}
