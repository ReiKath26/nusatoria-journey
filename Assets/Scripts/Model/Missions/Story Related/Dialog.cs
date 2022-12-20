using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialog
{

   public enum DialogTypes {prologueDialogue, questDialogue, cutsceneDialogue, lightTransition, glitchTransition}

   public DialogTypes dialogType;

   public enum Speaker {mainCharacter, yudhaUnknown, yudha, npc, other}

   public Speaker speaker;

   public enum Expression {happy, sad, angry, shook, hurt, neutral, think}

   public Expression expression;

   //will try to find better implementation

   //======================================

   public bool changeScenery;

   public string[] highLightedPhrases;

   //======================================

   public string dialog;

}
