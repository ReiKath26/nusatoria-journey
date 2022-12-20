using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueManager : MonoBehaviour
{
    [SerializeField] private Dialog [] dialogues;
    private bool AutoPlay;

    private SaveSlots slot;

    void Awake()
    {
        slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
    }

    
    
}
