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

          switch(slot.chapterNumber)
          {
               case 0:
               activeSlots[number].chapter.text = "Prologue: Permulaan dari Semuanya";
               break;
               case 1:
               activeSlots[number].chapter.text = "Chapter 1: Terjebak di Dalam Kegelapan";
               break;
               case 2:
                activeSlots[number].chapter.text = "Chapter 2: Pencarian Kebebasan Tanpa Henti";
                break;
               default: break;
          }

       }

       else
       {
          emptySlots[number].SetActive(true);
          activeSlots[number].holder.SetActive(false);
       }

       
   }

   public void continueGame(int slotSelected)
   {

     PlayerPrefs.SetInt("choosenSlot", slotSelected);

      SaveSlots slot = SaveHandler.instance.loadSlot(slotSelected);

     switch(slot.chapterNumber)
     {
          case 0:
          SceneManage.instance.LoadScene(4);
          break;

          case 1:
          SceneManage.instance.LoadScene(5);
          break;

          case 2:
          SceneManage.instance.LoadScene(6);
          break;

          default: break;
     }
   }
}
