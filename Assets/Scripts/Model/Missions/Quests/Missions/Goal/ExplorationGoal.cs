using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationGoal : Goal
{
    private GameObject[] interactionInstance;
    private int[] unlockKeyConcept {get; set;}
    private string[] interactionInstanceNames {get; set;}
    private bool usePath;
    private bool[] hasInteracted;

    public ExplorationGoal(string desc, int required, string[] strings, int[] key, Story[] storyType, bool path)
    {
        this.interactionInstanceNames = strings;
        this.unlockKeyConcept = key;
        this.usePath = path;

        this.hasInteracted = new bool[interactionInstanceNames.Length];

        for(int i=0;i<interactionInstanceNames.Length;i++)
        {
            this.hasInteracted[i] = false;
        }

        base.initialize(desc, required, storyType);

        int count = 0;

        interactionInstance = new GameObject[interactionInstanceNames.Length];
        foreach(string instanceName in interactionInstanceNames)
        {
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
        if(hasInteracted[number] != true)
        {
            hasInteracted[number] = true;
            finishObjective();

            Story onFinishStory = loadStoryOnFinish(number);

            if(onFinishStory != null)
            {
                StoryManager.instance.assignStory(onFinishStory);
            }

            evaluate();
        }

        else
        {
             List<Dialogs> dialouge = new List<Dialogs>
            {
                new MainCharacterDialog(true, characterExpression.neutral, "Sebaiknya aku mengeksplorasi tempat lain di dekat sini...", null),
                       
            };
            Story alreadyExplored = new Story("Kamu sudah berinteraksi dengan benda/orang berikut...", dialouge, false, null);

            StoryManager.instance.assignStory(alreadyExplored);
        }
        
        
    }
}
