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

    public void updatePosition(Vector3 position, int slotNumber)
    {
        SaveSlots slot = loadSlot(slotNumber);
        slot.lastPosition.x_pos = position.x;
        slot.lastPosition.y_pos = position.y;
        slot.lastPosition.z_pos = position.z;

        saveSlot(slot, slotNumber);
    }

    public string loadName(int slotNumber)
    {
        SaveSlots slot = loadSlot(slotNumber);
        string name = slot.playerName;

        return name;
    }

    public Vector3 loadPosition(int slotNumber)
    {
        SaveSlots slot = loadSlot(slotNumber);
        Vector3 loadedPosition = new Vector3(slot.lastPosition.x_pos, slot.lastPosition.y_pos, slot.lastPosition.z_pos);
        return loadedPosition;
    }

    public void saveItem(Item item, int slotNumber)
    {
        SaveSlots slot = loadSlot(slotNumber);
        
        foreach(InventorySlots sloted in slot.player_inventory.slotList)
        {
            if(sloted.itemSaved == null)
            {
                sloted.setItem(item);
                saveSlot(slot, slotNumber);
                return;
            }
        }
    }

    public bool submitItem(Item[] items, int slotNumber)
    {
        int count = 0;
         SaveSlots slot = loadSlot(slotNumber);
         foreach(InventorySlots sloted in slot.player_inventory.slotList)
        {
            foreach(Item item in items)
            {
                 if(sloted.itemSaved != null)
                {
                    if(sloted.itemSaved.itemName == item.itemName)
                    {
                         if(sloted.itemSaved.itemCount >= item.itemCount)
                        {
                            sloted.itemSaved.itemCount -= item.itemCount;

                            if(sloted.itemSaved.itemCount == 0)
                            {
                                sloted.itemSaved = null;
                            }

                            count+= 1;
                        }

                        else
                        {
                            return false;
                        }
                    
                    }
                }
            }
           
        }

        if(count == items.Length)
        {
            return true;
        }

        else
        {
            return false;
        }


    }


    public void unlockKeyConcept(int number, int slotNumber)
    {
        SaveSlots slot = loadSlot(slotNumber);

        foreach(KeyConcepts concept in slot.player_glossary.conceptList)
        {
            if(concept.keyNumber == number)
            {
                concept.unlocked = true;
                saveSlot(slot, slotNumber);
                return;
            }
        }

       
    }
}
