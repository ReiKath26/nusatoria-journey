using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedInventoryManager : MonoBehaviour
{
    private Component[] itemUI;

    public static SelectedInventoryManager instance;

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
         itemUI = GetComponents(typeof(InventoryUI));
    }

    public void triggerSelectItem(int number)
    {
        foreach(InventoryUI invent in itemUI)
        {
            if(invent.thisNumber == number)
            {
                invent.selected = true;
            }

            else

            {
                invent.selected = false;
            }
        }
    }
}
