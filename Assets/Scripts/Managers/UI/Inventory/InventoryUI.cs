using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] private GameObject InventoryPlaceHolder;
    [SerializeField] private GameObject SelectedInventoryPlaceHolder;
    [SerializeField] private GameObject item;

    [SerializeField] private TextMeshProUGUI itemTitle;
    [SerializeField] private TextMeshProUGUI itemDesc;
    
    public int thisNumber;

    public bool selected;

    private SaveSlots slot;

    private void Update()
    {
        slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
        foreach(InventorySlots slot in slot.player_inventory.slotList)
        {
            if(slot.slotNumber == thisNumber)
            {
                if(slot.itemSaved != null)
                {
                    setItems(slot);
                    if(selected == true)
                    {
                        InventoryPlaceHolder.SetActive(false);
                        SelectedInventoryPlaceHolder.SetActive(true);
                    }
                }

                else
                {
                    InventoryPlaceHolder.SetActive(true);
                    SelectedInventoryPlaceHolder.SetActive(false);
                }
      
            }
        }

       
    }

    public bool setItems(InventorySlots slot)
    {
        if(slot.itemSaved == null)
        {
            return false;
        }

        else
        {
            item.GetComponent<LoadSpriteManage>().loadNewSprite(slot.itemSaved.itemSprite);
            return true;
        }
    }

    public void selectItems()
    {
        foreach(InventorySlots slot in slot.player_inventory.slotList)
        {
            if(slot.slotNumber == thisNumber)
            {
                if(slot.itemSaved != null)
                {
                    itemTitle.text = slot.itemSaved.itemName;
                    itemDesc.text = slot.itemSaved.itemDesc;
                    SelectedInventoryManager.instance.triggerSelectItem(thisNumber);
                 }

                 else

                 {
                    itemTitle.text = "";
                    itemDesc.text = "";
                 }
            }
          
        }
    }




}
