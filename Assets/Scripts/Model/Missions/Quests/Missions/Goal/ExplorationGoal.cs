using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationGoal : Mission.QuestGoal
{
    [SerializeField] private string goalDesc;
    [SerializeField] private GameObject interactionInstance;
    [SerializeField] private int unlockKeyConcept = -1;

    public override string getDescription()
    {
        return goalDesc;
    }

    public override void initialize()
    {
        base.initialize();
        EventManager.Instance.AddListener<InteractionEvent>(OnInteract);
    }

    public void OnInteract(InteractionEvent eventInfo)
    {
        if(eventInfo.interactorName == interactionInstance)
        {
            currentAmount++;

            if (unlockKeyConcept != -1)
            {
                SaveHandler.instance.unlockKeyConcept(unlockKeyConcept, PlayerPrefs.GetInt("choosenSlot"));
            }
            Evaluate();
        }
    }
}
