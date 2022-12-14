using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyConcepts
{
   public int keyNumber;
   public string keyName;
   public string keyDesc;
   public bool unlocked = false;

   public void unlockConcept()
   {
        unlocked = true;
   }
}
