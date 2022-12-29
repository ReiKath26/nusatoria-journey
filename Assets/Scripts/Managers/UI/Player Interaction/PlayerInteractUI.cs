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
            Show(playerInteraction.GetInteractableObject());
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
