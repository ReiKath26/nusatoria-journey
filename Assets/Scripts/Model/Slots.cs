using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slots 
{
    public int slotNumber;
    public Item itemSaved;

    public void setItem(Item item)
    {
        itemSaved = item;
    }
}
