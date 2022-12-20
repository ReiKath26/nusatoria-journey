using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioOptions: MonoBehaviour
{

    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }

    void Awake()
    {
        SaveHandler.instance.loadSettings();

        if (SaveHandler.instance.playerSettings != null)
        {
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
        
        AudioManager.instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        soundEffectsVolume = value;
        
        SaveHandler.instance.playerSettings.sfx_vol = soundEffectsVolume;
        SaveHandler.instance.saveSettings();

        AudioManager.instance.UpdateMixerVolume();
    }
}
