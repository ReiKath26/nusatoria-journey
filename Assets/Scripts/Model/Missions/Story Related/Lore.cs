using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lore
{
    public enum LoreType {opening, trigger, finish, interaction}

    public LoreType loreType;
    public Dialog[] loreDialog;
}
