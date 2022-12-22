using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSlots
{
   public int slot;
   public string playerName;
   public int playerGender;
   public float time;
   public int chapterNumber;

   public PlayerPosition lastPosition;
   public int understandingLevel;
   public int missionNumber;
   public int dialogNumber;

   public Glossary player_glossary;
   public Inventory player_inventory;
}
