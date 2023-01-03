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
            Interactable interactbale = playerInteraction.GetInteractableObject();
            SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

            if(interactbale.getMissionNumber() == slot.missionNumber || interactbale.getTitle() != "")
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
        Debug.Log("Show" );
        interactionGameObject.SetActive(true);
        interactionText.text = interactable.GetInteractText();
    }
    
}
