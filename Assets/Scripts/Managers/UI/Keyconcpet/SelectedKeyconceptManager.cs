using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedKeyconceptManager : MonoBehaviour
{
    private Component[] keyConceptsUI;

    public static SelectedKeyconceptManager instance;

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
         keyConceptsUI = GetComponents(typeof(KeyconceptsUI));
    }

    public void triggerSelectKeyConcept(int number)
    {
        foreach(KeyconceptsUI keyconcept in keyConceptsUI)
        {
            if(keyconcept.number == number)
            {
                keyconcept.selected = true;
            }

            else

            {
                keyconcept.selected = false;
            }
        }
    }
   


}
