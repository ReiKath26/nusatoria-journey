using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public void tryInteracting()
    {
        Interactable i = GetInteractableObject();

        if (i != null)
        {
            i.interact(transform);
        }
    }

    public Interactable GetInteractableObject()
    {
        List<Interactable> interactableList = new List<Interactable>();
        float range = 20;

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, range);

        foreach(Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out Interactable interactable))
            {
                interactableList.Add(interactable);
            }
        }


        Interactable closestInteractable = null;
        foreach(Interactable interactable in interactableList)
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
