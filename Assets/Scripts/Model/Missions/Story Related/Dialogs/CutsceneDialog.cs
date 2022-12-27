using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDialog : Story.Dialogs
{
    [SerializeField] private string cutSceneSprite;
    [SerializeField] private string dialog;

    public override string getName()
    {
        return null;
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

    public string getSprites()
    {
        return cutSceneSprite;
    }
}
