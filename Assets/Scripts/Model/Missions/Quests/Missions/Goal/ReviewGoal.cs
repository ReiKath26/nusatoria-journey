using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReviewGoal : Goal
{
   private Review[] reviews;
   private GameObject recipient;
   private string recipientName {get; set;}

   public ReviewGoal(string desc, int current, int required, string rec, Story[] storyType)
   {
        this.recipientName = rec;
        base.initialize(desc, current, required, storyType);
            
        recipient = GameObject.Find(this.recipientName);
   }

   public void setReview(Review[] review) 
   {
        this.reviews = review;
   }

   public Review[] getReview()
   {
        return reviews;
   }

   public void OnAllReviewDone()
   {
        bool allDone = reviews.All(r => r.getDone());

        if(allDone == true)
        {
            finishObjective();
              StoryManager.instance.assignStory(loadStoryOnFinish(0));
            evaluate();
        }
   }

}
