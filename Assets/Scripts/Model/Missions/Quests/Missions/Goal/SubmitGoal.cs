using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitGoal : Goal
{
    [SerializeField] private string goalDesc;
    [SerializeField] private Item[] neededItem;
    [SerializeField] private GameObject recipient;

    [SerializeField] private GameObject giveOverlay;
    [SerializeField] private GameObject[] itemHolder;
    [SerializeField] private GameObject submitButton;


    public override void initialize()
    {
        base.initialize();
 
    }

    public void OnInteractWithRecipient(GameObject interactorName)
    {
        if(xinteractorName == recipient)
        {
            giveOverlay.SetActive(true);

            for(int i=0; i<itemHolder.Length;i++)
            {
                if(i > neededItem.Length - 1)
                {
                    itemHolder[i].SetActive(false);
                }

                else
                {
                    itemHolder[i].SetActive(true);
                    //set item
                }
            }

            bool canSubmit = SaveHandler.instance.submitItem(neededItem, PlayerPrefs.GetInt("choosenSlot"));

            if(canSubmit == true)
            {
                submitButton.SetActive(true);
            }

            else
            {
                submitButton.SetActive(false);
            }
        }
    }
 

    public void OnSubmit(SubmitItemEvent eventInfo)
    {
        bool isSubmitted = eventInfo.canSubmit;

        if(isSubmitted == true)
        {
            currentAmount++;
            evaluate();
        }
    }
}
