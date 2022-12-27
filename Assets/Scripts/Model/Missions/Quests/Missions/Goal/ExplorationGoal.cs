using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationGoal : Goal
{
    [SerializeField] private GameObject[] interactionInstance;
    [SerializeField] private int[] unlockKeyConcept;


    public override void initialize()
    {
        base.initialize();
    }

    public void OnInteract(GameObject interactorName)
    {
         foreach(GameObject instance in interactionInstance)
         {
            if(interactorName == interactionInstance)
            {
                currentAmount++;

                if (unlockKeyConcept[currentAmount - 1] != -1)
                {
                    SaveHandler.instance.unlockKeyConcept(unlockKeyConcept, PlayerPrefs.GetInt("choosenSlot"));
                }
                evaluate();
            }
         }
        
    }
}
