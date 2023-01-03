using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDialog : Dialogs
{
    private string cutSceneSprite {get; set;}

    public CutsceneDialog(string sprite, string dialog, string[] objects)
    {
        this.cutSceneSprite = sprite;

        base.initialize(null, dialog, objects);
    }

    public string getSprites()
    {
        return cutSceneSprite;
    }
}
