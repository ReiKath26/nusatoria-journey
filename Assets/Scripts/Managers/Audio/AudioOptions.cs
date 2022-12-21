using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioOptions: MonoBehaviour
{

    public Slider musicSlider;
    public Slider sfxSlider;

    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }

    void Start()
    {
        SaveHandler.instance.loadSettings();

        if (SaveHandler.instance.playerSettings != null)
        {
            musicSlider.value = SaveHandler.instance.playerSettings.music_vol;
            sfxSlider.value = SaveHandler.instance.playerSettings.sfx_vol;
            musicVolume = SaveHandler.instance.playerSettings.music_vol;
            soundEffectsVolume = SaveHandler.instance.playerSettings.sfx_vol;

            AudioManager.instance.UpdateMixerVolume();
        }

    }

    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;

        SaveHandler.instance.playerSettings.music_vol = musicVolume;
        SaveHandler.instance.saveSettings();

        Debug.Log(SaveHandler.instance.playerSettings.music_vol);
        Debug.Log(SaveHandler.instance.playerSettings.sfx_vol);
        
        AudioManager.instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        soundEffectsVolume = value;
        
        SaveHandler.instance.playerSettings.sfx_vol = soundEffectsVolume;
        SaveHandler.instance.saveSettings();

        Debug.Log(SaveHandler.instance.playerSettings.music_vol);
        Debug.Log(SaveHandler.instance.playerSettings.sfx_vol);

        AudioManager.instance.UpdateMixerVolume();
    }
}
