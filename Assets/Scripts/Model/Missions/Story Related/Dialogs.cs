using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogs 
{

    private string nameText {get; set;}
    private string dialogText {get; set;}
    public bool shown {get; protected set;}
    public string[] objectChange;

    public string getName()
    {
        return nameText;
    }

    public string getDialog()
    {
        return dialogText;
    }

    public virtual void initialize(string name, string dialog, string[] objects)
    {
        this.nameText = name;
        this.dialogText = dialog.Replace("Player", SaveHandler.instance.loadName(PlayerPrefs.GetInt("choosenSlot")));
        this.objectChange = objects;
        shown = false;
    }

    public void showLine()
    {
        shown = true;
    }

}
