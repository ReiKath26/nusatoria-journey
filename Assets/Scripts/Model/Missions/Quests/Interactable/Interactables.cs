using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface interactables
{
    public bool useProximity {get;}

    public void interact(Transform interactor);

    public string GetInteractText();
    public Transform GetTransform();
}
