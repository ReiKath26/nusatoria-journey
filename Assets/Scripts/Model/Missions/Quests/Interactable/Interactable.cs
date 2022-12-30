using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
   [SerializeField] private string interactName;
   public void interact(Transform interactor)
   {
          MissionManager.instance.triggerInteraction(gameObject);
   }

   public string GetInteractText()
   {
        return interactName;
   }

   public Transform GetTransform()
   {
        return transform;
   }


}
