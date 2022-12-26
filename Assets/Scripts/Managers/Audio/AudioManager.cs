using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;

    public static AudioManager instance;

    public int priority;

   void Awake()
   {

        if(instance != null  && instance.priority > this.priority)
        {
            Destroy(this.gameObject);
            return;
        }

        else
        {
            instance = this;
        }
        
        DontDestroyOnLoad(this.gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;

             switch (s.audioType)
            {
                case Sound.AudioTypes.sfx:
                    s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;

                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }

            if (s.playOnAwake == true)
            {
                s.source.Play();
            }
        }
   }

    //kalau mau mainin suara find AudioManager di scene terus pake fungsi ini
   public void Play (string name)
   {
       Sound s =  Array.Find(sounds, sound => sound.name == name);
       if (s == null)
       {
            return;
       }
       s.source.Play();
   }

   public void Stop (string name)
   {
       Sound s =  Array.Find(sounds, sound => sound.name == name);
       if (s == null)
       {
            return;
       }
       s.source.Stop();
   }

    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(AudioOptions.musicVolume) * 20);
        soundEffectsMixerGroup.audioMixer.SetFloat("Sound Effects Volume", Mathf.Log10(AudioOptions.soundEffectsVolume) * 20);
    }
}
