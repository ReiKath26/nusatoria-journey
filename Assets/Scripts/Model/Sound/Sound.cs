using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public enum AudioTypes {sfx, music}
    public AudioTypes audioType;

    
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}
