using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationInteractable : MonoBehaviour, interactables
{
   [SerializeField] private string interactText;
   [SerializeField] private Lore noQuestInteraction;
   [SerializeField] private GameObject gameOverlay;
   [SerializeField] private GameObject storyOverlay;


   private int currentMissionNumber = 0;

   public void Update()
   {
        SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
        currentMissionNumber = slot.missionNumber;
   }

     public void interact(Transform interactor)
   {
       Mission mission = getMission();

       if(mission != null)
       {
          int missionNumber = mission.getMissionNumber();
          if(currentMissionNumber == missionNumber)
          {
               mission.OnFinishObjectives();
          }

          else
          {
               DialogueManager.instance.restartTheCounter();
               DialogueManager.instance.setDialogues(noQuestInteraction.loreDialog);
               gameOverlay.SetActive(false);
               storyOverlay.SetActive(true);
               DialogueManager.instance.NextLine();
          }
       }

       else
       {
          DialogueManager.instance.restartTheCounter();
          DialogueManager.instance.setDialogues(noQuestInteraction.loreDialog);
          gameOverlay.SetActive(false);
          storyOverlay.SetActive(true);
          DialogueManager.instance.NextLine();
       }
   }

     public string GetInteractText()
     {
        return interactText;
     }

    public Transform GetTransform()
     {
        return transform;
     }

    public Mission getMission()
     {
      if(gameObject.TryGetComponent(out Mission mission))
      {
          return mission;
      }
      else
      {
          return null;
      }
      }
}
