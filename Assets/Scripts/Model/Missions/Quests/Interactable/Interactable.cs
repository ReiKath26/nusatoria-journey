using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
   [SerializeField] private string interactName;
   [SerializeField] private bool isNpc;

   public void interact(Transform interactor)
   {
          if(isNpc == true)
          {
               Vector3 rotation = new Vector3(interactor.position.x - transform.position.x, 0.0f, interactor.position.z - transform.position.z);
               transform.rotation = Quaternion.LookRotation(rotation);
          }
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
