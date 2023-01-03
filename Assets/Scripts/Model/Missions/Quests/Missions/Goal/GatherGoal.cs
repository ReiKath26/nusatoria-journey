using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherGoal : Goal
{
   private GameObject[] interactionInstance;
   private string[] interactionInstanceNames {get; set;}
   private bool usePath;


    public GatherGoal(string desc, int required, string[] strings, Story[] storyType, bool path)
    {
        this.interactionInstanceNames = strings;
        this.usePath = path;
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

    public bool usingPath()
    {
        return usePath;
    }

    public void OnGather(int number)
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
