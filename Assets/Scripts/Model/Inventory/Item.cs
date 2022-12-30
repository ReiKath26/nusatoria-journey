using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Item: MonoBehaviour
{
    public string itemSprite;
    public string itemName;
    public string itemDesc;
    public int itemCount;

    public Item(string itemSprite, string itemName, string itemDesc, int itemCount)
    {
        this.itemSprite = itemSprite;
        this.itemName = itemName;
        this.itemDesc = itemDesc;
        this.itemCount = itemCount;
    }
}
