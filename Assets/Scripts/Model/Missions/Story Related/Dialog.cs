using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialog
{

   public enum DialogTypes {prologueDialogue, questDialogue, cutsceneDialogue, lightTransition, glitchTransition}

   public DialogTypes dialogType;

   public enum Speaker {mainCharacter, yudhaUnknown, yudha, unknown, sultanAgung, tumenggungBaurekhsa, 
   pedagang, kepalaPedagang, wargaMataram, wargaKaumAdat, wargaKaumPadri, james, tuankuLintau, 
   pasukanTuankuLintau, pasukanBelanda, tuankuImamBonjol, putri, pamanPutri, francis, other}

   public Speaker speaker;

   public enum Expression {happy, sad, angry, shook, hurt, neutral, think}

   public Expression expression;

   //will try to find better implementation

   //======================================

   public bool changeScenery;

   public string[] highLightedPhrases;

   public ModelToBeMoved [] modelsToBeMoved;

   //======================================

   public string dialog;

}
