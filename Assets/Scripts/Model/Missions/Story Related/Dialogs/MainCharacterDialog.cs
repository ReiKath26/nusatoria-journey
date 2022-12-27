using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum characterExpression
{
    happy, sad, angry, shook, hurt, neutral, think
}

public class MainCharacterDialog : Story.Dialogs
{
    [SerializeField] private characterExpression expression;
    [SerializeField] private string dialog;
    [SerializeField] private bool isProtagonist;

    public override string getName()
    {
        if(isProtagonist == true)
        {
            return SaveHandler.instance.loadName(PlayerPrefs.GetInt("choosenSlot"));
        }

        else
        {
            return "Yudha";
        }
    }

    public override string getDialog()
    {
        string changedDialog = dialog.Replace("Player", SaveHandler.instance.loadName(PlayerPrefs.GetInt("choosenSlot")));
        return changedDialog;
    }

    public override void initialize()
    {
        base.initialize();
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
            add_string = "_yudha"
        }

        switch(expression)
        {
            case happy:

            return "happy" + add_string;
            break;

            case sad:

            return "sad" + add_string;
            break;

            case angry:

            return "angry" + add_string;
            break;

            case shook:

            return "shook" + add_string;
            break;

            case hurt:

            return "hurt" + add_string;
            break;

            case neutral:

            return "neutral" + add_string;
            break;

            case think:

            return "think" + add_string;
            break;

            default: break;
        }
    }

}
