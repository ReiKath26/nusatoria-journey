using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherGoal : Goal
{
  
   [SerializeField] private GameObject[] interactionInstance;


    public override void initialize()
    {
        base.initialize();

    }

    public void OnGather(GameObject itemObject)
    {
        foreach(GameObject instance in interactionInstance)
        {
            if (itemObject == instance)
            {
                currentAmount++;

                 if(eventInfo.itemObject.TryGetComponent(out Item item))
                 {
                    SaveHandler.instance.saveItem(item, PlayerPrefs.GetInt("choosenSlot"));
                }

                evaluate();
                return;
            
            }
        }
        
    }

    
}
