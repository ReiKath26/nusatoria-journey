using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitMission : MonoBehaviour, Mission
{

    //On trigger: display give UI, display needed item sprite, then check if player has the needed item, if not the give button is disabled
    //On finish one of objectives: remove item from player's inventory, start story if any, add objective +1
    //On finish quest: Move to next quest after story if there's any
    //Submit quest DON'T use proximity

    [SerializeField] private int _missionNumber;
    [SerializeField] private string _missionPrompt;
    [SerializeField] private int _objectiveCleared;
    [SerializeField] private int _requiredObjective;
    [SerializeField] private bool useProximity;

    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private GameObject storyOverlay;
    [SerializeField] private GameObject giveItemOverlay;
    [SerializeField] private GameObject [] neededItemSprite;
    [SerializeField] private GameObject giveButton;

    [SerializeField] private Lore [] attachedLore;
    [SerializeField] private Item [] neededItem;
    [SerializeField] private Item journal;

    private SaveSlots slot;

    void Awake()
    {
        slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
    }

    public int getMissionNumber()
    {
        return _missionNumber;
    }

    public bool isUsingProximity()
    {
        return useProximity;
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
        gameOverlay.SetActive(true);
        giveItemOverlay.SetActive(false);
        
        int itemHave = 0;

        for(int i = 0; i < neededItem.Length; i++)
        {
            neededItemSprite[i].GetComponent<LoadSpriteManage>().loadNewSprite(neededItem[i].itemSprite);

            foreach(InventorySlots slot in slot.player_inventory.slotList)
            {
                if(slot.itemSaved == neededItem[i])
                {
                    itemHave +=1;
                }
            }
        }

        if (itemHave == neededItem.Length)
        {
            giveButton.SetActive(true);
        }

        else
        {
            giveButton.SetActive(false);
        }

        
    }

    public void OnFinishObjectives()
    {
        gameOverlay.SetActive(true);
        giveItemOverlay.SetActive(false);
        
        int itemHave = 0;

        for(int i = 0; i < neededItem.Length; i++)
        {
            neededItemSprite[i].GetComponent<LoadSpriteManage>().loadNewSprite(neededItem[i].itemSprite);

            foreach(InventorySlots slot in slot.player_inventory.slotList)
            {
                if(slot.itemSaved == neededItem[i])
                {
                    itemHave +=1;
                }
            }
        }

        if (itemHave == neededItem.Length)
        {
            giveButton.SetActive(true);
        }

        else
        {
            giveButton.SetActive(false);
        }

    }

    public void reallyFinishingThisTypeOfQuest()
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
