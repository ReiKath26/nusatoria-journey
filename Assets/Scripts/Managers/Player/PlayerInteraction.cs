using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{


    public void tryInteracting()
    {
        interactables i = GetInteractableObject();

        if (i != null)
        {
            i.interact(transform);
        }
    }

    public interactables GetInteractableObject()
    {
        Debug.Log("Searching...");
        List<interactables> interactableList = new List<interactables>();
        float range = 20;

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, range);

        foreach(Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out interactables interactable))
            {
                interactableList.Add(interactable);
            }
        }


        interactables closestInteractable = null;
        foreach(interactables interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }

            else

            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < 
                Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }
}
