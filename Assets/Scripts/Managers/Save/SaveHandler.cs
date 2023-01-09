using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;

public class SaveHandler : MonoBehaviour
{

    public static SaveHandler instance;

    void Awake()
    {
        instance = this;
        SaveManager.Init();
        AudioSettings setting = loadSettings();

        if(setting == null)
        {
            setting = new AudioSettings(){music_vol = 0.8f, sfx_vol = 0.8f};
            saveSettings(setting);
        }
      
    }

    public AudioSettings loadSettings()
    {
        string contents = SaveManager.LoadSettings();

        if (contents != null)
        {
             AudioSettings playerSettings = JsonUtility.FromJson<AudioSettings>(contents);
             return playerSettings;
        }

        else
        {
            return null;
        }
       
    }

    public void saveSettings(AudioSettings setting)
    {
        string jsonString = JsonUtility.ToJson(setting);
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

    public void saveKeyconcepts(KeyConcepts concepts, int slotNumber, int keyNumber)
    {
        string jsonString = JsonUtility.ToJson(concepts);
        SaveManager.saveGlossary(jsonString, slotNumber, keyNumber);
    }

    public KeyConcepts loadKeyconcepts(int slotNumber, int keyNumber)
    {
        string contents = SaveManager.loadGlossary(slotNumber, keyNumber);

        if(contents != null)
        {
            KeyConcepts concept = JsonUtility.FromJson<KeyConcepts>(contents);
            return concept;
        }

        else
        {
            return null;
        }
     
    }

    public void saveInventory(InventorySlots invent, int slotNumber, int keyNumber)
    {
        string jsonString = JsonUtility.ToJson(invent);
        SaveManager.saveInventory(jsonString, slotNumber, keyNumber);
    }

    public InventorySlots loadInventory(int slotNumber, int keyNumber)
    {
        string contents = SaveManager.loadInventory(slotNumber, keyNumber);

          if(contents != null)
        {
            InventorySlots slotList = JsonUtility.FromJson<InventorySlots>(contents);
            return slotList;
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
        for(int i=0; i<8; i++)
        {
            InventorySlots slotDummy = loadInventory(slotNumber, i);
            if(slotDummy.itemSaved.itemName == "")
            {
                slotDummy.setItem(item);
                saveInventory(slotDummy, slotNumber, i);
                break;
            }

            else
            {
                if(slotDummy.itemSaved.itemName == item.itemName)
                {
                    slotDummy.itemSaved.itemCount += item.itemCount;
                    saveInventory(slotDummy, slotNumber, i);
                    break;
                }
            }
        }
        
    }

    public bool checkSubmitItem(Item[] items, int slotNumber)
    {
        int count = 0;

        InventorySlots slot;

        for(int i=0; i<8;i++)
        {
            slot =  loadInventory(slotNumber, i);
            foreach(Item item in items)
            {
                 if(slot.itemSaved.itemName != "")
                {
                    if(slot.itemSaved.itemName == item.itemName)
                    {
                         if(slot.itemSaved.itemCount >= item.itemCount)
                        {
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

    public void submitItem(Item[] items, int slotNumber)
    {
        InventorySlots slot;
         for(int j=0;j<8;j++)
        {
            foreach(Item item in items)
            {
                slot =  loadInventory(slotNumber, j);
                if(slot.itemSaved.itemName == item.itemName)
                {
                    slot.itemSaved.itemCount -= item.itemCount;

                    if(slot.itemSaved.itemCount == 0)
                    {
                        slot.itemSaved = null;
                    }

                    saveInventory(slot, slotNumber, j);
                }
            }
                
                
        }
    }


    public void unlockKeyConcept(int number, int slotNumber)
    {

        for(int i=0; i<24; i++)
        {
             KeyConcepts key =  loadKeyconcepts(slotNumber, i);
            if(key.keyNumber == number)
            {
                key.unlocked = true;
                saveKeyconcepts(key, slotNumber, i);
                return;
            }
        }

      
        // foreach(KeyConcepts concept in slot.player_glossary.conceptList)
        // {
        //     Debug.Log("KeyNumber:" + concept.keyNumber);
        //     if(concept.keyNumber == number)
        //     {
        //         Debug.Log("Approved?");
        //         concept.unlocked = true;
        //         Debug.Log("Concept status:" + concept.unlocked);
        //         saveSlot(slot, slotNumber);
        //     }
        // }
 
    }
}
