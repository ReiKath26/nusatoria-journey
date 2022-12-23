using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lore
{
    public enum LoreType {trigger, finish, casualInteraction}

    public LoreType loreType;
    public Dialog[] loreDialog;
}
