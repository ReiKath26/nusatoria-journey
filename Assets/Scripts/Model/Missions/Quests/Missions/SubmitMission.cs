using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitMission : MonoBehaviour, Mission
{

    //On interact, show giving item to NPC view and check if can give item to NPC
    [SerializeField] private int _missionNumber;
    [SerializeField] private string _missionPrompt;
    [SerializeField] private int _objectiveCleared;
    [SerializeField] private int _requiredObjective;

    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private GameObject storyOverlay;
    [SerializeField] private GameObject giveItemOverlay;
    [SerializeField] private Lore [] attachedLore;
    [SerializeField] private Item [] neededItem;

    private SaveSlots slot;

    void Awake()
    {
        slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

        if(_missionNumber == slot.missionNumber)
        {
            //set mission UI
        }
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
        gameOverlay.SetActive(true);
        giveItemOverlay.SetActive(false);

        //check if the player has item
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

        _objectiveCleared++;
        bool questFinished = OnCheckQuestFinished();

        if (questFinished == true)
        {
            slot.missionNumber += 1;
            SaveHandler.instance.saveSlot(slot, slot.slot);
            gameOverlay.SetActive(true);
            storyOverlay.SetActive(false);
        }

        else
        {
            //set mission UI
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
