using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
   private Mission currentMission {get; set;}
   private Goal currentGoal {get; set;}
   [SerializeField] private string questFile;
   [SerializeField] private GameObject missionToggle;
   [SerializeField] private TextMeshProUGUI missionText;

   [SerializeField] private GameObject lineRenderer;

   //interaction goal

   [SerializeField] private GameObject getKeyconceptNotification;
   [SerializeField] private TextMeshProUGUI keyConceptText;

   //gather goal
   
   [SerializeField] private GameObject getItemNotification;
   [SerializeField] private GameObject gotItemSprite;
   [SerializeField] private TextMeshProUGUI gotItemTextShow;

   //judgement goal

   [SerializeField] private GameObject questionOverlay;
   [SerializeField] private TextMeshProUGUI nameHolder;
   [SerializeField] private TextMeshProUGUI dialogHolder;

   [SerializeField] private GameObject choice_1;
   [SerializeField] private GameObject choice_2;
   [SerializeField] private GameObject choice_3;

   [SerializeField] private AnimationCurve good;
   [SerializeField] private AnimationCurve decent;
   [SerializeField] private AnimationCurve poor;

    private float goodValue = 0f;
    private float decentValue = 0f;
    private float poorValue = 0f;

   //submit goal
   
    [SerializeField] private GameObject giveOverlay;
    [SerializeField] private GameObject[] itemHolder;
    [SerializeField] private GameObject[] neededItemSprite;
    [SerializeField] private TextMeshProUGUI neededItemTextCount;
    [SerializeField] private GameObject submitButton;

   private SaveSlots slot;

   private int missionCounter = 0;
   private int goalCounter = 0;

   public static MissionManager instance;

   void Awake()
   {
       slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
       instance = this;
       MissionProvider.instance.getChapterMissions(slot.chapterNumber);
       assignMission();
   }

   private void assignMission()
   {
       if(missionCounter < MissionProvider.instance.allMissions.Count)
       {
          currentMission = MissionProvider.instance.allMissions[missionCounter];
          goalCounter = 0;

          if(currentMission.loadTriggerStory() != null)
          {
               StoryManager.instance.assignStory(currentMission.loadTriggerStory());
          }

          assignGoal();
       }

       else
       {
          StoryManager.instance.assignStory(MissionProvider.instance.getEndChapterStory(slot.chapterNumber));
          slot.chapterNumber += 1;
       }

   }

   private void assignGoal()
   {
     if(goalCounter < currentMission.goals.Count)
     {
         currentGoal = currentMission.goals[goalCounter];
     }
   
   }

   private void update()
   {
     checkGoal();
     checkMission();
     setCurrGoal();
   }

   private void checkGoal()
   {
     if(currentGoal.getCompletion() == true)
     {
          if(currentGoal is JudgementGoal jdg_goal)
          {
               float finalScore = jdg_goal.getFinalScore();
               int currentUnderstanding = evaluateResult(finalScore);

               if(currentUnderstanding == 1)
               {

               }

               else if(currentUnderstanding == 2)
               {

               }

               else 
               {

               }
          }

          else
          {
               goalCounter++;
               assignGoal();
               displayGoal();
          }
        
     }
   }

   private void checkMission()
   {
      if(currentMission.completed)
     {
          missionCounter++;
          assignMission();
          displayGoal();
     }
   }

   private void displayGoal()
   {
     missionToggle.SetActive(true);
   }

   private void setCurrGoal()
   {
     missionText.text = currentGoal.getDescription();
   }

   public void togglePathToGoal()
   {
      if(lineRenderer.activeSelf == true)
      {
          lineRenderer.SetActive(false);
      }

      else
      {
          lineRenderer.SetActive(true);
      }
   }

   private void displayPathToGoal()
   {
      GameObject player;
      if(slot.playerGender == 0)
      {
          player = GameObject.Find("Male MC Model");
      }

      else
      {
          player = GameObject.Find("Female MC Model");
      }

      LineRenderer _line = lineRenderer.GetComponent<LineRenderer>();

     if(currentGoal is ExplorationGoal exp_goal)
     {
          int count = 0;
          Transform[] positions = new Transform[exp_goal.getInstances().Length + 1];
          foreach(GameObject obj in exp_goal.getInstances())
          {
               positions[count] = obj.transform;
               count++;
          }

          positions[count] = player.transform;

          _line.positionCount = positions.Length;
          count = 0;

          foreach(Transform pos in positions)
          {
               _line.SetPosition(count, pos.position);
               count++;
          }
     }

     else if(currentGoal is GatherGoal gth_goal)
     {
          int count = 0;
          Transform[] positions = new Transform[gth_goal.getInstances().Length + 1];
          foreach(GameObject obj in gth_goal.getInstances())
          {
               positions[count] = obj.transform;
               count++;
          }

          positions[count] = player.transform;

          _line.positionCount = positions.Length;
          count = 0;

          foreach(Transform pos in positions)
          {
               _line.SetPosition(count, pos.position);
               count++;
          }
     }

     else if(currentGoal is SubmitGoal sbm_goal)
     {
          int count = 0;
          Transform[] positions = new Transform[2];
     
          positions[0] = player.transform;
          positions[1] = sbm_goal.recipient.transform;

          _line.positionCount = positions.Length;

          foreach(Transform pos in positions)
          {
               _line.SetPosition(count, pos.position);
               count++;
          }
     }

     else if(currentGoal is JudgementGoal jdg_goal)
     {
          int count = 0;
          Transform[] positions = new Transform[2];
     
          positions[0] = player.transform;
          positions[1] = jdg_goal.recipient.transform;

          _line.positionCount = positions.Length;

          foreach(Transform pos in positions)
          {
               _line.SetPosition(count, pos.position);
               count++;
          }
     }

   }



   public void triggerInteraction(GameObject interactor)
   { 
     if(currentGoal is ExplorationGoal exp_goal)
     {
          for(int i = 0; i < exp_goal.interactionInstance.Length; i++)
          {
               if(interactor == exp_goal.interactionInstance[i])
               {
                   
                    exp_goal.OnInteract(i);
                    if (exp_goal.unlockKeyConcept[number] != -1)
                    {
                         string getKey = "";

                         int count = exp_goal.unlockKeyConcept[number];
                         for(int j=0; j < slot.player_glossary.conceptList.Count; j++)
                         {
                              while(count > 0)
                              {
                                   if(slot.player_glossary.conceptList[j].unlocked != true)
                                   {
                                        SaveHandler.instance.unlockKeyConcept(slot.player_glossary.conceptList[j], PlayerPrefs.GetInt("choosenSlot"));
                                        getKey += " " + slot.player_glossary.conceptList[j].keyName;
                                        count -= 1;
                                   }
                              }
                         }

                         keyConceptText.text = getKey;
                         getKeyconceptNotification.SetActive(true);
                        
    
                    return;
                    }
               }
          }
     }

     else if(currentGoal is GatherGoal gth_goal)
     {
          for(int i = 0; i < gth_goal.interactionInstance.Length; i++)
          {
               if(interactor == gth_goal.interactionInstance[i])
               {
                    gth_goal.OnGather(i);
                       if(interactor.TryGetComponent(out Item item))
                      {
                         SaveHandler.instance.saveItem(item, PlayerPrefs.GetInt("choosenSlot"));
                         gotItemSprite.GetComponent<LoadSpriteManage>().loadNewSprite(item.itemSprite);
                         gotItemTextShow.text = "x" + item.itemCount + " " + item.itemName;
                         StartCoroutine(showGetNotification);
                      }
               }
          }
     }

     else if(currentGoal is SubmitGoal sbm_goal)
     {
          if(interactor == sbm_goal.recipient)
          {
               giveOverlay.SetActive(true);

               for(int i=0; i<itemHolder.Length;i++)
               {
                    if(i > sbm_goal.neededItem.Length - 1)
                    {
                         itemHolder[i].SetActive(false);
                    }

                    else
                    {
                         itemHolder[i].SetActive(true);
                         neededItemSprite[i].GetComponent<LoadSpriteManage>().loadNewSprite(sbm_goal.neededItem[i].itemSprite);
                         neededItemTextCount[i].text = "x" + sbm_goal.neededItem[i].itemCount;
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

     else if(currentGoal is JudgementGoal jdg_goal)
     {
          if(interactor == jdg_goal.recipient)
          {
               startJudgement();
          }
     }
   }

   public void submitItemForSubmitGoal()
   {
      if(currentGoal is SubmitGoal sbm_goal)
      {
          sbm_goal.OnSubmit(0);
      }
   }

   public void startJudgement()
   {

   }


    private int evaluateResult(float score)
    {
        goodValue = good.Evaluate(score);
        decentValue = decent.Evaluate(score);
        poorValue = poor.Evaluate(score);

        if(goodValue > decentValue && goodValue > poorValue)
        {
            slot.understandingLevel = 3;
        }

        else if(decentValue >= goodValue && decentValue > poorValue )
        {
            slot.understandingLevel = 2;
        }

        else if(poorValue >= decentValue)
        {
            slot.understandingLevel = 1;
        }

        SaveHandler.instance.saveSlot(slot, slot.slot);

        return slot.understandingLevel;
    }

    IEnumerator showGetNotification()
    {
          getItemNotification.SetActive(true);
          yield return new WaitForSeconds(1f);
          getItemNotification.SetActive(false);
    }

   
}
