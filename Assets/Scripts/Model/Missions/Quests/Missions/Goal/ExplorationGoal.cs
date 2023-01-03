using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationGoal : Goal
{
    private GameObject[] interactionInstance;
    private int[] unlockKeyConcept {get; set;}
    private string[] interactionInstanceNames {get; set;}

    public ExplorationGoal(string desc, int required, string[] strings, int[] key, Story[] storyType)
    {
        this.interactionInstanceNames = strings;
        this.unlockKeyConcept = key;
        base.initialize(desc, required, storyType);

        int count = 0;

        interactionInstance = new GameObject[interactionInstanceNames.Length];
        foreach(string instanceName in interactionInstanceNames)
        {
            GameObject obj = GameObject.Find(instanceName);
            interactionInstance[count] = obj;
            count++;
        }
    }

    public GameObject[] getInstances()
    {
        return interactionInstance;
    }

    public int[] getListOfKeyConcept()
    {
        return unlockKeyConcept;
    }

    public void OnInteract(int number)
    {
        finishObjective();

        Story onFinishStory = loadStoryOnFinish(number);

        if(onFinishStory != null)
        {
             StoryManager.instance.assignStory(onFinishStory);
        }

        evaluate();
        
    }
}
