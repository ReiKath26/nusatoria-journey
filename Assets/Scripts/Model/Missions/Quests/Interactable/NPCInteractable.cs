using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, interactables
{
   [SerializeField] private string interactText;

   private int currentMissionNumber = 0;

   public void interact(Transform interactor)
   {
        SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
        currentMissionNumber = slot.missionNumber;

        Debug.Log("Interact with" + interactText);
   }

   public string GetInteractText()
   {
        return interactText;
   }

   public Transform GetTransform()
   {
        return transform;
   }
}
