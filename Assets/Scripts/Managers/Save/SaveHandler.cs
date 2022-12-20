using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;

public class SaveHandler : MonoBehaviour
{

    public static SaveHandler instance;

    public AudioSettings playerSettings;

    void Awake()
    {
        instance = this;
        SaveManager.Init();
    }

    public void loadSettings()
    {
        string contents = SaveManager.LoadSettings();

        if (contents != null)
        {
             playerSettings = JsonUtility.FromJson<AudioSettings>(contents);
        }

        else
        {
            return;
        }
       
    }

    public void saveSettings()
    {
        string jsonString = JsonUtility.ToJson(playerSettings);

        SaveManager.SaveSettings(jsonString);
        
    }

    public void saveSlot(SaveSlots slotToBeSaved, int SlotNumber)
    {
        string jsonString = JsonUtility.ToJson(slotToBeSaved);

        SaveManager.savePlayerSlot(jsonString, SlotNumber);
    }

    public SaveSlots loadSlot(int slotNumber)
    {
        string contents = SaveManager.LoadSlots(slotNumber);

        if (contents != null)
        {
            SaveSlots slot = JsonUtility.FromJson<SaveSlots>(contents);
            return slot;
        }

        else
        {
            return null;
        }
    }
}
