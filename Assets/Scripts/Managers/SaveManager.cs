using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;

public class SaveManager : MonoBehaviour
{
    string fileDestination;

    public static SaveManager instance;
    public PlayerSave saveData;

    void Awake()
    {
        instance = this;
        fileDestination = Application.persistentDataPath + "/playerData.json";
    }

    public void loadFile()
    {
        if (File.Exists(fileDestination))
        {
            string contents = File.ReadAllText(fileDestination);

            saveData = JsonUtility.FromJson<PlayerSave>(contents);
        }
    }

    public void saveFile()
    {
        string jsonString = JsonUtility.ToJson(saveData);

        File.WriteAllText(fileDestination, jsonString);
    }

    //settings save

//     public float sfxVolume;
//     public float musicVolume;

//     public void LoadSettings()
//     {
//         sfxVolume = PlayerPrefs.GetFloat("sfx_vol");
//         musicVolume = PlayerPrefs.GetFloat("mus_vol");
//     }

//     public void SaveSettings()
//     {
//         PlayerPrefs.SetFloat("sfx_vol", sfxVolume);
//         PlayerPrefs.SetFloat("mus_vol", musicVolume);
//         PlayerPrefs.Save();
//     }
//    //save slot + in game

//    public List<SaveSlots> slots;

//    public GameObject player;

//    public GameObject activeCard;
//    public GameObject inactiveCard;

//    public void addSlot(int slotNum, string playerName, int playerGender)
//    {

//    }

//    public void updateSlot()
//    {

//    }

//    public void LoadSaveSlot()
//    {
       
//    }

//    public void SaveGame()
//    {
       
//    }


//    public void ContinueGame()
//    {
//         Vector3 loadPosition = new Vector3 (
//             PlayerPrefs.GetFloat("x_pos"),
//             PlayerPrefs.GetFloat("y_pos"),
//             PlayerPrefs.GetFloat("z_pos")
//         );
//         player.transform.position = loadPosition;
//    }

   
//    //glosarium save 

//     [SerializeField] private KeyConcepts[] glossary_concepts;
//     [SerializeField] private TextMeshPro [] texts;


//     public bool unlocked = false;

//     public void glossaryInit()
//     {
//         foreach(KeyConcepts concept in glossary_concepts)
//         {
//             PlayerPrefs.SetInt("conceptUnlock" + concept.keyNumber, 0);
//         }
//     }

//     public void LoadGlosssary()
//     {
//         int count = 0;
//         foreach(KeyConcepts concept in glossary_concepts)
//         {
//             concept.unlocked = PlayerPrefs.GetInt("conceptUnlock" + concept.keyNumber) == 1 ? true: false;

//             if (concept.unlocked)
//             {
//                 texts[count].text = concept.keyName;
//             }

//             else
//             {
//                 texts[count].text = "???";
//             }

//             count++;
//         }
//     }

//     public void SaveGlossary(int glossaryNumber)
//     {
//         glossary_concepts[glossaryNumber].unlockConcept();
//         PlayerPrefs.SetInt("conceptUnlock" + glossaryNumber, glossary_concepts[glossaryNumber].unlocked ? 1:0);
//         PlayerPrefs.Save();
//     }


//    //inventory save

//     //will look for better solution if possible
//    [SerializeField] private Item[] items;
//    [SerializeField] private InventorySlots[] inventorySlots;
//    [SerializeField] private GameObject[] itemInSlots;

//    public void LoadInventory()
//    {
//         foreach(InventorySlots slot in inventorySlots)
//         {
//             foreach(Item item in items)
//             {
//                 if (item.itemName == PlayerPrefs.GetString("slotItem" + slot.slotNumber))
//                 {
//                     slot.setItem(item);
//                     itemInSlots[slot.slotNumber].GetComponent<Image>().sprite = item.itemSprite;
//                 }
//             }
//         }
//    }

//    public void SaveInventory(string itemName, int slotNumber)
//    {
//         foreach(Item item in items)
//         {
//             if (item.itemName == itemName)
//             {
//                 inventorySlots[slotNumber].setItem(item);
//                 PlayerPrefs.SetString("slotItem" + slotNumber, itemName);
//             }
//         }
//    }
}
