using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherGoal : Mission.QuestGoal
{
   [SerializeField] private string goalDesc;
   [SerializeField] private GameObject interactionInstance;

    public override string getDescription()
    {
        return goalDesc;
    }

    public override void initialize()
    {
        base.initialize();
        EventManager.Instance.AddListener<GatheringGameEvent>(OnGather);
        EventManager.Instance.AddListener<SubmitItemEvent>(OnSubmit);
    }

    public void OnGather(GatheringGameEvent eventInfo)
    {
        if (eventInfo.itemObject == interactionInstance)
        {
            currentAmount++;

            if(eventInfo.itemObject.TryGetComponent(out Item item))
            {
                SaveHandler.instance.saveItem(item, PlayerPrefs.GetInt("choosenSlot"));
                Evaluate();
            }
            
        }
    }

    public void OnSubmit(SubmitItemEvent eventInfo)
    {
        foreach(string name in eventInfo.submitItemName)
        {
            bool isSubmitted = SaveHandler.instance.submitItem(name, PlayerPrefs.GetInt("choosenSlot"));

            if(isSubmitted == true)
            {
                currentAmount++;
                Evaluate();
            }
        }
    }
}
