using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewMission : MonoBehaviour, Mission
{
    //On trigger: triggered by judgement mission, display story and review choices
    //On finish one of objectives: trigger additional judgement mission
    //On finish quest: Move to next quest after story if there's any
    //Review mission DON'T use proximity

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
        foreach(Lore lore in attachedLore)
        {
            if(lore.loreType == Lore.LoreType.trigger)
            {
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
