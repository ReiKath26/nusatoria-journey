using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    //settings save

    public float sfxVolume;
    public float musicVolume;

    public void LoadSettings()
    {
        sfxVolume = PlayerPrefs.GetFloat("sfx_vol");
        musicVolume = PlayerPrefs.GetFloat("mus_vol");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("sfx_vol", sfxVolume);
        PlayerPrefs.SetFloat("mus_vol", musicVolume);
        PlayerPrefs.Save();
    }
   //save slot

   public int slot;
   public string playerName;
   public string playerGender;
   public float time;
   public int chapterNumber = 0;
   public int understandingLevel = 0;

   public GameObject activeCard;
   public GameObject inactiveCard;

   public void LoadSaveSlot()
   {
        if (PlayerPrefs.HasKey("playerName" + slot))
        {
            activeCard.SetActive(true);
            inactiveCard.SetActive(false);

            playerName = PlayerPrefs.GetString("playerName" + slot);
            playerGender = PlayerPrefs.GetString("playerGender" + slot);
            time = PlayerPrefs.GetFloat("playTime" + slot);
            chapterNumber = PlayerPrefs.GetInt("chap_num" + slot);
            understandingLevel = PlayerPrefs.GetInt("und_level" + slot);
        }

        else
        {
             activeCard.SetActive(false);
            inactiveCard.SetActive(true);
        }
   }

   public void SaveSlot()
   {
        PlayerPrefs.SetString("playerName" + slot, playerName);
        PlayerPrefs.SetString("playerGender" + slot, playerGender);
        PlayerPrefs.SetFloat("playTime" + slot, time);
        PlayerPrefs.SetInt("chap_num" + slot, chapterNumber);
        PlayerPrefs.SetInt("und_level" + slot, understandingLevel);
        PlayerPrefs.Save();
   }


   //in game save

   public GameObject player;
   public int missionNumber = 0;
   public int storyNumber = 0;

   public void LoadInGame()
   {
        Vector3 loadPosition = new Vector3 (
            PlayerPrefs.GetFloat("x_pos" + slot),
            PlayerPrefs.GetFloat("y_pos" + slot),
            PlayerPrefs.GetFloat("z_pos" + slot)
        );
        player.transform.position = loadPosition;
        missionNumber = PlayerPrefs.GetInt("mission_num" + slot);
        storyNumber = PlayerPrefs.GetInt("story_num" + slot);
   }

   public void SaveInGame()
   {
        Vector3 playerPosition = player.transform.position;
        PlayerPrefs.SetFloat("x_pos" + slot, playerPosition.x);
        PlayerPrefs.SetFloat("y_pos" + slot, playerPosition.y);
        PlayerPrefs.SetFloat("z_pos" + slot, playerPosition.z);
        PlayerPrefs.SetInt("mission_num" + slot, missionNumber);
        PlayerPrefs.SetInt("story_num" + slot, storyNumber);
        PlayerPrefs.Save();
   }

   //glosarium save 

    [SerializeField] private KeyConcepts[] glossary_concepts;
    [SerializeField] private TextMeshPro [] texts;


    public bool unlocked = false;

    public void glossaryInit()
    {
        foreach(KeyConcepts concept in glossary_concepts)
        {
            PlayerPrefs.SetInt("conceptUnlock" + concept.keyNumber, 0);
        }
    }

    public void LoadGlosssary()
    {
        int count = 0;
        foreach(KeyConcepts concept in glossary_concepts)
        {
            concept.unlocked = PlayerPrefs.GetInt("conceptUnlock" + concept.keyNumber) == 1 ? true: false;

            if (concept.unlocked)
            {
                texts[count].text = concept.keyName;
            }

            else
            {
                texts[count].text = "???";
            }

            count++;
        }
    }

    public void SaveGlossary(int glossaryNumber)
    {
        glossary_concepts[glossaryNumber].unlockConcept();
        PlayerPrefs.SetInt("conceptUnlock" + glossaryNumber, glossary_concepts[glossaryNumber].unlocked ? 1:0);
        PlayerPrefs.Save();
    }


   //inventory save

    //will look for better solution if possible
   [SerializeField] private Item[] items;
   [SerializeField] private Slots[] inventorySlots;
   [SerializeField] private GameObject[] itemInSlots;

   public void LoadInventory()
   {
        foreach(Slots slot in inventorySlots)
        {
            foreach(Item item in items)
            {
                if (item.itemName == PlayerPrefs.GetString("slotItem" + slot.slotNumber))
                {
                    slot.setItem(item);
                    itemInSlots[slot.slotNumber].GetComponent<Image>().sprite = item.itemSprite;
                }
            }
        }
   }

   public void SaveInventory(string itemName, int slotNumber)
   {
        foreach(Item item in items)
        {
            if (item.itemName == itemName)
            {
                inventorySlots[slotNumber].setItem(item);
                PlayerPrefs.SetString("slotItem" + slotNumber, itemName);
            }
        }
   }
}
