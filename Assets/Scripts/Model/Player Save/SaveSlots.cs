using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSlots
{
   public int slot;
   public string playerName;
   public int playerGender;
   public int chapterNumber;

   public PlayerPosition lastPosition;
   public int understandingLevel;
   public int missionNumber;
   public int goalNumber;
}
