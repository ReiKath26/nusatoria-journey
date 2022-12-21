using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationInteractable : MonoBehaviour, interactables
{
   [SerializeField] private string interactText;
   [SerializeField] private bool _useProximity;

   public  bool useProximity => _useProximity;

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
