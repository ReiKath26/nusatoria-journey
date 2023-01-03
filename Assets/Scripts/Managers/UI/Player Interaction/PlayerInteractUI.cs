using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionGameObject;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private TextMeshProUGUI interactionText;

    private void Update()
    {
        if(playerInteraction.GetInteractableObject() != null)
        {
            bool interactionValid = true;
            Interactable interactbale = playerInteraction.GetInteractableObject();
            SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

            int[] missionNumbers = interactbale.getMissionNumber();

            foreach(int mis in missionNumbers)
            {
                if(mis == slot.missionNumber)
                {
                    interactionValid = true;
                    break;
                }

                interactionValid = false;
            }

            if(interactionValid == true || interactbale.getTitle() != "")
            {
                Show(playerInteraction.GetInteractableObject());
            }
 
        }

        else
        {

            SceneManage.instance.closePopUp(interactionGameObject);
        }
    }

    public void Show(Interactable interactable)
    {
        interactionGameObject.SetActive(true);
        interactionText.text = interactable.GetInteractText();
    }
    
}
