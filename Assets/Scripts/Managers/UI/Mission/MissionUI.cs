using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionUI : MonoBehaviour
{
   [SerializeField] private GameObject[] missionTriggerObjects;
   [SerializeField] private GameObject missionToggle;
   [SerializeField] private TextMeshProUGUI missionText;
   
   private SaveSlots slot;

   private void Update()
   {
        // slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

        // foreach (GameObject o in missionTriggerObjects)
        // {
        //     if(o.TryGetComponent(out Mission mission))
        //     {
        //         int number = mission.getMissionNumber();
        //         if (number == slot.missionNumber)
        //         {
        //             triggerOnNewQuest(mission.getMissionPrompt());
        //             mission.OnTriggerMission();
        //         }
        //     }
        // }
   }

   public void triggerOnNewQuest(string missionPrompt)
   {
        missionToggle.SetActive(true);
        missionText.text = missionPrompt;
   }
}
