using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReviewGoal : Goal
{
   private GameObject recipient;
   private string recipientName {get; set;}

   public ReviewGoal(string desc, int required, string rec, Story[] storyType)
   {
        this.recipientName = rec;
        base.initialize(desc, required, storyType);
            
        recipient = FindInactiveObject.instance.find(this.recipientName);
   }

    public GameObject getRecipient()
    {
        return recipient;
    }

   public void OnAllReviewDone()
   {

          finishObjective();
          Story onFinishStory = loadStoryOnFinish(0);

          if(onFinishStory != null)
          {
               StoryManager.instance.assignStory(onFinishStory);
          }
          evaluate();
        
   }

}
