using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingEvent : MonoBehaviour
{

    public GameObject [] emptySlots;
    public ActiveSlotData [] activeSlots;



    void Start()
    {
          int i = 0;
          while(i<3)
          {
               loadSavedGameSlots(i);
               i++;
          }
    }

   public void triggerOnboarding(int SlotSelected)
   {
        PlayerPrefs.SetInt("choosenSlot", SlotSelected);
        SceneManage.instance.LoadScene(2);
   }

   public void loadSavedGameSlots(int number)
   {
       SaveSlots slot = SaveHandler.instance.loadSlot(number+1);

       if(slot != null)
       {
          emptySlots[number].SetActive(false);
          activeSlots[number].holder.SetActive(true);

          if (slot.playerGender == 0)
          {
               activeSlots[number].male_pic.SetActive(true);
               activeSlots[number].female_pic.SetActive(false);
          }

          else
          {
               activeSlots[number].male_pic.SetActive(false);
               activeSlots[number].female_pic.SetActive(true);
          }
          
          activeSlots[number].name.text = slot.playerName;
          activeSlots[number].time.text = "" + slot.time;

          //reveal last chapter

       }

       else
       {
          emptySlots[number].SetActive(true);
          activeSlots[number].holder.SetActive(false);
       }

       
   }

   public void continueGame()
   {
     //TBA
   }
}
