using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Goal
{
   private string description {get; set;}
   private bool completed {get; set;}
   private int currentAmount {get; set;}
   private int requiredAmount {get; set;}
   private Story[] onFinishStory {get; set;}

   public virtual void initialize(string desc, int required, Story[] storyType)
   {
        this.onFinishStory = storyType;
        this.description = desc;
        this.currentAmount = 0;
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

   public int getRequired()
   {
      return requiredAmount;
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
      Debug.Log("Goal Complete?" + completed);
        if(currentAmount >= requiredAmount)
        {
          complete();
        }
   }

   public bool checkAllStory()
   {
      if(onFinishStory != null)
      {
        bool completed = onFinishStory.All(s=>s.getCompleted());
        return completed;
      }

      else
      {
          return true;
      }
   }

   public void complete()
   {
     completed = true;
   }

   public void retractComplete()
   {
     completed = false;
     currentAmount = 0;
   }

}
