using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum characterExpression
{
    happy, sad, angry, shook, hurt, neutral, think
}

public class MainCharacterDialog : Dialogs
{
    private characterExpression expression {get; set;}
    private bool isProtagonist {get; set;}


    public MainCharacterDialog(bool isProtag, characterExpression expressions, string dialog, string[] objects)
    {
        this.isProtagonist = isProtag;
        this.expression = expressions;
        string name;

        if(this.isProtagonist == true)
        {
            name = SaveHandler.instance.loadName(PlayerPrefs.GetInt("choosenSlot"));
        }

        else
        {
            name = "Yudha";
        }

        base.initialize(name, dialog, objects);
       
    }

    public string getSpriteToBeShown()
    {
        string add_string;

        if (isProtagonist == true)
        {
            SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
            add_string = "_mc_" + slot.playerGender;
        }

        else
        {
            add_string = "_yudha";
        }

        switch(expression)
        {
            case characterExpression.happy:
            {
                return "happy" + add_string;
            }

            case characterExpression.sad:
            {
                return "sad" + add_string;
            }

            case characterExpression.angry:
            {
                return "angry" + add_string;
            }


            case characterExpression.shook:
            {
                return "shook" + add_string;
            }

            case characterExpression.hurt:
            {
                return "hurt" + add_string;
            }

            case characterExpression.neutral:
            {
                return "neutral" + add_string;
            }

            case characterExpression.think:
            {
                return "think" + add_string;
            }

            default: 
            {
               return null;
            }

        }
    }

}
