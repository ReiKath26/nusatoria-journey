using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface interactables
{
    public void interact(Transform interactor);

    public string GetInteractText();
    public Transform GetTransform();

    public Mission getMission();
}
