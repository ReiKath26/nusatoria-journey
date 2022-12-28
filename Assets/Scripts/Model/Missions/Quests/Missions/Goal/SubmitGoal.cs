using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitGoal : Goal
{
   private Item[] neededItem {get; set;}
    private GameObject recipient;
    private string recipientName {get; set;}


    public void initialize(string desc, int current, int required, string rec, Story[] storyType, Item[] items)
    {
        this.recipientName = rec;
        base.initialize(desc, current, required, storyType);
            
        recipient = GameObject.Find(this.recipientName);
        neededItem = items;

    }
 
    public void OnSubmit(int number)
    {
        bool canSubmit = SaveHandler.instance.submitItem(neededItem, PlayerPrefs.GetInt("choosenSlot"));

        if(canSubmit == true)
        {
            finishObjective();
            evaluate();
            StoryManager.instance.assignStory(loadStoryOnFinish(number));
        }
    }
}

 
