using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlots
{
   public int slot;
   public string playerName;
   public int playerGender;
   public float time;
   public int chapterNumber;

   public PlayerPosition lastPosition = new PlayerPosition() {x_pos = 0, y_pos = 0, z_pos = 0};
   public int understandingLevel;
   public int missionNumber;
   public int storyNumber;

   public Glossary player_glossary = new Glossary();
   public Inventory player_inventory = new Inventory();
}
