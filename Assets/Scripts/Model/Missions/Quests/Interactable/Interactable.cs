using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
   [SerializeField] private string npcName;
   public void interact(Transform interactor)
   {
          MissionManager.instance.triggerInteraction(gameObject);
   }

   public string GetInteractText()
   {
        return npcName;
   }

   public Transform GetTransform()
   {
        return transform;
   }


}
