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

   public int currentTutorial;

   public static TutorialTriggerManager instance;

   public void setTutorial()
   {
        if(currentTutorial < tutorials.Length)
        {
            tutorialOverlay.SetActive(true);
            tutorialSprite.GetComponent<LoadSpriteManage>().loadNewSprite(tutorials[currentTutorial].tutorialImage);
            tutorialTexts.text = tutorials[currentTutorial].tutorialText;
        }
       
   }
}
