using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
   [SerializeField] private string interactName;
   [SerializeField] private Item item;
   [SerializeField] private int[] missionNumber;
   [SerializeField] private string interactionTitle;
   [SerializeField] private string[] interactionDialog;
   [SerializeField] private bool isNpc;

   public void interact(Transform interactor)
   {
          bool interactionTriggered = false;

          SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

          foreach(int min_number in missionNumber)
          {
               if(slot.missionNumber == min_number)
               {
                    MissionManager.instance.triggerInteraction(gameObject);
                    interactionTriggered = true;
                    break;
               }
          } 

          if(interactionTriggered == false && isNpc == true)
          {
               showInteraction();
          }


   }

   public void showInteraction()
   {
           if(interactionTitle != "" && interactionDialog.Length != 0)
               {
                    List<Dialogs> dialog = new List<Dialogs>();
                    
                    foreach(string str in interactionDialog)
                    {
                         Dialogs dialogue = new NPCDialog("", str, null);

                         dialog.Add(dialogue);
                    }

                    Story story = new Story(interactionTitle, dialog,false);
                    StoryManager.instance.assignStory(story);
               }
   }

   public string getTitle()
   {
          return interactionTitle;
   }

   public int[] getMissionNumber()
   {
       
     return missionNumber;
       
   }

   public bool isThisNPC()
   {
     return isNpc;
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

   private void onDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 40);
    }


}
