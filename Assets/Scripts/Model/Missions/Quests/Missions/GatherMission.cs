using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherMission : MonoBehaviour, Mission
{
    //On trigger: if there's story, load the story, normally just marks which items to gather with ! mark
    //On finish one of objectives: if there's item, input item in the inventory and hide item and mark in 3D world, if there's story load the story, add objective +1
    //On finish quest: Move to next quest after story if there's any
    //gather quest MAY or MAY NOT use proximity


    [SerializeField] private int _missionNumber;
    [SerializeField] private string _missionPrompt;
    [SerializeField] private int _objectiveCleared;
    [SerializeField] private int _requiredObjective;
    [SerializeField] private Lore [] attachedLore;

    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private GameObject storyOverlay;
    [SerializeField] private Item [] getItem;

    [SerializeField] private int [] keyConceptUnlock;
    
    private SaveSlots slot;

     void Awake()
    {
        slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
    }

    public int getMissionNumber()
    {
        return _missionNumber;
    }

    public int objectiveCleared()
    {
        return _objectiveCleared;
    }

    public int requiredObjective()
    {
        return _requiredObjective;
    }

    public Lore[] getLore()
    {
        return attachedLore;
    }

    public string getMissionPrompt()
    {
        if (_requiredObjective < 1)
        {
            return _missionPrompt + "(" + _objectiveCleared + "/" + _requiredObjective + ")";
        }

        else

        {
            return _missionPrompt;
        }
        
    }

    public void OnTriggerMission()
    {
        if(gameObject.TryGetComponent(out QuestMarker marker))
        {
          marker.Show();
        }

        foreach(Lore lore in attachedLore)
        {
            if(lore.loreType == Lore.LoreType.trigger)
            {
                DialogueManager.instance.restartTheCounter();
                DialogueManager.instance.setDialogues(lore.loreDialog);
                DialogueManager.instance.NextLine();
                gameOverlay.SetActive(false);
                storyOverlay.SetActive(true);
                break;
            }

            else
            {
                gameOverlay.SetActive(true);
                storyOverlay.SetActive(false);
            }
        }

    }

    public void OnFinishObjectives()
    {
        foreach(Lore lore in attachedLore)
        {
            if(lore.loreType == Lore.LoreType.finish)
            {
                DialogueManager.instance.restartTheCounter();
                DialogueManager.instance.setDialogues(lore.loreDialog);
                DialogueManager.instance.NextLine();
                gameOverlay.SetActive(false);
                storyOverlay.SetActive(true);
                break;
            }

            else
            {
                gameOverlay.SetActive(true);
                storyOverlay.SetActive(false);
            }
        }

            if(getItem[_objectiveCleared].itemName != "")
            {
                 foreach(InventorySlots inventorySlot in slot.player_inventory.slotList)
                {
                     if(inventorySlot.itemSaved == getItem[_objectiveCleared])
                    {
                        inventorySlot.itemSaved.itemCount += 1;
                    }

                     else if (inventorySlot.itemSaved == null)
                    {
                        inventorySlot.setItem(getItem[_objectiveCleared]);
                    }
                 }

                gameObject.SetActive(false);
            }

        if(gameObject.TryGetComponent(out QuestMarker marker))
        {
          marker.Hide();
        }

        if (keyConceptUnlock.Length != 0)
        {
            foreach(KeyConcepts concept in slot.player_glossary.conceptList)
            {
                if(concept.keyNumber == keyConceptUnlock[_objectiveCleared])
                {
                    concept.unlockConcept();
                }
            }
    
        }
           

        _objectiveCleared++;
        bool questFinished = OnCheckQuestFinished();

        if (questFinished == true)
        {
            slot.missionNumber += 1;
            SaveHandler.instance.saveSlot(slot, slot.slot);
            gameOverlay.SetActive(true);
            storyOverlay.SetActive(false);
        }
    }

    public bool OnCheckQuestFinished()
    {
        int cleared = objectiveCleared();

        int required = requiredObjective();

        if(cleared== required)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    
}




