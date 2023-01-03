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
    private AudioSettings setting;

    void Start()
    {
        setting = SaveHandler.instance.loadSettings();

        if (setting!= null)
        {
            if(musicSlider != null && sfxSlider != null)
            {
                musicSlider.value = setting.music_vol;
                sfxSlider.value = setting.sfx_vol;
            }
          
            musicVolume = setting.music_vol;
            soundEffectsVolume = setting.sfx_vol;

            AudioManager.instance.UpdateMixerVolume();
        }

    }

    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;

        setting.music_vol = musicVolume;
        SaveHandler.instance.saveSettings(setting);
        
        AudioManager.instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        soundEffectsVolume = value;
        
        setting.sfx_vol = soundEffectsVolume;
        SaveHandler.instance.saveSettings(setting);

        AudioManager.instance.UpdateMixerVolume();
    }
}
