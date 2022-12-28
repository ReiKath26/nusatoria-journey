using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherGoal : Goal
{
   private GameObject[] interactionInstance;
   private string[] interactionInstanceNames {get; set;}


    public void initialize(string desc, int current, int required, string[] strings, Story[] storyType)
    {
        this.interactionInstanceNames = strings;
        base.initialize(desc, current, required, storyType);
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

    public void OnGather(int number)
    {

        finishObjective();

        StoryManager.instance.assignStory(loadStoryOnFinish(number));

        evaluate();    
    }

    
}
