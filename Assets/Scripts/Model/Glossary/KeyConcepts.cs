using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyConcepts
{
   public int keyNumber;
   public string keyName;
   public string keyDesc;
   public bool unlocked;

   public void unlockConcept()
   {
        unlocked = true;
   }
}
