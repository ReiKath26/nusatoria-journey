using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreMission : MonoBehaviour, Mission
{
    //On trigger: show trigger story if any
    //On finish one of objectives: add objective +1
    //On finish quest: Move to next quest after story if there's any
    //Lore mission MAY or MAY NOT use proximity

    [SerializeField] private int _missionNumber;
    [SerializeField] private string _missionPrompt;
    [SerializeField] private int _objectiveCleared;
    [SerializeField] private int _requiredObjective;
    [SerializeField] private bool useProximity;

    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private GameObject storyOverlay;
    [SerializeField] private Lore [] attachedLore;

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
        foreach(Lore lore in attachedLore)
        {
            if(lore.loreType == Lore.LoreType.trigger)
            {
                DialogueManager.instance.restartTheCounter();
                DialogueManager.instance.setDialogues(lore.loreDialog);
                gameOverlay.SetActive(false);
                storyOverlay.SetActive(true);
                DialogueManager.instance.NextLine();
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
                gameOverlay.SetActive(false);
                storyOverlay.SetActive(true);
                DialogueManager.instance.NextLine();
            }
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
