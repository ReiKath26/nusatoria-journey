using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    public AudioSettings setting = new AudioSettings() {sfx_vol = 80, music_vol = 80};
    public List<SaveSlots> list = new List<SaveSlots>();

}
