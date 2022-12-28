using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogs 
{

    private string nameText {get; set;}
    private string dialogText {get; set;}
    private string[] highlightedText {get; set;}
    public bool shown {get; protected set;}

    public string getName()
    {
        return nameText;
    }

    public string getDialog()
    {
        return dialogText;
    }

    public string[] getHighlighted()
    {
        return highlightedText;
    }

    public virtual void initialize(string name, string dialog, string[] highlightTexts)
    {
        this.nameText = name;
        this.dialogText = dialog.Replace("Player", SaveHandler.instance.loadName(PlayerPrefs.GetInt("choosenSlot")));
        this.highlightedText = highlightTexts;
        shown = false;
    }

    public void showLine()
    {
        shown = true;
    }

}
