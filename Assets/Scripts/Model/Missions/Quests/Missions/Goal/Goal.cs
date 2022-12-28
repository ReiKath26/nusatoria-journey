using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Goal
{
   private string description {get; set;}
   private bool completed {get; set;}
   private int currentAmount {get; set;}
   private int requiredAmount {get; set;}
   private Story[] onFinishStory {get; set;}

   public virtual void initialize(string desc, int current, int required, Story[] storyType)
   {
        this.onFinishStory = storyType;
        this.description = desc;
        this.currentAmount = current;
        this.requiredAmount = required;
        this.completed = false;
   }

   public string getDescription()
   {
     return description + "(" + currentAmount + "/" + requiredAmount + ")";
   }

   public bool getCompletion()
   {
     return completed;
   }

   public void finishObjective()
   {
      currentAmount++;
   }

   public int getCurrentAmount()
   {
     return currentAmount;
   }

   public Story loadStoryOnFinish(int number)
   {
     if(onFinishStory != null)
     {
          if(onFinishStory[number] != null)
          {
                return onFinishStory[number];
         }

          else
          {
               return null;
          }
     }

     else
     {
          return null;
     }
    
   }

   public void evaluate()
   {
        if(currentAmount >= requiredAmount)
        {
          complete();
        }
   }

   public void complete()
   {
     completed = true;
   }

}
