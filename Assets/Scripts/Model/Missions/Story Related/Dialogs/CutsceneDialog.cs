using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDialog : Dialogs
{
    private string cutSceneSprite {get; set;}

    public override void initialize(string sprite, string dialog, string [] highlightTexts)
    {
        this.cutSceneSprite = sprite;

        base.initialize(null, dialog, highlightTexts);
    }

    public string getSprites()
    {
        return cutSceneSprite;
    }
}
