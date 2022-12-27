using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : Story.Dialogs
{
    [SerializeField] private string npcName;
    [SerializeField] private string dialog;

    public override string getName()
    {
        return npcName;
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
}
