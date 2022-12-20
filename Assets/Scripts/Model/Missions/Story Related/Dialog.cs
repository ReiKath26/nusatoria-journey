using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialog
{

   public enum DialogTypes {storyDialogue, questDialogue, cutsceneDialogue}

   public DialogTypes dialogType;

   public enum SpeakerTypes {mainCharacter, npc}

   public SpeakerTypes speakerType;

   public string sprite;

   public bool changeScenery;

   public string name;

   public string dialog;

}
