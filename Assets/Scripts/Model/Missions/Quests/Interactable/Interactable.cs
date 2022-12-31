using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
   [SerializeField] private string interactName;
   [SerializeField] private bool isNpc;
   [SerializeField] private Item item;

   public void interact(Transform interactor)
   {
          if(isNpc == true)
          {
               Vector3 rotation = new Vector3(interactor.position.x - transform.position.x, 0.0f, interactor.position.z - transform.position.z);
               transform.rotation = Quaternion.LookRotation(rotation);
          }
          MissionManager.instance.triggerInteraction(gameObject);
   }

   public Item getPocketItem()
   {
      if(item.itemName != "")
      {
          return item;
      }

      else
      {
          return null;
      }
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
