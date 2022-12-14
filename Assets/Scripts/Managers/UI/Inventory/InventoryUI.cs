using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] private GameObject [] InventoryPlaceHolder;
    [SerializeField] private GameObject [] SelectedInventoryPlaceHolder;
    [SerializeField] private GameObject [] item;
    [SerializeField] private TextMeshProUGUI [] itemCountText;

    [SerializeField] private TextMeshProUGUI itemTitle;
    [SerializeField] private TextMeshProUGUI itemDesc;

    private SaveSlots slot;

    private void Update()
    {
        for(int i=0;i<8;i++)
        {
            InventorySlots slots = SaveHandler.instance.loadInventory(PlayerPrefs.GetInt("choosenSlot"), i);
            setItems(slots, i);
        }
       
    }

    public void setItems(InventorySlots slot, int number)
    {
        if(slot.itemSaved.itemName == "")
        {
            item[number].SetActive(false);
            itemCountText[number].text = "";
        }

        else
        {
            item[number].SetActive(true);
             itemCountText[number].text = "x" + slot.itemSaved.itemCount;
            item[number].GetComponent<LoadSpriteManage>().loadNewSprite(slot.itemSaved.itemSprite);
        }
    }

    public void selectItems(int number)
    {
        for(int i = 0; i < InventoryPlaceHolder.Length; i++)
        {
            if(i == number)
            {
                InventoryPlaceHolder[i].SetActive(false);
                SelectedInventoryPlaceHolder[i].SetActive(true);

                InventorySlots slots = SaveHandler.instance.loadInventory(PlayerPrefs.GetInt("choosenSlot"), i);

                if(slots.itemSaved != null)
                {
                    itemTitle.text = slots.itemSaved.itemName;
                    itemDesc.text = slots.itemSaved.itemDesc;
                 }

                 else

                 {
                    itemTitle.text = "";
                    itemDesc.text = "";
                 }
            }

            else
            {
                InventoryPlaceHolder[i].SetActive(true);
                SelectedInventoryPlaceHolder[i].SetActive(false);
            }

            

        }
    }
}
