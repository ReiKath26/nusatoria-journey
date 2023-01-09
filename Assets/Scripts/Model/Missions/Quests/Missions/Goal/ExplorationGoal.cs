using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationGoal : Goal
{
    private GameObject[] interactionInstance;
    private int[] unlockKeyConcept {get; set;}
    private string[] interactionInstanceNames {get; set;}
    private bool usePath;

    public ExplorationGoal(string desc, int required, string[] strings, int[] key, Story[] storyType, bool path)
    {
        this.interactionInstanceNames = strings;
        this.unlockKeyConcept = key;
        this.usePath = path;
        base.initialize(desc, required, storyType);

        int count = 0;

        interactionInstance = new GameObject[interactionInstanceNames.Length];
        foreach(string instanceName in interactionInstanceNames)
        {
               if(FindInactiveObject.instance == null)
     {
          Debug.Log("It's null!");
     }

     else
     {
        Debug.Log("nay Null");
     }
            GameObject obj = FindInactiveObject.instance.find(instanceName);
            interactionInstance[count] = obj;
            count++;
        }
    }

    public bool usingPath()
    {
        return usePath;
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
