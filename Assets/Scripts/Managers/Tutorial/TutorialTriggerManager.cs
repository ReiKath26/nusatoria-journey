using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialTriggerManager : MonoBehaviour
{
   [SerializeField] private Tutorial[] tutorials;
   [SerializeField] private GameObject tutorialOverlay;
   [SerializeField] private GameObject tutorialSprite;
   [SerializeField] private TextMeshProUGUI tutorialTexts;

   private int currentTutorial = 0;

   public static TutorialTriggerManager instance;

   void Awake()
   {
        instance = this;
   }

   public void setTutorial()
   {
        if(currentTutorial < tutorials.Length)
        {
            tutorialOverlay.SetActive(true);
            tutorialSprite.GetComponent<LoadSpriteManage>().loadNewSprite(tutorials[currentTutorial].tutorialImage);
            tutorialTexts.text = tutorials[currentTutorial].tutorialText;
        }

        else
        {
            tutorialOverlay.SetActive(false);
            currentTutorial = 0;
        }
       
   }
}
