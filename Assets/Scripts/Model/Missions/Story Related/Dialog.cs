using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog
{

   public enum DialogTypes {storyDialogue, questDialogue}

   public DialogTypes dialogType;

   public enum SpeakerTypes {mainCharacter, npc}

   public SpeakerTypes speakerType;

   public string sprite;

   public string background;

   public string name;

   public string dialog;

   public bool autoPlay;

}
