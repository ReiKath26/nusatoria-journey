using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioOptions : MonoBehaviour
{
    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }


    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        
        AudioManager.instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        soundEffectsVolume = value;
        
        AudioManager.instance.UpdateMixerVolume();
    }
}
