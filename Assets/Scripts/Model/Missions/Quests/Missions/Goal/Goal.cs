using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Goal
{
   [SerializeField] private string description;
   [SerializeField] private bool completed;
   public int currentAmount;
   public int requiredAmount;

   public virtual void initialize()
   {
        completed = false;
   }

   public void getDescription()
   {
        return description + currentAmount + "/" + requiredAmount;
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
