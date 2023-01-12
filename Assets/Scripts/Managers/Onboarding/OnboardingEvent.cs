using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingEvent : MonoBehaviour
{

    public GameObject [] emptySlots;
    public ActiveSlotData [] activeSlots;
    public GameObject deletePopUp;


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
        SceneManage.instance.LoadScene(1);
   }

   public void loadSavedGameSlots(int number)
   {
       SaveSlots slot = SaveHandler.instance.loadSlot(number+1);

       if(slot != null)
       {
          emptySlots[number].SetActive(false);
          activeSlots[number].holder.SetActive(true);
          activeSlots[number].deleteButton.SetActive(true);

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
                activeSlots[number].chapter.text = "Chapter 2: Warna yang Berbeda";
                break;
               case 3:
               activeSlots[number].chapter.text = "Bersambung...";
               break;
               default: break;
          }

       }

       else
       {
          emptySlots[number].SetActive(true);
          activeSlots[number].holder.SetActive(false);
          activeSlots[number].deleteButton.SetActive(false);
       }

       
   }

   public void continueGame(int slotSelected)
   {

     PlayerPrefs.SetInt("choosenSlot", slotSelected);

      SaveSlots slot = SaveHandler.instance.loadSlot(slotSelected);

     switch(slot.chapterNumber)
     {
          case 0:
          SceneManage.instance.LoadScene(2);
          break;

          case 1:
          SceneManage.instance.LoadScene(3);
          break;

          case 2:
          SceneManage.instance.LoadScene(4);
          break;

          default: 
          break;
     }
   }

   public void onDeleteButtonClicked(int select)
   {
          Debug.Log("Click");
          PlayerPrefs.SetInt("choosenSlot", select);
          deletePopUp.SetActive(true);
   }

   public void delete()
   {
     SaveHandler.instance.deleteSlot(PlayerPrefs.GetInt("choosenSlot"));
     deletePopUp.SetActive(false);

     loadSavedGameSlots(PlayerPrefs.GetInt("choosenSlot") - 1);
   }
}
