using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class MissionManager : MonoBehaviour
{
   private Mission currentMission {get; set;}
   private Goal currentGoal {get; set;}
   [SerializeField] private GameObject missionToggle;
   [SerializeField] private TextMeshProUGUI missionText;

   [SerializeField] private GameObject lineRenderer;
   [SerializeField] private GameObject gameOverlay;

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

   [SerializeField] private TextMeshProUGUI choice_1Text;
   [SerializeField] private TextMeshProUGUI choice_2Text;
   [SerializeField] private TextMeshProUGUI choice_3Text;

   [SerializeField] private AnimationCurve good;
   [SerializeField] private AnimationCurve decent;
   [SerializeField] private AnimationCurve poor;

    private float goodValue = 0f;
    private float decentValue = 0f;
    private float poorValue = 0f;

    private List<Question> questionDataList;
    private questionType[] needReviewType;

   //submit goal
   
    [SerializeField] private GameObject giveOverlay;
    [SerializeField] private GameObject[] itemHolder;
    [SerializeField] private GameObject[] neededItemSprite;
    [SerializeField] private TextMeshProUGUI[] neededItemTextCount;
    [SerializeField] private GameObject submitButton;

    //review goal
    
    [SerializeField] private GameObject reviewOverlay;
    [SerializeField] private GameObject[] reviewChoice;
    [SerializeField] private TextMeshProUGUI[] reviewTexts;

    [SerializeField] private GameObject reviewStoryOverlay;
    [SerializeField] private TextMeshProUGUI reviewText;

    private List<Review> toBeReviewed;
    private int reviewCount = 4;
    private int reviewLineCount = 0;
    private int choosenReview = 0;

    private Coroutine typeNextCoroutine;

   private SaveSlots slot;

   private bool isAwake = true;

   public static MissionManager instance;

   void Start()
   {      
     

      slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
     if(slot.chapterNumber == 1)
     {
          if(slot.missionNumber == 16 || slot.missionNumber == 17)
          {
               slot.missionNumber = 15;
          }
     }

     else
     {
          if(slot.missionNumber > 2)
          {
               GameObject obj = FindInactiveObject.instance.find("Yudha Paguruyung");
               if(obj.activeSelf == true)
               {
                    obj.SetActive(false);
               }
          }
          if(slot.missionNumber == 14 || slot.missionNumber == 16)
          {
               slot.missionNumber = 13;
          }

          else if(slot.missionNumber == 30 || slot.missionNumber == 31)
          {
               slot.missionNumber = 29;
          }
     }
      
       instance = this;
       assignMission();
   }

   private void assignMission()
   {
          List<Mission> allMissions = getChapterMissions(slot.chapterNumber);
          if(slot.missionNumber < allMissions.Count)
          {
               currentMission = allMissions[slot.missionNumber];

               if(isAwake != true)
               {
                    slot.goalNumber = 0;
               }

               else
               {
                    if(slot.goalNumber > 0)
                    {
                         if(slot.goalNumber > currentMission.goals.Count - 1)
                         {
                              slot.goalNumber = currentMission.goals.Count - 1;
                         }
                         int i = slot.goalNumber - 1;

                         while(i >= 0)
                         {
                              currentMission.goals[i].complete();
                              i-=1;
                         }
                    }
                   
                    isAwake = false;
               }
               

               assignGoal();
               displayGoal();
               setCurrGoal();
          }
         SaveHandler.instance.saveSlot(slot, slot.slot);
     

   }

   private void assignGoal()
   {
     if(slot.goalNumber < currentMission.goals.Count)
     {
         currentGoal = currentMission.goals[slot.goalNumber];
         checkForMissionInstance();
     }

     else
     {
          checkMission();
     }

     displayPathToGoal();
     SaveHandler.instance.saveSlot(slot, slot.slot);
   
   }

   private void checkForMissionInstance()
   {
          if(currentGoal is ExplorationGoal exp_goal)
          {
               GameObject[] inst = exp_goal.getInstances();
               for(int i = 0; i < exp_goal.getInstances().Length; i++)
               {
                    if(inst[i].activeSelf == false)
                    {
                         inst[i].SetActive(true);
                    }

               }
          }

          else if(currentGoal is GatherGoal gth_goal)
          {
               GameObject[] inst = gth_goal.getInstances();
               for(int i = 0; i < gth_goal.getInstances().Length; i++)
               {
                    if(inst[i].activeSelf == false)
                    {
                         inst[i].SetActive(true);
                    }

               }
          }

          else if(currentGoal is SubmitGoal sbm_goal)
          {
               GameObject inst = sbm_goal.getRecipient();

               if(inst.activeSelf == false)
               {
                    inst.SetActive(true);
               }
          }
           else if(currentGoal is JudgementGoal jdg_goal)
          {
               GameObject inst = jdg_goal.getRecipient();

               if(inst.activeSelf == false)
               {
                    inst.SetActive(true);
               }
          }
           else if(currentGoal is ReviewGoal rv_goal)
          {
               GameObject inst = rv_goal.getRecipient();

               if(inst.activeSelf == false)
               {
                    inst.SetActive(true);
               }
          }
   }

   private void Update()
   {
        checkGoal();
        setCurrGoal();
        displayPathToGoal();
   }
   
   private void checkGoal()
   {
     if(currentGoal.getCompletion() == true)
     {
     if(currentGoal.checkAllStory() == true)
     {
          if(currentGoal is JudgementGoal jdg_goal)
          {
               float finalScore = jdg_goal.getFinalScore();
               int currentUnderstanding = evaluateResult(finalScore);

               if(jdg_goal.getMain() == true)
               {
                    if(currentUnderstanding == 1)
                    {
                          if(slot.chapterNumber == 1)
                         {
                            slot.missionNumber = 17;
                            needReviewType = new questionType[]{questionType.latarBelakang_sa, questionType.seranganSatu_sa, questionType.seranganDua_sa, questionType.akhir_sa};
          
                              Story badStory = new Story("Tatapan dari kepala pedagang memberitahumu kalau kamu sudah gagal dalam tes kecilnya", new List<Dialogs>
                              {
                                   new NPCDialog("Kepala Pedagang", "Bah! Jelas kamu berbohong!", null),
                                   new NPCDialog("Kepala Pedagang", "Pergi dari sini dan jangan menunjukan wajahmu didepan saya lagi", null),
                                   new MainCharacterDialog(true, characterExpression.hurt, "Sudahlah tidak ada gunanya aku berdebat dengannya, sebaiknya aku mencari bantuan orang lain", null)
                               }, false, null);

                               StoryManager.instance.assignStory(badStory);

                         }

                          else
                         {
                              if(slot.missionNumber == 13)
                              {
                                   slot.missionNumber = 15;
                                   Story badStory = new Story("Pasukan itu langsung membawamu dan Yudha paksa tanpa peringatan", new List<Dialogs>
                                   {
                                        new NPCDialog("Pasukan Kaum Padri", "Sudah sekarang kalian ikut denganku, orang-orang mencurigakan", null),
                                        new MainCharacterDialog(true, characterExpression.angry, "Hei, lepaskan! Aku bisa jalan sendiri tau!", null),
                                        new MainCharacterDialog(true, characterExpression.angry, "Beritahukan saja arahnya kemana, kami tidak akan kabur", null),
                                        new MainCharacterDialog(false, characterExpression.shook, "Kalau aku jadi kamu sih, sudah pasti langsung kabur...", null)
                                    }, false, null);

                                   StoryManager.instance.assignStory(badStory);
                              }

                              else
                              {
                                   slot.missionNumber = 31;
                                   Story badStory = new Story("Paman Putri mendengarkanmu dengan seksama, tapi karena beberapa jawabanmu tidak sesuai yang dia ingat, Yudha memperhatikanmu dengan tatapan yang kurang enak", new List<Dialogs>
                                   {
                                        new NPCDialog("Paman Putri", "Hmmm...apakah itu sudah semuanya?", null),
                                        new MainCharacterDialog(false, characterExpression.angry, "Maaf tuan, sebelum kita lanjutkan bisakah aku bicara dengan Player sebentar?", null),
                                        new NPCDialog("Paman Putri", "Oh tentu, silahkan-silahkan...", null),
                                        new MainCharacterDialog(false, characterExpression.angry, "Player, temui aku sebentar...", null),
                                        new MainCharacterDialog(true, characterExpression.shook, "(Kenapa firasatku tidak enak...)", null)
                                    }, false, null);

                                   StoryManager.instance.assignStory(badStory);
                              }
                         }
                    }

               else if(currentUnderstanding == 2)
               {
                   if(slot.chapterNumber == 1)
                   {
                         slot.missionNumber = 16;

                           Story badStory = new Story("Kepala Pedagang nampak ragu...", new List<Dialogs>
                              {
                                   new NPCDialog("Kepala Pedagang", "Lumayan sih...", null),
                                   new NPCDialog("Kepala Pedagang", "Tapi ada beberapa yang kamu salah mengerti, aku harus memperbaiki pemahamanmu tentang itu", null),
                                   new MainCharacterDialog(true, characterExpression.hurt, "Oh tidak...", null)
                               }, false, null);

                               StoryManager.instance.assignStory(badStory);

                   }

                   else
                   {
                          if(slot.missionNumber == 13)
                         {
                              slot.missionNumber = 14;
                                 Story badStory = new Story("Pasukan itu terlihat menimbang-nimbang sesuatu", new List<Dialogs>
                              {
                                   new NPCDialog("Pasukan Kaum Padri", "Hmmm....", null),
                                   new NPCDialog("Pasukan Kaum Padri", "Beberapa pengertianmu itu masih belum benar, aku tidak bisa membiarkan kalian pergi tanpa membenarkan kesalahan kalian", null),
                                   new MainCharacterDialog(true, characterExpression.hurt, "Oh tidak...", null)
                               }, false, null);

                              StoryManager.instance.assignStory(badStory);
                         }

                         else
                         {
                              slot.missionNumber = 30;
                             
                              Story badStory = new Story("Yudha tampak menggeleng-gelengkan kepalanya...", new List<Dialogs>
                              {
                                   new NPCDialog("Paman Putri", "Hmmm...apakah itu sudah semuanya...", null),
                                   new MainCharacterDialog(false, characterExpression.angry, "Sepertinya pengertianmu itu masih ada yang kurang, sebaiknya kita mereview beberapa kejadian sebentar", null)
                               }, false, null);

                               StoryManager.instance.assignStory(badStory);
                         }
                   }
               }

               else 
               {

                   if(slot.chapterNumber == 1)
                   {
                         slot.missionNumber = 19;
                   }

                   else
                   {
                          if(slot.missionNumber == 13)
                         {
                              slot.missionNumber =  17;
                             
                         }

                         else
                         {
                              slot.missionNumber = 32;

                         }
                   }
                    gameOverlay.SetActive(true); 
               }

                    assignMission();
                    displayGoal();
               }

               else
               {
                    if(currentUnderstanding < 2)
                    {
                         jdg_goal.retractComplete();
                         slot.goalNumber = 0;
                         displayGoal();
                    }
                
                    else
                    {
                         if(slot.chapterNumber == 1)
                         {
                              slot.missionNumber =  19;
                              assignMission();
                              displayGoal();

                         }

                         else
                         {
                              if(slot.missionNumber == 14 || slot.missionNumber == 16)
                              {
                                   slot.missionNumber =  17;
                                   assignMission();
                                   displayGoal();
                              }

                              else
                              {
                                   if(slot.understandingLevel == 1)
                                   {
                                        slot.missionNumber =  33;
                                        assignMission();
                                        displayGoal();
                                   }

                                   else
                                   {
                                        slot.missionNumber =  32;
                                        assignMission();
                                        displayGoal();
                                   }
                              }
                         }

                         
                    }

                    gameOverlay.SetActive(true); 
               }    
          }

          else if(currentGoal is ExplorationGoal ex_goal)
          {
               
               for(int i = 0; i < ex_goal.getInstances().Length; i++)
               {
               int[] listOfConcepts = ex_goal.getListOfKeyConcept();
               if (listOfConcepts[i] != -1)
               {
                    string getKey = "";

                    int count = listOfConcepts[i];
                    int lastLocked = 0;
                    for(int j=0; j<24; j++)
                    {
                         KeyConcepts concept = SaveHandler.instance.loadKeyconcepts(PlayerPrefs.GetInt("choosenSlot"), j);
                         if(concept.unlocked != true)
                         {
                              lastLocked = j;
                              break;
                         }
                        
                    }

                    for(int k=lastLocked; k<count + lastLocked;k++)
                    {
                         KeyConcepts concept = SaveHandler.instance.loadKeyconcepts(PlayerPrefs.GetInt("choosenSlot"), k);
                          if(concept.unlocked != true)
                         {
                              SaveHandler.instance.unlockKeyConcept(k, PlayerPrefs.GetInt("choosenSlot"));
                              getKey += "[" + concept.keyName + "]";
                         }
                    }

                    keyConceptText.text = getKey;
                    getKeyconceptNotification.SetActive(true);
                    AudioManager.instance.Play("Get");
                        
               }
          }

               slot.goalNumber++;
               assignGoal();
               displayGoal();
          }

          else
          {
               slot.goalNumber++;
               assignGoal();
               displayGoal();
          }
     }
        
     }
   }

   private void checkMission()
   {
     currentMission.evaluate();
      if(currentMission.completed)
     {
          slot.missionNumber++;
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

     if(currentGoal is ExplorationGoal exp_goal && exp_goal.usingPath() == true)
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

     else if(currentGoal is GatherGoal gth_goal && gth_goal.usingPath() == true)
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
          positions[1] = sbm_goal.getRecipient().transform;

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
          positions[1] = jdg_goal.getRecipient().transform;

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
          for(int i = 0; i < exp_goal.getInstances().Length; i++)
          {
               GameObject[] inst = exp_goal.getInstances();

               if(inst[i] != null)
               {
                    if(interactor == inst[i])
                    {
                         exp_goal.OnInteract(i);
                         return;
                    }
               }
              
          }
     }

     else if(currentGoal is GatherGoal gth_goal)
     {
          for(int i = 0; i < gth_goal.getInstances().Length; i++)
          {
               GameObject[] inst = gth_goal.getInstances();
               if(interactor == inst[i])
               {
                    gth_goal.OnGather(i);
                       if(interactor.TryGetComponent(out Interactable interactable))
                      {
                         Item item = interactable.getPocketItem();

                         if(item != null)
                         {
                              if(!interactable.isThisNPC() == true)
                              {
                                   inst[i].SetActive(false);
                              }
                              SaveHandler.instance.saveItem(item, PlayerPrefs.GetInt("choosenSlot"));
                              gotItemSprite.GetComponent<LoadSpriteManage>().loadNewSprite(item.itemSprite);
                              gotItemTextShow.text = "x" + item.itemCount + " " + item.itemName;
                              StartCoroutine(showGetNotification());
                         }
                        
                      }
               }
          }
     }

     else if(currentGoal is SubmitGoal sbm_goal)
     {
          if(interactor == sbm_goal.getRecipient())
          {
               Item[] listOfItem = sbm_goal.getItemNeeded();
               giveOverlay.SetActive(true);

               for(int i=0; i<itemHolder.Length;i++)
               {
                    if(i > listOfItem.Length - 1)
                    {
                         itemHolder[i].SetActive(false);
                    }

                    else
                    {
                         itemHolder[i].SetActive(true);
                         neededItemSprite[i].GetComponent<LoadSpriteManage>().loadNewSprite(listOfItem[i].itemSprite);
                         neededItemTextCount[i].text = "x" + listOfItem[i].itemCount;
                    }
               }

            bool canSubmit = SaveHandler.instance.checkSubmitItem(listOfItem, PlayerPrefs.GetInt("choosenSlot"));

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
          if(interactor == jdg_goal.getRecipient())
          {
               startJudgement();
          }
     }

     else if(currentGoal is ReviewGoal rev_goal)
     {
          if(interactor == rev_goal.getRecipient())
          {
               assignReview();
          }
         
     }

   }

   public void submitItemForSubmitGoal()
   {
      if(currentGoal is SubmitGoal sbm_goal)
      {
          sbm_goal.OnSubmit(0);
          giveOverlay.SetActive(false);
      }
   }

   public void answerChoice(int number)
   {
      if(currentGoal is JudgementGoal jd_goal )
      {
          if(jd_goal.getMain() == true)
          {
               Choice[] choices = questionDataList[jd_goal.getCurrentAmount()].getChoices();

               if(!choices[number].correct)
               {
                    if(!needReviewType.Contains(questionDataList[jd_goal.getCurrentAmount()].getQuestionType()))
                    {
                         for(int i=0;i<needReviewType.Length; i++)
                         {
                              if(needReviewType[i] == questionType.not_set)
                              {
                                   needReviewType[i] = questionDataList[jd_goal.getCurrentAmount()].getQuestionType();
                                   break;
                              }
                         }
                        
                    }
               }
               jd_goal.OnAnswerQuestion(questionDataList[jd_goal.getCurrentAmount()], number, questionDataList.Count);
               nextQuestion();
          }

          else
          {
               jd_goal.OnAnswerQuestion(questionDataList[jd_goal.getCurrentAmount()], number, questionDataList.Count);
               nextQuestion();
          }
         
      }
   }

   public void startJudgement()
   {
       setUpQuestion();
       questionOverlay.SetActive(true);
       gameOverlay.SetActive(false);

       if(slot.chapterNumber == 1)
       {
          needReviewType = new questionType[4] {questionType.not_set, questionType.not_set, questionType.not_set, questionType.not_set};
       }
       else
       {
          needReviewType = new questionType[3] {questionType.not_set, questionType.not_set, questionType.not_set};
       }
 
       nextQuestion();
       
   }

   public void nextQuestion()
   {
      if(currentGoal.getCurrentAmount() < questionDataList.Count)
      {
          nameHolder.text = questionDataList[currentGoal.getCurrentAmount()].getnpcName();
          dialogHolder.text = questionDataList[currentGoal.getCurrentAmount()].getQuestionDescription();

          Choice[] choices = questionDataList[currentGoal.getCurrentAmount()].getChoices();
          choice_1Text.text = choices[0].choiceDescription;
          choice_2Text.text = choices[1].choiceDescription;
          choice_3Text.text = choices[2].choiceDescription;
      }

      else
      {
          questionOverlay.SetActive(false);
          checkGoal();
      }
   }

   public void setUpQuestion()
   { 
     List<Question> easyQuestion;
     List<Question> mediumQuestion;
     List<Question> hardQuestion;

     questionDataList = new List<Question>();

      if(slot.chapterNumber == 1)
      {
          easyQuestion = new List<Question>
          {
               new Question("Kepala Pedagang", "Apa yang kamu bisa pelajari dari serangan Sultan Agung?", 
               new Choice[]
               {
                    new Choice("Untuk tidak menyerah membela apa yang benar", true),
                    new Choice("Untuk selalu mempersiapkan segala sesuatu dengan baik", true),
                    new Choice("Untuk belajar dari kesalahan dan menjadi lebih baik kedepannya", true)
               }, level.easy, questionType.akhir_sa),
               new Question("Kepala Pedagang", "Apa akibat dari penyerangan Jepara oleh Van den Marct?", 
               new Choice[]
               {
                    new Choice("Mataram mengalami kebakaran", false),
                    new Choice("Mataram mengalami kerugian besar", true),
                    new Choice("Mataram berhasil menangani serangan tersebut", false)
               }, level.easy, questionType.latarBelakang_sa),
               new Question("Kepala Pedagang", "Apa yang menghalangi cita-cita Sultan Agung?", 
               new Choice[]
               {
                    new Choice("Keberadaan Spanyol di tanah Jawa", false),
                    new Choice("Keberadaan Inggris di tanah Jawa", false),
                    new Choice("Keberadaan Belanda di tanah Jawa", true)
               }, level.easy, questionType.latarBelakang_sa),
               new Question("Kepala Pedagang", "Apa kamu masih ingat apa cita-cita dari Sultan Agung?", 
               new Choice[]
               {
                    new Choice("Membubarkan keberdaan kongsi dagang VOC", false),
                    new Choice("Menyatukan seluruh rakyatnya", false),
                    new Choice("Menyatukan tanah Jawa dan mengusir bangsa asing", true)
               }, level.easy, questionType.latarBelakang_sa),
               new Question("Kepala Pedagang", "Apa yang dilakukan J.P. Coen dalam masa kekuasaannya?", 
               new Choice[]
               {
                    new Choice("Membantu warga Mataram", false),
                    new Choice("Menjalin hubungan baik dengan Mataram", false),
                    new Choice("Mengeksploitasi hasil bumi", true)
               }, level.easy, questionType.latarBelakang_sa),


               new Question("Kepala Pedagang", "Apa yang dilakukan Sultan Agung setelah serangannya tidak berhasil?", 
               new Choice[]
               {
                    new Choice("Ia menyerah pada Belanda", false),
                    new Choice("Ia terus berjuang sampai akhir meskipun akhirnya gagal", true),
                    new Choice("Ia mati di medan pertempuran", false)
               }, level.easy, questionType.akhir_sa),

               new Question("Kepala Pedagang", "Siapa yang menyuruh Van den Marct untuk menyerang Jepara?", 
               new Choice[]
               {
                    new Choice("J. P. Coen", true),
                    new Choice("Pieter Booth", false),
                    new Choice("Daendels", false)
               }, level.easy, questionType.latarBelakang_sa),

               new Question("Kepala Pedagang", "Apa hasil dari serangan pertama Sultan Agung?", 
               new Choice[]
               {
                    new Choice("Pasukan Mataram berhasil memukul mundur VOC", false),
                    new Choice("Pasukan Mataram gagal dan terpaksa mundur", true),
                    new Choice("Pasukan Mataram mengadakan perjanjian damai dengan Belandaa", false)
               }, level.easy, questionType.seranganSatu_sa),
               new Question("Kepala Pedagang", "Apa hasil dari serangan kedua Sultan Agung?", 
               new Choice[]
               {
                    new Choice("Pasukan Mataram gagal karena wabah dan lumbung perbekalan hancur", true),
                    new Choice("Pasukan Mataram berhasil memukul mundur VOC", false),
                    new Choice("Kerajaan Mataram dikuasai Belanda", false)
               }, level.easy, questionType.seranganDua_sa),
               new Question("Kepala Pedagang", "Apa yang terjadi setelah Sultan Agung meninggal?", 
               new Choice[]
               {
                    new Choice("Raja yang baru memukul mundur VOC dari Mataram", false),
                    new Choice("Mataram dikuasai oleh VOC", true),
                    new Choice("Bawahan Sultan Agung terus berjuang tanpanya", false)
               }, level.easy, questionType.akhir_sa),

          };

          mediumQuestion = new List<Question>
          {
               new Question("Kepala Pedagang", "Kejadian apa yang menjadi penyebab memburuknya hubungan Mataram dan Belanda?", 
               new Choice[]
               {
                    new Choice("Belanda yang serakah dan ingin memonopoli perdagangan di Mataram", false),
                    new Choice("Terangkatnya J.P. Coen menjadi Gubernur Jendral VOC", false),
                    new Choice("Perintah J.P. Coen pada Van den Marct untuk menyerang Jepara", true)
               }, level.medium, questionType.latarBelakang_sa),
               new Question("Kepala Pedagang", "Kapan serangan Mataram terhadap VOC pertama kali terjadi?", 
               new Choice[]
               {
                    new Choice("Agustus 1628", true),
                    new Choice("Oktober 1628", false),
                    new Choice("Desember 1628", false)
               }, level.medium, questionType.seranganSatu_sa),
               new Question("Kepala Pedagang", "Apa yang terjadi setelah serangan-serangan Sultan Agung?", 
               new Choice[]
               {
                    new Choice("Sultan Agung melancarkan serangan lagi dan menang", false),
                    new Choice("Mataram mulai menurun sepeninggal Sultan Agung", true),
                    new Choice("Mataram tetap berjaya dibawah raja baru sepeninggal Sultan Agung", false)
               }, level.medium, questionType.akhir_sa),
               new Question("Kepala Pedagang", "Dimana serangan Mataram terhadap VOC pertama kali terjadi?", 
               new Choice[]
               {
                    new Choice("Batavia", true),
                    new Choice("Cirebon", false),
                    new Choice("Jepara", false)
               }, level.medium, questionType.seranganSatu_sa),
               new Question("Kepala Pedagang", "Kapan Mataram melancarkan serangan kedua mereka?", 
               new Choice[]
               {
                    new Choice("1628", true),
                    new Choice("1629", false),
                    new Choice("1630", false)
               }, level.medium, questionType.seranganDua_sa),
               new Question("Kepala Pedagang", "Di tahun berapa Sultan Agung meninggal dunia?", 
               new Choice[]
               {
                    new Choice("1646", false),
                    new Choice("1655", false),
                    new Choice("1645", true)
               }, level.medium, questionType.akhir_sa),

               new Question("Kepala Pedagang", "Setelah pasukan Tumenggung Baureksa menyerang Batavia, menyusul pasukan Mataram kedua pada bulan...", 
               new Choice[]
               {
                    new Choice("September 1628", false),
                    new Choice("Oktober 1628", true),
                    new Choice("Desember 1628", false)
               }, level.medium, questionType.seranganSatu_sa),

               new Question("Kepala Pedagang", "Kapan J.P. Coen mempunyai kontrol penuh atas kota Batavia?", 
               new Choice[]
               {
                    new Choice("1620", false),
                    new Choice("1619", true),
                    new Choice("1618", false)
               }, level.medium, questionType.latarBelakang_sa),
               new Question("Kepala Pedagang", "Siapa nama pemimpin pasukan serangan pertama Sultan Agung?", 
               new Choice[]
               {
                    new Choice("Tumenggung Baurekhsa", true),
                    new Choice("Tumenggung Prabuwijaya", false),
                    new Choice("Tumenggung Sura Agul Agul", false)
               }, level.medium, questionType.seranganSatu_sa),
               new Question("Kepala Pedagang", "Kapan tanggal mundurnya Pasukan Mataram pada serangan pertama?", 
               new Choice[]
               {
                    new Choice("9 Desember 1628", false),
                    new Choice("6 Desember 1628 ", true),
                    new Choice("6 Desember 1629", false)
               }, level.medium, questionType.seranganSatu_sa),

          };

          hardQuestion = new List<Question>
          {
               new Question("Kepala Pedagang", "Dimana Pasukan Sultan Agung membangun lumbung perbekalan untuk serangan kedua?", 
               new Choice[]
               {
                    new Choice("Batavia dan Bandung", false),
                    new Choice("Cirebon dan Priyangan", true),
                    new Choice("Priyangan dan Jepara", false)
               }, level.hard, questionType.seranganDua_sa),
               new Question("Kepala Pedagang", "Apa faktor yang menyebabkan kekalahan pasukan Mataram pada serangan pertama?", 
               new Choice[]
               {
                    new Choice("Belanda membakar kapal dari Mataram", false),
                    new Choice("Kekurangan perbekalan", true),
                    new Choice("Tumenggung Baureksha terbunuh", false)
               }, level.hard, questionType.seranganSatu_sa),
               new Question("Kepala Pedagang", "Apa faktor yang menyebabkan kekalahan pasukan Mataram pada serangan pertama?", 
               new Choice[]
               {
                    new Choice("Belanda memblokade kapal pasukan Mataram", false),
                    new Choice("Adanya mata-mata dari Belanda yang masuk", false),
                    new Choice("Persenjataan Belanda yang lebih modern", true)
               }, level.hard, questionType.seranganSatu_sa),
               new Question("Kepala Pedagang", "Apa penyebab dari kegagalan serangan kedua?", 
               new Choice[]
               {
                    new Choice("Cuaca yang buruk di medan pertempuran", false),
                    new Choice("Belanda mengambil alih lumbung perbekalan", false),
                    new Choice("Wabah kolera dan malaria ", true)
               }, level.hard, questionType.seranganDua_sa),
               new Question("Kepala Pedagang", "Nama kaki tangan Belanda yang diminta untuk menyerang Jepara adalah...", 
               new Choice[]
               {
                    new Choice("Van den Bosch", false),
                    new Choice("Van Vermillion", false),
                    new Choice("Van de Marct", true)
               }, level.hard, questionType.latarBelakang_sa),


               new Question("Kepala Pedagang", "J. P. Coen berkuasa dari tahun..", 
               new Choice[]
               {
                    new Choice("1618 -1624 dan 1628 - 1629", false),
                    new Choice("1619 -1623 dan 1627 - 1629", true),
                    new Choice("1620 - 1624 dan 1628 -1630", false)
               }, level.hard, questionType.latarBelakang_sa),

               new Question("Kepala Pedagang", "Apa yang menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaan J.P. Coen dan VOC?", 
               new Choice[]
               {
                    new Choice("Tindakan semena-mena dan eksploitasi hasil bumi", true),
                    new Choice("Penyerangan tanpa alasan kepada kerajaan-kerjaan", false),
                    new Choice("Dianggap sebagai orang asing dan tidak berhak ada di Nusantara", false)
               }, level.hard, questionType.latarBelakang_sa),

               new Question("Kepala Pedagang", "Siapa nama dua bersaudara yang menyusul pasukan Tumenggung Baureksa pada serangan pertama?", 
               new Choice[]
               {
                    new Choice("Kiai Dipati Mandurojo dan Upa Santa", true),
                    new Choice("Kiai Dipati Mandurojo dan Upa Santo", false),
                    new Choice("Kiai Bupati Mandurojo dan Upa Santo", false)
               }, level.hard, questionType.seranganSatu_sa),
               new Question("Kepala Pedagang", "Apa yang terjadi setelah serangan pertama Mataram gagal?", 
               new Choice[]
               {
                    new Choice("Mataram terpaksa mundur", true),
                    new Choice("Mataram terus berjuang ", false),
                    new Choice("Mataram menerapkan sistem gerilya", false)
               }, level.hard, questionType.seranganSatu_sa)

          };
      }

      else
      {
          if(slot.missionNumber < 20)
          {
                easyQuestion = new List<Question>
          {
               new Question("Pasukan", "Siapa nama dari orang Belanda yang mengadakan perjanjian persahabatan dengan kaum Adat?",
               new Choice[]
               {
                    new Choice("Jack Du Puy", false),
                    new Choice("James Du Puy", true),
                    new Choice("Jamie Du Puy", false)
               }, level.easy, questionType.latarBelakang_padri),

               new Question("Pasukan", "Apa yang terjadi setelah Simawang diduduki Belanda?",
               new Choice[]
               {
                    new Choice("Kaum Padri tidak merespons apa-apa", false),
                    new Choice("Kaum Padri panik dan bingung harus apa", false),
                    new Choice("Kaum Padri menentang keras tindakan tersebut", true)
               }, level.easy, questionType.latarBelakang_padri),

               new Question("Pasukan", "Siapa nama tuanku yang memimpin perjuangan di Lintau?",
               new Choice[]
               {
                    new Choice("Tuanku Imam Bonjol", false),
                    new Choice("Tuanku Lintau", true),
                    new Choice("Tuanku Nan Renceh", false)
               }, level.easy, questionType.perjuangan_tl),

               new Question("Pasukan", "Berapa banyak pasukan yang dikerahkan Tuanku Lintau untuk mengadakan serangan?",
               new Choice[]
               {
                    new Choice("15.000 sampai 20.000", false),
                    new Choice("25.000 sampai 30.000", false),
                    new Choice("20.000 sampai 25.000", true)
               }, level.easy, questionType.perjuangan_tl),

               new Question("Pasukan", "James Du Puy mengadakan perjanjian persahabatan dengan tokoh Adat. yaitu...",
               new Choice[]
               {
                    new Choice("Tuanku Suruaso dan 12 Penghulu Minangkabau", false),
                    new Choice("Tuanku Suruaso dan 13 Penghulu Minangkabau", false),
                    new Choice("Tuanku Suruaso dan 14 Penghulu Minangkabau", true)
               }, level.easy, questionType.latarBelakang_padri),

               new Question("Pasukan", "Dimana Tuanku Nan Renceh memimpin pasukannya?",
               new Choice[]
               {
                    new Choice("Baso", true),
                    new Choice("Batavia", false),
                    new Choice("Lintau", false)
               }, level.easy, questionType.latarBelakang_padri),

               new Question("Pasukan", "Berapa pasukan yang meninggal setelah serangan Tuanku Lintau?",
               new Choice[]
               {
                    new Choice("150 orang", false),
                    new Choice("250 orang", false),
                    new Choice("350 orang", true)
               }, level.easy, questionType.perjuangan_tl),

               new Question("Pasukan", "Bagaimana sikap Kaum Padri pada Kaum Adat pada awalnya?",
               new Choice[]
               {
                    new Choice("Mendukung apa yang Kaum Adat lakukan", false),
                    new Choice("Menentang apa yang Kaum Adat lakukan", true),
                    new Choice("Biasa saja terhadap apa yang Kaum Adat lakukan", false)
               }, level.easy, questionType.perjuangan_tl),

               new Question("Pasukan", "Berapa pasukan Eropa termasuk kaum Adat yang dipersiapkan oleh Belanda untuk melawan serangan Tuanku Lintau?",
               new Choice[]
               {
                    new Choice("100 serdadu Eropa ", false),
                    new Choice("200 serdadu Eropa", true),
                    new Choice("300 serdadu Eropa", false)
               }, level.easy, questionType.perjuangan_tl),
          };
                mediumQuestion = new List<Question>
                { 
                new Question("Pasukan", "Apa penyebab kaum Adat dan Kaum Padri bertentangan?",
                new Choice[]
                {
                    new Choice("Kaum Adat bersikap kurang ajar terhadap Kaum Padri", false),
                    new Choice("Perbedaan praktik keagamaan", true),
                    new Choice("Kaum Padri mengajarkan pembaruan praktik Islam", false)
                }, level.medium, questionType.latarBelakang_padri),

                new Question("Pasukan", "Kapan perjanjian persahabatan antara Belanda dan kaum Adat terjadi?",
                new Choice[]
                {
                    new Choice("10 April 1821", false),
                    new Choice("10 Agustus 1821", false),
                    new Choice("10 Februari 1821", true)
                }, level.medium, questionType.latarBelakang_padri),

                new Question("Pasukan", "Pada tahun berapa ya Perang Padri mulai?",
                new Choice[]
                {
                    new Choice("1820", false),
                    new Choice("1821", true),
                    new Choice("1822", false)
                }, level.medium, questionType.latarBelakang_padri),

                new Question("Pasukan", "Hasil dari serangan Tuanku Lintau adalah...",
                new Choice[]
                {
                    new Choice("Tuanku Lintau menang dari pasukan Belanda", false),
                    new Choice("Tuanku Lintau ditangkap oleh Belanda", false),
                    new Choice("Tuanku Lintau mundur ke Lintau", true)
                }, level.medium, questionType.perjuangan_tl),

                new Question("Pasukan", "Apa saja yang dilakukan oleh Kaum Adat yang dilarang oleh ajaran Islam?",
                new Choice[]
                {
                    new Choice("Judi, memelihara ayam, dan minum-minuman keras", false),
                    new Choice("Judi, sabung ayam, dan minum-minuman buah", false),
                    new Choice("Judi, sabung ayam, dan minum-minuman keras", true)
                }, level.medium, questionType.latarBelakang_padri),

                new Question("Pasukan", "James Du Puy diangkat menjadi residen di Minangkabau pada tahun...",
                new Choice[]
                {
                    new Choice("1821", true),
                    new Choice("1822", false),
                    new Choice("1823", false)
                }, level.medium, questionType.latarBelakang_padri),

                new Question("Pasukan", "Perjanjian Masang dibuat karena...",
                new Choice[]
                {
                    new Choice("Belanda merasa terdesak dengan kaum Padri", true),
                    new Choice("Belanda serius ingin berdamai dengan kaum Padri", false),
                    new Choice("Belanda menginginkan persahabatan dengan kaum Padri", false)
                }, level.medium, questionType.perjuangan_kp),

                new Question("Pasukan", "Fase pertama Perang Padri terjadi dari tahun sampai tahun berapa?",
                new Choice[]
                {
                    new Choice("1821 - 1824", false),
                    new Choice("1821 - 1825", true),
                    new Choice("1821 - 1826", false)
                }, level.medium, questionType.perjuangan_kp),
          };

                hardQuestion = new List<Question>
                {
               new Question("Pasukan", "Apa nama wilayah yang berhasil diduduki oleh Belanda karena bantuan kaum Adat?",
                new Choice[]
               {
                    new Choice("Sipinang", false),
                    new Choice("Soli Air", false),
                    new Choice("Simawang", true)
               }, level.hard, questionType.latarBelakang_padri),

               new Question("Pasukan", "Apa yang dilakukan kaum Padri pada September 1821?",
                new Choice[]
               {
                    new Choice("Bekerja sama dengan Belanda untuk mengalahkan kaum Adat", false),
                    new Choice("Berunding dengan Belanda dan membuat perjanjian", false),
                    new Choice("Menyerang pos-pos patroli Belanda di berbagai tempat", true)
               }, level.hard, questionType.perjuangan_kp),

               new Question("Pasukan", "Berapa pasukan pribumi yang dipersiapkan oleh Belanda untuk melawan serangan Tuanku Lintau?",
                new Choice[]
               {
                    new Choice("30.000 pasukan orang pribumi termasuk kaum Adat", false),
                    new Choice("20.000 pasukan orang pribumi termasuk kaum Adat", false),
                    new Choice("10.000 pasukan pribumi termasuk kaum Adat", true)
               }, level.hard, questionType.perjuangan_tl),

               new Question("Pasukan", "Apa yang terjadi setelah perjanjian Masang ya?",
                new Choice[]
               {
                    new Choice("Belanda menepati perjanjiannya", false),
                    new Choice("Belanda tidak bertindak apa-apa", false),
                    new Choice("Belanda memanfaatkan perdamaian untuk menduduki daerah lain", true)
               }, level.hard, questionType.perjuangan_kp),

               new Question("Pasukan", "Apa akibat dari ditangkapnya Tuanku Mensiangan?",
                new Choice[]
               {
                    new Choice("Kaum Padri meminta Belanda untuk menepati janjinya", false),
                    new Choice("Kaum Padri membantu Tuanku Mensiangan kabur dari Belanda", false),
                    new Choice("Kaum Padri menyatakan pembatalan kesepakatan dalam Perjanjian Masang", true)
               }, level.hard, questionType.perjuangan_kp),

               new Question("Pasukan", "Apa yang terjadi pada tanggal 10 Februari 1821?",
                new Choice[]
               {
                    new Choice("Tuanku Lintau melakukan penyerangan pada Belanda", false),
                    new Choice("Perjanjian persahabatan antara Belanda dan Kaum Adat", true),
                    new Choice("Perjanjian damai antara Belanda dan Kaum Padri", false)
               }, level.hard, questionType.latarBelakang_padri),

               new Question("Pasukan", "Pada September 1822 kaum Padri berhasil mengusir Belanda dari...",
                new Choice[]
               {
                    new Choice("Sungai Puar, Duduk Sigandang, dan Tajong Alam", false),
                    new Choice("Sungai Puar, Guguk Sigandang, dan Tajong Alam", true),
                    new Choice("Sungai Puar, Duduk Sigandang, dan Tanjung Alam", false)
               }, level.hard, questionType.perjuangan_kp),

               new Question("Pasukan", "Apa yang terjadi di Kapau pada tahun 1823?",
                new Choice[]
               {
                    new Choice("Pasukan Padri mengalahkan Pasukan Kaum Adat", false),
                    new Choice("Pasukan Padri berhasil mengalahkan tentara Belanda", true),
                    new Choice("Tentara Belanda berhasil mengalahkan pasukan Padri", false)
               }, level.hard, questionType.perjuangan_kp),
          };
            }

          else
          {
               easyQuestion = new List<Question>
               {
               new Question("Yudha", "Imam Bonjol dibawa ke Batavia dan akhirnya dibuang ke...",
               new Choice[]
               {
                    new Choice("Cirebon", false),
                    new Choice("Cimahi", false),
                    new Choice("Cianjur", true)
               }, level.easy, questionType.akhirPerang_padri),

               new Question("Yudha", "Setelah bersatu, akhirnya kaum Adat dan Kaum Padri memutuskan untuk...",
               new Choice[]
               {
                    new Choice("Bergerak dan menyerang pos-pos tentara Belanda", true),
                    new Choice("Menyerang pasukan Belanda yang datang ke tempat mereka", false),
                    new Choice("Melakukan perang gerilya", false)
               }, level.easy, questionType.faseKetiga_padri),

               new Question("Yudha", "Belanda pada fase ketiga, ingin menguasai suatu benteng yang ada di Lintau yang bernama...",
               new Choice[]
               {
                    new Choice("Benteng Mataram", false),
                    new Choice("Benteng Marapalam", true),
                    new Choice("Benteng Malapalam", false)
               }, level.easy, questionType.faseKetiga_padri),

               new Question("Yudha", "Pelajaran apa yang bisa dipetik dari peristiwa ini adalah...",
               new Choice[]
               {
                    new Choice("Pantang menyerah meski terpojok", true),
                    new Choice("Merencanakan sesuatu dengan lebih matang", true),
                    new Choice("Belajar dari masa lalu dan tidak mengulangi kesalahan yang sama", true)
               }, level.easy, questionType.akhirPerang_padri),

               new Question("Yudha", "Belanda membuat seseorang membujuk para pemuka kaum Padri untuk berdamai, yaitu...",
               new Choice[]
               {
                    new Choice("Rafaiman Ajufri", false),
                    new Choice("Alaiman Ajufri", false),
                    new Choice("Sulaiman Aljufri", true)
               }, level.easy, questionType.faseKedua_padri),

               new Question("Yudha", "Pada tahun 1831, Gillavary digantikan oleh seseorang yang bernama...",
               new Choice[]
               {
                    new Choice("Jacob Elout", true),
                    new Choice("Jacky Elout", false),
                    new Choice("James Elout", false)
               }, level.easy, questionType.faseKetiga_padri),

               new Question("Yudha", "Pada tahun 1832 Belanda semakin ofensif pada kaum Padri karena...",
               new Choice[]
               {
                    new Choice("Datangnya bantuan dari Belanda", false),
                    new Choice("Datangnya bantuan pasukan dari Jawa", true),
                    new Choice("Datangnya bantuan dari Inggris", false)
               }, level.easy, questionType.faseKetiga_padri),

               new Question("Yudha", "Sebelum diasingkan, Imam Bonjol sempat dibawa ke tempat kekuasaan Belanda di...",
               new Choice[]
               {
                    new Choice("Batavia", true),
                    new Choice("Bandung", false),
                    new Choice("Jepara", false)
               }, level.easy, questionType.akhirPerang_padri),

               new Question("Yudha", "Benteng pertahanan terakhir Kaum Padri bernama...",
               new Choice[]
               {
                    new Choice("Benteng Lintau", false),
                    new Choice("Benteng Ellout", false),
                    new Choice("Benteng Bonjol", true)
               }, level.easy, questionType.faseKetiga_padri),

               new Question("Yudha", "Nama pengganti dari Jacob Elout yang menyerang pos-pos pertahanan Padri adalah",
               new Choice[]
               {
                    new Choice("Frank", false),
                    new Choice("Francis", true),
                    new Choice("Franco", false)
               }, level.easy, questionType.faseKetiga_padri),

          };

               mediumQuestion = new List<Question>
               {
               new Question("Yudha", "Belanda akhirnya merasa tersudut dengan kaum Adat dan Padri yang bersatu sehingga mereka membuat janji damai dalam bentuk...",
               new Choice[]
               {
                    new Choice("Plakat Padang", false),
                    new Choice("Plakat Padri", false),
                    new Choice("Plakat Panjang", true)
                }, level.medium, questionType.faseKetiga_padri),

               new Question("Yudha", "Pada fase ketiga perang padri, pertahanan terakhir kaum Padri berada di tangan...",
               new Choice[]
               {
                    new Choice("Tuanku Nan Renceh", false),
                    new Choice("Tuanku Pasaman", false),
                    new Choice("Tuanku Imam Bonjol", true)
                }, level.medium, questionType.faseKetiga_padri),

               new Question("Yudha", "Perjanjian Padang terjadi pada...",
               new Choice[]
               {
                    new Choice("15 Oktober 1825", false),
                    new Choice("15 Desember 1825", false),
                    new Choice("15 November 1825", true)
                }, level.medium, questionType.faseKedua_padri),

               new Question("Yudha", "Apa yang Belanda lakukan di fase kedua perang Padri?",
               new Choice[]
               {
                    new Choice("Belanda ingin berperang melawan kaum Padri", false),
                    new Choice("Belanda ingin mengadakan perjanjian damai dengan kaum Padri", true),
                    new Choice("Belanda ingin memutus kerjasamanya dengan kaum Adat", false)
                }, level.medium, questionType.faseKedua_padri),

               new Question("Yudha", "Pemimpin pasukan Belanda yang menyerang nagari di Ampek Angkek adalah...",
               new Choice[]
               {
                    new Choice("Gillavary", false),
                    new Choice("Elout", false),
                    new Choice("Francis", true)
                }, level.medium, questionType.faseKetiga_padri),

               new Question("Yudha", "Apa syarat yang diajukan Imam Bonjol ketika Belanda mengajukan perjanjian damai dengannya?",
               new Choice[]
               {
                    new Choice("Agar Belanda pergi dari Nusantara", false),
                    new Choice("Agar rakyatnya dibebaskan", true),
                    new Choice("Agar Belanda tidak mendukung kaum Adat", false)
                }, level.medium, questionType.faseKedua_padri),
          
               new Question("Yudha", "Pemimpin pasukan Belanda yang menyerang nagari di Ampek Angkek adalah...",
               new Choice[]
               {
                    new Choice("Ampu Angkuk", false),
                    new Choice("Nagaria", false),
                    new Choice("Ampek Angkek", true)
                }, level.medium, questionType.faseKetiga_padri),

               new Question("Yudha", "Siasat apa yang dipakai Francis untuk menangkap Imam Bonjol yang lolos dari pengepungan?",
               new Choice[]
               {
                    new Choice("Tipu muslihat menawarkan perjanjian damai", true),
                    new Choice("Menyerang Benteng Bonjol", false),
                    new Choice("Mengancam keselamatan warganya", false)
                }, level.medium, questionType.akhirPerang_padri),

          };

                hardQuestion = new List<Question>
                {
               new Question("Yudha", "Benteng di perbukitan dekat Bonjol jatuh ke tangan Belanda pada bulan...",
               new Choice[]
               {
                    new Choice("Juni 1836", false),
                    new Choice("Juli 1837", false),
                    new Choice("Agustus 1835", true)
               }, level.hard, questionType.faseKetiga_padri),

               new Question("Yudha", "Kota tempat Belanda menyerang Kaum Padri bernama...",
               new Choice[]
               {
                    new Choice("Koto Tuo", true),
                    new Choice("Kota Tua", false),
                    new Choice("Minangkabau", false)
               }, level.hard, questionType.faseKetiga_padri),

               new Question("Yudha", "Akhirnya pada tanggal 16 Agustus 1837, Benteng Bonjol..",
               new Choice[]
               {
                    new Choice("Berhasil dipertahanakan dari ancaman Belanda", false),
                    new Choice("Dikepung dari empat penjuru dan dilumpuhkan", true),
                    new Choice("Ditinggalkan oleh Belanda", false)
               }, level.hard, questionType.faseKetiga_padri),

               new Question("Yudha", "Yang mana yang bukan isi dari Perjanjian Padang? ",
               new Choice[]
               {
                    new Choice("Praktik adu ayam akan terus dilanjutkan", true),
                    new Choice("Belanda mengakui kekuasaan pemimpin Padri", false),
                    new Choice("Belanda dan Kaum Padri akan melindungi para pedagang dan orang-orang dalam perjalanan", false)
               }, level.hard, questionType.faseKedua_padri),

               new Question("Yudha", "Berikut ini merupakan tempat yang diakui sebagai kekuasaan pemimpin padri dalam perjanjian padang adalah...",
               new Choice[]
               {
                    new Choice("Sigandang", true),
                    new Choice("Sigidung", false),
                    new Choice("Ciremai", false)
               }, level.hard, questionType.faseKedua_padri),

               new Question("Yudha", "Plakat Panjang adalah",
               new Choice[]
               {
                    new Choice("Janji khidmat simbol tidak akan ada lagi perang antara Belanda dan Kaum Padri", true),
                    new Choice("Janji khidmat simbol tidak akan ada lagi perang antara Kaum Adat dan Padri", false),
                    new Choice("Janji khidmat simbol tidak akan ada lagi sambung ayam", false)
               }, level.hard, questionType.faseKetiga_padri),
          };

            }    
      }

      if(currentGoal is JudgementGoal jd_goal)
      {
          if(jd_goal.getMain() == true)
          {
               switch(slot.understandingLevel)
                {
               case 0:
               {
                    randomQuestion(5, easyQuestion);
                    randomQuestion(5, mediumQuestion);
                    randomQuestion(5, hardQuestion);
                    break;
               }
              
               case 1:
               {
                    foreach(Question q in easyQuestion)
                    {
                         questionDataList.Add(q);
                    }
                    randomQuestion(5, mediumQuestion);
                    break;
               }
               case 2:
               {
                    randomQuestion(5, easyQuestion);
                    randomQuestion(8, mediumQuestion);
                    randomQuestion(2, hardQuestion);
                    break;
               }
               case 3:
               {
                    randomQuestion(3, easyQuestion);
                    randomQuestion(6, mediumQuestion);
                    randomQuestion(6, hardQuestion);
                    break;
               }
               default: break;
               }
          }

          else
          {
               switch(slot.understandingLevel)
               {
                case 1:
               randomQuestion(3, easyQuestion);
               randomQuestion(2, mediumQuestion);
               break;
               case 2:
               randomQuestion(1, easyQuestion);
               randomQuestion(3, mediumQuestion);
               randomQuestion(1, hardQuestion);
               break;
               default: break;
               }
          }
          
      }

   }

   private void randomQuestion(int num, List<Question> question)
   {
          List<Question> toBeRolled = new List<Question>(question);
          for(int i=0; i<num;i++)
          {
               int index = -1;
               index = Random.Range(0, toBeRolled.Count);
               questionDataList.Add(toBeRolled[index]);
               toBeRolled.RemoveAt(index);
          }
   }


    private int evaluateResult(float score)
    {
         
        goodValue = good.Evaluate(score);
        decentValue = decent.Evaluate(score);
        poorValue = poor.Evaluate(score);

         if(currentGoal is JudgementGoal jdg_goal)
          {
               if(jdg_goal.getMain() == true)
               {
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
               
               else
               {
                    int u_level = 0;

                    if(goodValue > decentValue && goodValue > poorValue)
                    {
                         u_level = 3;
                    }

                     else if(decentValue >= goodValue && decentValue > poorValue )
                    {
                         u_level = 2;
                    }

                     else if(poorValue >= decentValue)
                    {
                         u_level = 1;
                    }

                    return u_level;

               }
          }

          return 0;

       
    }

     private void assignReview()
    {
        List<Review> reviews = new List<Review>
        {
           new Review(new Story("Latar Belakang Serangan Sultan Agung", new List<Dialogs>
               {
                    new NPCDialog("Review", "Pada awalnya hubungan Mataram dan Belanda cukup baik", null),
                    new NPCDialog("Review", "Sampai pada tanggal 18 November 1618, jendral VOC saat itu, Jan Pieterszoon Coen", null),
                    new NPCDialog("Review", "Meminta Van den Marct untuk menyerang Jepara, menyebabkan kerugian yang besar bagi Mataram", null),
                    new NPCDialog("Review", "Sejak saat itu hubungan Belanda dan Mataram memburuk", null),
                    new NPCDialog("Review", "Hal ini juga dikarenakan keberadaan Belanda di tanah jawa menghalangi cita-cita dari Sultan Agung", null),
                    new NPCDialog("Review", "Ia bercita-cita untuk menyatukan tanah jawa dan mengusir kekuasaan bangsa asing dari tanah jawa", null),
                    new NPCDialog("Review", "Jadi apa yang mau kamu pelajari selanjutnya?", null)
               }, false, null), questionType.latarBelakang_sa),
          new Review(new Story("Serangan Pertama Sultan Agung", new List<Dialogs>
               {
                    new NPCDialog("Review", "Dan akhirnya pada Agustus 1628, Mataram melancarkan serangan ke Batavia", null),
                    new NPCDialog("Review", "Pasukan Tumenggung Baureksa sampai ke Batavia terlebih dahulu dan mulai melakukan serangan", null),
                    new NPCDialog("Review", "Selanjutnya pada Oktober 1628, menyusul pasukan dari Tumenggung Sura Agul-Agul, Kiai Dipati Mandurojo dan Upa Santa", null),
                    new NPCDialog("Review", "Setelah perang yang cukup lama, Mataram mengalami kekalahan karena kekurangan perbekalan", null),
                    new NPCDialog("Review", "Juga karena persenjataan Belanda lebih modern", null),
                    new NPCDialog("Review", "Sehingga pada 6 Desember 1628, pasukan Mataram mundur", null),
                    new NPCDialog("Review", "Jadi apa yang mau kamu pelajari selanjutnya?", null)
               }, false, null), questionType.seranganSatu_sa),
          new Review(new Story("Serangan Kedua Sultan Agung", new List<Dialogs>
               {
                    new NPCDialog("Review", "Setelah kekalahan di serangan pertama, Mataram mulai mempersiapkan lebih matang untuk serangan kedua", null),
                    new NPCDialog("Review", "Sebelum penyerangan untuk menyebabkan perbekalan lebih, mereka membangun lumbung di Priyangan dan Cirebon", null),
                    new NPCDialog("Review", "Pada 1629, mereka pun melancarkan serangan lagi ke Batavia", null),
                    new NPCDialog("Review", "Akan tetapi karena adanya wabah kolera dan malaria", null),
                    new NPCDialog("Review", "Juga karena Belanda menghancurkan lumbung yang mereka bangun", null),
                    new NPCDialog("Review", "Akhirnya serangan ini juga mengalami kegagalan", null),
                    new NPCDialog("Review", "Jadi apa yang mau kamu pelajari selanjutnya?", null)
               }, false, null), questionType.seranganDua_sa),
          new Review(new Story("Akhir Serangan Sultan Agung", new List<Dialogs>
               {
                    new NPCDialog("Review", "Namun setelah 2 kegagalan pun Sultan Agung tidak menyerah untuk menyerang Batavia dan mengusir VOC", null),
                    new NPCDialog("Review", "Sayangnya sepeninggal Sultan Agung di tahun 1945, Mataram mengalami kemunduran", null),
                    new NPCDialog("Review", "Dan ini membuka peluang untuk Belanda menguasai Mataram", null),
                    new NPCDialog("Review", "Jadi apa yang mau kamu pelajari selanjutnya?", null)
               }, false, null), questionType.akhir_sa),
          new Review(new Story("Latar Belakang Perang Padri", new List<Dialogs>
               {
                    new NPCDialog("Pasukan", "Di dalam Minangkabau, terdapat dua kaum.", null),
                    new NPCDialog("Pasukan", "Kaum Padri dan Kaum Adat", null),
                    new NPCDialog("Pasukan", "Kaum Padri dan Adat saling bertentangan karena perbedaan praktik keagamaan sehingga munculah bentrokan.", null),
                    new NPCDialog("Pasukan", "James Du Puy datang pada tahun 1821 dan membuat perjanjian persahabatan dengan kaum Adat.", null),
                    new NPCDialog("Pasukan", "Perjanjian tersebut terjadi pada tanggal 10 Februari 1821.", null),
                    new NPCDialog("Pasukan", "Setelah perjanjian tersebut, Belanda pun menduduki Simawang.", null),
                    new NPCDialog("Pasukan", "Hal ini membuat kaum Padri marah sehingga kaum Padri menentang sikap tersebut dan perang pun dimulai pada tahun 1821 juga.", null),
               }, false, null), questionType.latarBelakang_padri),
          new Review(new Story("Perjuangan Tuanku Lintau", new List<Dialogs>
               {
                    new NPCDialog("Pasukan", "Pada September 1821, kaum Padri mulai menyerang pos-pos patroli Belanda di berbagai tempat.", null),
                    new NPCDialog("Pasukan", "Tuanku Lintau menggerakan 20.000 sampai 25.000 pasukan untuk menyerang Belanda.", null),
                    new NPCDialog("Pasukan", "Kaum Padri menggunakan senjata-senjata tradisional, seperti tombak dan parang.", null),
                    new NPCDialog("Pasukan", "James Du Puy datang pada tahun 1821 dan membuat perjanjian persahabatan dengan kaum Adat.", null),
                    new NPCDialog("Pasukan", "Sedangkan, Belanda menyiapkan 200 orang serdadu Eropa dan 10.000 pasukan pribumi termasuk kaum Adat.", null),
                    new NPCDialog("Pasukan", "Akibat dari serangan tersebut, Tuanku Lintau mundur ke Lintau.", null),
                    new NPCDialog("Pasukan", "Tuanku Lintau juga kehilangan 350 orang pasukan, termasuk putranya.", null),
               }, false, null), questionType.perjuangan_tl),
          new Review(new Story("LPerjuangan Kaum Padri dan Perjanjian Masang", new List<Dialogs>
               {
                    new NPCDialog("Pasukan", "Pada periode 1821-1825, serangan-serangan kaum Padri meluas di seluruh Minangkabau.", null),
                    new NPCDialog("Pasukan", "Pada September 1822, kaum Padri berhasil mengusir Belanda dari Sungai Puar, Guguk Sigandang, dan Tajong Alam.", null),
                    new NPCDialog("Pasukan", "Pada tahun 1823, pasukan Padri berhasil mengalahkan tentara Belanda di Kapau.", null),
                    new NPCDialog("Pasukan", "Pada tahun 1824, Belanda merasa terdesak sehingga mereka membuat perjanjian damai yang bernama Perjanjian Masang.", null),
                    new NPCDialog("Pasukan", "Sayangnya, Belanda memanfaatkan perjanjian tersebut untuk menduduki daerah-daerah lain.", null),
                    new NPCDialog("Pasukan", "Tuanku Imam Bonjol pun setuju dengan perjanjian ini, namun Tuanku Mensiangan menolak dan melawan Belanda.", null),
                    new NPCDialog("Pasukan", "Akhirnya pusat pertahanannya dibakar dan dia pun ditangkap.", null),
                    new NPCDialog("Pasukan", "Hal ini membuat Kaum Padri menyatakan pembatalan kesepakatan dalam Perjanjian Masang dan kembali melawan Belanda.", null),
               }, false, null), questionType.perjuangan_kp),
          new Review(new Story("Fase Kedua Perang Padri", new List<Dialogs>
               {
                    new MainCharacterDialog(false, characterExpression.neutral, "Belanda ingin mengadakan perjanjian damai dengan kaum Padri.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Tetapi tidak ada yang menghiraukannya sehingga Belanda mengutus Sulaiman Aljufri untuk mengajak para pemuka kaum Padri untuk berdamai.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Tuanku Imam Bonjol tidak setuju, tetapi Tuanku Lintau dan Tuanku Nan Renceh setuju.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Sehingga akhirnya terjadilah Perjanjian Padang pada tanggal 15 November 1825.", null),
               }, false, null), questionType.faseKedua_padri),
          new Review(new Story("Fase Ketiga Perang Padri", new List<Dialogs>
               {
                    new MainCharacterDialog(false, characterExpression.neutral, "Kaum Padri mulai mendapatkan simpati dari kaum Adat sehingga mereka memutuskan untuk bergerak dan menyerang pos-pos tentara Belanda.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Tapi, tindakan ini membuat Belanda menyerang Koto Tuo di Ampek Angkek, yang pasukannya dipimpin oleh Gillavary.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Pada tahun 1831, Gillavary digantikan oleh Jacob Elout.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Dia segera mengerahkan pasukannya untuk menguasai Manggung, Naras, dan Batipuh.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Setelah itu, mereka ingin menguasai Benteng Marapalam yang ada di Lintau.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Akibat dua orang kaum Padri yang berkhianat, maka pada Agustus 1831 Belanda dapat menguasai Benteng Marapalam.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Pada tahun 1832, datangnya bantuan pasukan dari Jawa membuat Belanda semakin ofensif pada kaum Padri.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Lalu, Elout digantikan oleh Francis.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Belanda akhirnya merasa tersudut dengan kaum Adat dan Padri yang bersatu sehingga mereka membuat janji khidmat bernama Plakat Panjang.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Beberapa tokoh memenuhi ajakan berdamai, sementara yang tidak setuju masih melanjutkan perlawanan.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Setelah itu, pertahanan terakhir kaum Padri berada di tangan Tuanku Imam Bonjol.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Setelah itu, benteng di perbukitan dekat Bonjol jatuh ke tangan Belanda pada bulan Agustus 1835.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Kemudian, Belanda mencoba berdamai dengan Imam Bonjol.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Tetapi, Imam Bonjol ingin berdamai jika rakyat Bonjol dibebaskan dari kerja paksa dan nagarinya tidak diduduki Belanda.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Sehingga, Belanda tidak memberi jawaban dan malah terus mengepung pertahanan Bonjol.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Benteng Bonjol tetap dipertahankan sampai tahun 1836.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Akhirnya pada tanggal 16 Agustus 1837, Benteng Bonjol dikepung dari empat penjuru dan dilumpuhkan.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Maka dari itu, Imam Bonjol akhirnya menerima tawaran damai dari Francis tetapi itu hanyalah tipu muslihat dan Tuanku Imam Bonjol langsung ditangkap di tempat perundingan.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Beberapa pengikutnya berhasil meloloskan diri dan melanjutkan perang gerilya.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Imam Bonjol dibawa ke Batavia dan akhirnya dibuang ke Cianjur.", null),
               }, false, null), questionType.faseKetiga_padri),
          new Review(new Story("Akhir dari Perang Padri", new List<Dialogs>
                { 
                    new MainCharacterDialog(false, characterExpression.neutral, "Akhirnya pada tanggal 16 Agustus 1837, Benteng Bonjol dikepung dari empat penjuru dan dilumpuhkan.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Maka dari itu, Imam Bonjol akhirnya menerima tawaran damai dari Francis tetapi itu hanyalah tipu muslihat dan Tuanku Imam Bonjol langsung ditangkap di tempat perundingan.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Beberapa pengikutnya berhasil meloloskan diri dan melanjutkan perang gerilya.", null),
                    new MainCharacterDialog(false, characterExpression.neutral, "Imam Bonjol dibawa ke Batavia dan akhirnya dibuang ke Cianjur.", null),
               }, false, null), questionType.akhirPerang_padri),
        };

          toBeReviewed = new List<Review>();

           for(int j=0;j<needReviewType.Length;j++)
           {

               foreach(Review review in reviews)
               {
                    if(review.getReviewType() == needReviewType[j])
                    {
                         Story rev = review.getReviewContent();
                         reviewTexts[j].text = rev.getTitle();
                         Debug.Log(review);
                         toBeReviewed.Add(review);
                    }
               }

          }
           


        reviewOverlay.SetActive(true);
        reviewStoryOverlay.SetActive(true);
        gameOverlay.SetActive(false);
        reviewText.text = "Jadi yang mana yang ingin kamu pelajari terlebih dahulu?";
        reviewCount = toBeReviewed.Count;

        for(int i=0; i<reviewChoice.Length;i++)
        {
          if(i >= toBeReviewed.Count)
          {
               reviewChoice[i].SetActive(false);
          } 
        }

    }

    public void OnChooseReview(int number)
    {
        reviewOverlay.SetActive(false);
        reviewChoice[number].SetActive(false);
        reviewCount -= 1;
        reviewText.text = toBeReviewed[number].getReviewContent().getTitle();
        choosenReview = number;
    }

    public void checkReview()
    {
        if(reviewCount == 0)
        {
            ReviewGoal goal = currentGoal as ReviewGoal;
            reviewOverlay.SetActive(false);
            reviewStoryOverlay.SetActive(false);
            goal.OnAllReviewDone();
        }
    }

    public void nextReview()
    {
        
        Story rev = toBeReviewed[choosenReview].getReviewContent();
        reviewText.text = "";

          if (typeNextCoroutine != null)
         {
            StopCoroutine(typeNextCoroutine);
         }

         if(reviewLineCount < rev.dialogs.Count)
         {     
               typeNextCoroutine = StartCoroutine(TypeLine(rev.dialogs[reviewLineCount].getDialog()));
               rev.dialogs[reviewLineCount].showLine();
               reviewLineCount++;
         }
        
        rev.checkDialogs();
        OnCheckReview(rev);

    }

    public void OnCheckReview(Story story)
    {
          if(story.getCompleted())
          {
               reviewLineCount = 0;
               reviewOverlay.SetActive(true);
               checkReview();
          }
    }


    IEnumerator TypeLine(string line)
    {
        AudioManager.instance.Play("Typewritter");
        foreach( char c in line.ToCharArray())
        {
            reviewText.text += c;
            yield return new WaitForSeconds(0.01f);
        }
        AudioManager.instance.Stop("Typewritter");
    }


    IEnumerator showGetNotification()
    {
          getItemNotification.SetActive(true);
          yield return new WaitForSeconds(1f);
          getItemNotification.SetActive(false);
    }

    


     public List<Mission> getChapterMissions(int chapter)
    {
        List<Mission> mission;
        if(chapter == 1)
        {
            mission = new List<Mission>
            {
                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke Kota Terdekat untuk Mencari Informasi", 1, new string[] {"Sultan Agung"}, new int[] {2}, new Story[] {
                        new Story("Kamu bertemu dengan seseorang didekat gerbang kota...", new List<Dialogs>
                        {
                            new NPCDialog("???", "Hei, kamu!", null),
                            new MainCharacterDialog(true, characterExpression.shook, "(Eh? Apa orang itu berbicara padaku?)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Aneh...jangan-jangan aku benar-benar terlempar ke masa lalu)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Tapi sebaiknya aku menanggapinya agar tidak tampak mencurigakan)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Iya...pak?", null),
                            new NPCDialog("???", "Hahaha, baru kali ini saya dipanggil pak oleh warga saya", null),
                            new NPCDialog("???", "Atau jangan-jangan kamu orang baru disini", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Bisa...dibilang begitu?", null),
                            new NPCDialog("???", "Oh kalau begitu, selamat datang di Mataram!", null),
                            new NPCDialog("???", "Saya harap saya dapat menjamu tamu seperti kamu dengan lebih baik", null),
                            new NPCDialog("???", "Tapi kami ditengah situasi cukup genting", null),
                            new MainCharacterDialog(true, characterExpression.think, "Situasi cukup genting?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Oh iya, tapi saya belum tau siapa bapak", null),
                            new NPCDialog("???", "Ah iya benar, saya belum memperkenalkan diri", null),
                            new NPCDialog("Sultan Agung", "Saya Sultan Agung, raja dari Kerajaan Mataram ini", null),
                            new MainCharacterDialog(true, characterExpression.shook, "(Raja?!)", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Aduh, maafkan ketidaksopanan saya, paduka raja", null),
                            new NPCDialog("Sultan Agung", "Sudah-sudah tidak perlu meminta maaf, dan panggil saya sultan saja ya", null),
                            new NPCDialog("Sultan Agung", "Dan sekali lagi saya meminta maaf karena saya tidak menjamu dek...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Player, Sultan", null),
                            new NPCDialog("Sultan Agung", "Ah iya, dek Player dengan lebih baik, karena kami sedang merencanakan sebuah perang", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Merencanakan perang? Dengan siapa?", null),
                            new NPCDialog("Sultan Agung", "Oh tentu dengan orang-orang Belanda menyebalkan itu...", null),
                            new NPCDialog("Sultan Agung", "Siapa nama mereka...ah iya, VOC, terutama pimpinan mereka itu, J.P. Coen", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Aku sepertinya pernah mendengar tentang mereka sebelumnya di sekolah...", null),
                            new NPCDialog("Sultan Agung", "Saya tidak tau ada sekolah yang mengajarkan tentang itu", null),
                            new NPCDialog("Sultan Agung", "Tapi intinya orang-orang di VOC itu ingin sekali menguasai perdagangan di pulau Jawa ini", null),
                            new NPCDialog("Sultan Agung", "Dan itu membawa penderitaan bagi rakyat disini", null),
                            new NPCDialog("Sultan Agung", "Lagi, pekerjaan mereka itu juga menghalangi saya menggapai cita-cita saya", null),
                            new NPCDialog("Sultan Agung", "Untuk menyatukan seluruh tanah jawa dan menghilangkan pengaruh mereka dari tanah ini", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Tapi apa yang mereka lakukan sampai Sultan merencanakan untuk berperang dengan mereka...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "...dan tidak menempuh jalur damai terlebih dahulu? ", null),
                            new NPCDialog("Sultan Agung", "Orang-orang Belanda itu tidak bisa dipercaya sama sekali, dek Player", null),
                            new NPCDialog("Sultan Agung", "Baru saat mereka mencoba menjalin hubungan baik dengan kerajaan saya...", null),
                            new NPCDialog("Sultan Agung", "Mereka malah menyerang kota Jepara 18 November tahun kemarin, 1618", null),
                            new NPCDialog("Sultan Agung", "Dan itu adalah ulah dari pemimpin mereka, si J.P Coen itu", null),
                            new NPCDialog("Sultan Agung", "Ia memerintahkan si Van der Marct itu untuk menyerang, dan membawa kerugian besar bagi kami", null),
                            new NPCDialog("Sultan Agung", "Itu semua karena mereka serakah dan gila kekuasaan", null),
                            new NPCDialog("Sultan Agung", "Karena itu lah, sudah saatnya Mataram membawa urusan ini ke Medan Perang", null),
                            new NPCDialog("Sultan Agung", "....sekarang kita tinggal melihat situasi disana", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Situasi di tempat anda akan menyerang, Sultan?", null),
                            new NPCDialog("Sultan Agung", " Iya, di pusat pemerintahan mereka di Batavia", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Tapi bukannya itu jauh sekali dari sini...pasukan anda akan menempuh perjalanan yang cukup panjang untuk itu...", null),
                            new NPCDialog("Sultan Agung", "Tidak apa-apa, semuanya akan dipersiapkan dengan baik, dek Player", null),
                            new NPCDialog("Sultan Agung", "Bicara soal itu...apa kamu ada rencana tertentu, dek Player?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Tidak sih untuk sekarang...", null),
                            new NPCDialog("Sultan Agung", "Kalau kamu tidak keberatan, saya ingin meminta bantuanmu untuk mengintai situasi di Batavia", null),
                            new NPCDialog("Sultan Agung", "Supaya saat serangan besok, kami sudah lebih siap dengan medan dan situasi yang ada", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah, tapi kemana arah menuju Batavia?", null),
                            new NPCDialog("Sultan Agung", "Ikuti saja jalur ini dan nanti kamu akan sampai ke Batavia", null),
                            new NPCDialog("Sultan Agung", "Saya akan menunggu kabar dari kamu", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Jadi Sultan Agung ini adalah raja Mataram pada masa pendudukan VOC)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Dan karena serangan VOC ke Jepara tanggal 18 November 1618 lalu, ia jadi akan melakukan serangan pada VOC)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Sebaiknya aku mencatat info ini, siapa tau aku memerlukannya nanti)", null),

                        }, false, null)}, true)}), 

                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi Mengintai ke Kota Batavia", 1, new string[] {"Batavia Fort Gate"}, new int[] {-1}, new Story[] {
                        new Story("Setelah perjalanan cukup panjang, kamu sampai di Batavia...", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.hurt, "Huftt...hufftt...", null),
            new MainCharacterDialog(true, characterExpression.hurt, " (Lelah sekali...untungnya jarak Batavia dan Mataram tidak sebegitu jauh seperti Jakarta dan Jawa Tengah di masaku...)", null),
            new MainCharacterDialog(true, characterExpression.think, "(Tapi apakah memang seharusnya begitu?)", null),
            new MainCharacterDialog(true, characterExpression.think, " (Memang ada sesuatu yang aneh tentang tempat ini...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Sebaiknya aku mencari petunjuk didalam benteng kota ini untuk aku laporkan kepada Sultan Agung)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Dan siapa tau juga aku bisa menemukan petunjuk tentang kemana mesin aneh tadi itu membawaku)", null),

        }, false, null)
                    }, true)
                }), 

                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Mencari petunjuk di Batavia", 3, new string[] {"Red Book", "Crates Box", "Belanda Soldier"}, new int[] {1, -1, -1}, new Story[] {
                        new Story("Kamu menemukan sebuah buku merah tergeletak di tanah...", new List<Dialogs>
        {
          new MainCharacterDialog(true, characterExpression.shook, "(Siapa yang menaruh file-file berserakan seperti ini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Tapi file apa ini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, " (Hmmm....)", null),
          new MainCharacterDialog(true, characterExpression.think, "(Eh kenapa ada info-info terkait J.P. Coen disini?)", null),
          new MainCharacterDialog(true, characterExpression.think, "(Mari kita lihat...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Hmmmm...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Jadi J.P. Coen memerintah dari 1618-1623 dan 1627-sekarang...aku asumsikan 1628...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Ada hal lain kah yang harus aku tau...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Hmmmm...dari data-data disini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Pengeluaran yang dikeluarkan VOC untuk membeli sebanyak ini hasil bumi sangat sedikit)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku tidak perlu belajar akuntansi untuk mengetahui ini aneh...)", null),
          new MainCharacterDialog(true, characterExpression.angry, "(Mereka pasti mengeksploitasi hasil bumi disini dan angkanya terus meningkat)", null),
          new MainCharacterDialog(true, characterExpression.angry, "(Memang serakah orang-orang itu, tapi sebaiknya aku menyimpan info ini untuk aku review kembali nantinya)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku sebaiknya cepat menyelesaikan urusanku disini dan kembali ke Sultan Agung agar tidak dicurigai karena mengambil data ini)", new string[]{"Red Book"}),
    

        }, false, null),
                        new Story("Diujung kota terdapat banyak kotak-kotak barang yang tertata rapi...", new List<Dialogs>
        {
          new MainCharacterDialog(true, characterExpression.neutral, "(Kotak-kotak ini semua hasil bumi...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Tapi terutama rempah-rempah)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Disini dituliskan semuanya akan dikirimkan ke VOC)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Jadi mereka memang menguasai seluruh jual beli disini ya...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Atau itu hasil dari monopoli mereka...)", null),
          new MainCharacterDialog(true, characterExpression.hurt, "(Hmmm...pandangan orang-orang di sini tidak enak sekali padaku, tapi apa hanya itu saja petunjuk yang bisa aku dapat?)", null),

        }, false, null),
                        new Story("Kamu memperhatikan penjaga yang berjaga di barak secara seksama...", new List<Dialogs>
        {
          new MainCharacterDialog(true, characterExpression.shook, "(Penjaga-penjaga itu...)", null),
          new MainCharacterDialog(true, characterExpression.shook, "(Sepertinya mereka membawa senapan yang cukup modern...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku tidak tau senjata semacam itu sudah ada di masa ini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Ini bisa jadi berbahaya jika Sultan Agung tidak mempersiapkan senjata yang setara)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku sebaiknya memberitahunya secepatnya)", null),
        }
, false, null)
                    }, false)
                }), 

                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Kembali ke Sultan Agung", 1, new string[] {"Sultan Agung"}, new int[] {-1}, new Story[] {
                        new Story("Setelah perjalanan panjang kembali kemudian...", new List<Dialogs>
        {
         new MainCharacterDialog(true, characterExpression.neutral, "Sultan, aku kembali!", null),
         new NPCDialog("Sultan Agung", "Ah, kamu sudah kembali, dek Player", null),
         new NPCDialog("Sultan Agung", "Jadi bagaimana hasil pengintaianmu?", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Semuanya sepertinya normal disana", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Dan sepertinya VOC sudah menguasai perdagangan disana karena aku menemukan banyak kotak dan dokumen...", null),
         new MainCharacterDialog(true, characterExpression.neutral, "...yang menunjukan demikian, mereka memonopoli semuanya disana", null),
         new NPCDialog("Sultan Agung", "Hmph, orang-orang serakah itu", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Aku juga melihat kalau para penjaga disana dibekali senapan yang cukup modern", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Aku penasaran...senjata apa yang anda siapkan untuk perang, Sultan?", null),
         new NPCDialog("Sultan Agung", "Senjata perang pada umumnya, bambu runcing dan lainnya", null),
         new NPCDialog("Sultan Agung", "Kita juga memiliki cukup senapan, jadi aku rasa kita sudah cukup siap", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Baiklah...kalau kau bilang begitu", null),
         new MainCharacterDialog(true, characterExpression.neutral, "(Aku tidak yakin itu cukup tapi ya sudahlah...)", null),
         new NPCDialog("Sultan Agung", "Tapi memang ada baiknya kami mulai mempersiakan perbekalan dengan baik...", null),
         new NPCDialog("Sultan Agung", "Agar tidak dimanfaatkan musuh kalau kami sampai kekurangan perbekalan", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Apakah ada yang bisa aku bantu terkait itu Sultan?", null),
         new NPCDialog("Sultan Agung", "Kamu ini baik sekali, dek Player ", null),
         new NPCDialog("Sultan Agung", "Terkait itu, kamu bisa temui pemimpin pasukan saya, Tumenggung Baurekhsa", null),
         new NPCDialog("Sultan Agung", "Seharusnya dia sekarang ada di gudang perbekalan di dekat Pelabuhan Jepara", null),
         new NPCDialog("Sultan Agung", "Coba kamu kesana dan nanti ikuti saja instruksi darinya", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Baik, Sultan!", null),
        }, false, null)
                    }, true)
                }), 

                 new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke gudang perbekalan pasukan Mataram", 1, new string[] {"Tumenggung Baurekhsa"}, new int[] {-1}, new Story[] {
                        new Story("Sesampainya di gudang perbekalan...", new List<Dialogs>
        {
            new NPCDialog("???", "Hmmm...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Permisi pak, apakah anda Tumenggung Baurekhsa?", null),
            new NPCDialog("T.Baurekhsa", " Iya saya sendiri, adek ini siapa", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Saya Player, saya dikirim Sultan Agung untuk membantu persiapan pasukan disini", null),
            new NPCDialog("T.Baurekhsa", "Begitu ya, tapi kami baru saja selesai mempersiapkan semuanya sebenarnya", null),
            new NPCDialog("T.Baurekhsa", "Jadi saya tidak yakin kamu bisa membantu apa lagi disini, dek Player", null),
            new NPCDialog("T.Baurekhsa", "Sebentar ya, saya sedang mengecek daftar perbekalan yang ada", null),
            new NPCDialog("T.Baurekhsa", "Hmmmm....", null),
            new NPCDialog("T.Baurekhsa", " ....", null),
            new NPCDialog("T.Baurekhsa", " Hmmm..kok masih ada yang kurang...", null),
            new NPCDialog("T.Baurekhsa", "Padahal aku sudah meminta pasukan yang lainnya untuk mengecek ulang sebelum menutup gudang...", null),
            new NPCDialog("T.Baurekhsa", "Sekarang aku jadi harus membuka gudang lagi sekaligus mengambil perbekalan yang kurang", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Pak, apakah saya bisa membantu mengambilkan perbekalan-perbekalan tersebut?", null),
            new NPCDialog("T.Baurekhsa", "Kamu yakin kamu bisa? Barang-barang perbekalan itu berat, lho?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Tidak apa-apa, saya yakin saya bisa", null),
            new MainCharacterDialog(true, characterExpression.happy, "(Kan tinggal aku masukan ke fitur inventory canggih di smartphoneku dan aku bisa membawa barang-barang itu seakan-akan mereka microchip)", null),
            new NPCDialog("T.Baurekhsa", "Baiklah kalau kamu bersikeras", null),
            new NPCDialog("T.Baurekhsa", "Kami membutuhkan 150 ekor sapi, 5.900 karung gula, 25.000 buah kelapa dan 12.000 karung beras lagi", null),
            new MainCharacterDialog(true, characterExpression.shook, "Itu banyak sekali??", null),
            new NPCDialog("T.Baurekhsa", "Haha..itu jumlah total semuanya saja, dek Player", null),
            new NPCDialog("T.Baurekhsa", "Kami hanya kurang 3 karung gula, 2 buah kelapa, dan 3 karung beras lagi..", null),
            new NPCDialog("T.Baurekhsa", "Biasanya tumpukan karung gula dan beras ada di dekat rumah-rumah warga, sementara kelapa aku rasa kamu bisa menemukannya di dekat pohon-pohon di sekitar sini", null),
            new NPCDialog("T.Baurekhsa", "Aku tunggu disini saat kamu sudah mendapatkan semua barang itu", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Baik, pak", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah, saatnya bekerja..", null),
        }, false, null)
                    }, true)
                }), 

                 new Mission(new List<Goal>
                {
                    new GatherGoal("Mengambil 2 karung beras untuk perbekalan", 2, new string[] {"Rice Sack", "Rice Sack (1)"}, null, false),
                    new GatherGoal("Mengambil 3 karung gula untuk perbekalan", 3, new string[] {"Sugar sack", "Sugar sack (1)", "Sugar sack (2)"}, null, false),
                    new GatherGoal("Mengambil 2 buah kelapa untuk perbekalan", 2, new string[] {"Coconut nut", "Coconut nut (1)"}, null, false)
                }), 
                 new Mission(new List<Goal>
                {
                    new SubmitGoal("Berikan perbekalan ke Tumenggung Baurekhsa", 1, "Tumenggung Baurekhsa", new Story[]{
                        new Story("Kamu kembali ke Tumenggung Baurekhsa dengan membawa seluruh perbekalan yang dibutuhkan", new List<Dialogs>
        {
            new NPCDialog("T.Baurekhsa", "Oh, kamu sudah membawakan semua perbekalan yang kami perlukan", null),
            new NPCDialog("T.Baurekhsa", "Terimakasih, dek Player", null),
            new NPCDialog("T.Baurekhsa", "Kalau begitu kami sudah siap untuk melakukan serangan kepada Batavia ", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Apakah anda sudah membuat rencana terkait serangan tersebut?", null),
            new NPCDialog("T.Baurekhsa", "Kami akan membawa semua perbekalan ini menggunakan kapal", null),
            new NPCDialog("T.Baurekhsa", "Dan kami akan bertingkah seakan kami ingin berdagang, untuk menginfiltrasi benteng mereka", null), 
            new NPCDialog("T.Baurekhsa", "Setelah itu kami akan memulai serangan dan mulai mendatangkan pasukan-pasukan tambahan pada bulan-bulan ke depan", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Aku masih khawatir terkait persenjataan yang dimiliki pasukan mereka...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Tapi bukan urusanku juga, mereka sudah aku peringatkan)", null),
            new NPCDialog("T.Baurekhsa", "Baiklah, karena persiapan semuanya sudah beres, bisakah kamu sampaikan ini kepada Sultan?", null),
            new NPCDialog("T.Baurekhsa", "Beritahu padanya kami siap menyerang kapan pun ia perintahkan", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sepertinya aku akan kembali ke Mataram)", null),
            new NPCDialog("T.Baurekhsa", "Hmmm...tapi sempat tidak ya...", null),
            new MainCharacterDialog(true, characterExpression.think, "Kenapa tidak sempat? Aku saja berjalan dari Mataram ke Batavia hanya dalam hitungan menit kok!", null),
            new MainCharacterDialog(true, characterExpression.think, "(Sesuatu yang aneh sih...)", null),
            new NPCDialog("T.Baurekhsa", "ya, tapi waktu sepertinya berjalan secara berbeda untuk orang-orang seperti kamu, dek Player", null),
            new MainCharacterDialog(true, characterExpression.think, "Orang seperti aku?", null),
            new NPCDialog("T.Baurekhsa", ".......", null), 
            new NPCDialog("T.Baurekhsa", "Lupakan saja, sebaiknya kamu mulai berjalan ke Mataram sekarang", null),
            new MainCharacterDialog(true, characterExpression.neutral, "......", null),


        }, false, null)
                    }, new Item[]{
                        new Item("Karung_beras", "Karung Beras", "Karung berisi beras yang merupakan makanan pokok rakyat Nusantara", 2),
                        new Item("Karung_gula", "Karung Gula", "Karung berisi gula dari perkebunan bambu Mataram", 3),
                        new Item("Coconut", "Kelapa", "Kelapa yang tentunya tidak jatuh dari pohon dimana tidak ada pohon kelapa di sekitarnya...", 2)
                    })
                }), 
                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi untuk mengantarkan pesan kepada Sultan Agung", 1, new string[] {"Sultan Agung"}, new int[] {-1}, new Story[]{
                        new Story("Sesampainya kembali ke kota untuk bertemu dengan Sultan Agung...", new List<Dialogs>
        {
            new NPCDialog("Sultan Agung", "Dek Player, kamu akhirnya sampai...", new string[]{"Tumenggung Baurekhsa"}),
            new MainCharacterDialog(true, characterExpression.shook, "Akhirnya?", null),
            new NPCDialog("Sultan Agung", " Iya, aku mendapat pesan dari Tumenggung Baurekhsa kalau kamu sedang berjalan kemari sekitar sebulan yang lalu", null),
            new MainCharacterDialog(true, characterExpression.shook, "Sebulan?! Aku hanya berjalan selama beberapa menit?!", null),
            new NPCDialog("Sultan Agung", "Mungkin karena kamu menikmati keindahan alam dan perjalanan jadi tidak terasa", null),
            new MainCharacterDialog(true, characterExpression.think, "(Aneh sekali...tadi itu benar-benar tidak terasa seperti satu bulan)", null),
            new MainCharacterDialog(true, characterExpression.shook, "(Bahkan aku tidak tau tanggal berapa sekarang di dunia antaberanta ini...)", null),
            new NPCDialog("Sultan Agung", "Kamu baik-baik saja, dek Player?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Sultan, kalau boleh bertanya, sekarang ini bulan dan tahun berapa ya?", null),
            new NPCDialog("Sultan Agung", " Agustus 1628, dek MC...Tumenggung Baurekhsa dan pasukannya baru saja akan berangkat ke Batavia", null),
            new NPCDialog("Sultan Agung", "Dan waktu kamu pergi ke Batavia waktu itu...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "...berapa lama Sultan menunggu sampai aku kembali kemari?", null),
            new NPCDialog("Sultan Agung", "Hmmm...sekitar 3 atau 4 bulan? Saya lupa sih", null),
            new MainCharacterDialog(true, characterExpression.hurt, "(Oke...ini tidak benar...waktu memang berjalan sangat aneh di tempat ini....)", null),
            new NPCDialog("Sultan Agung", "Ngomong-ngomong soal itu, saya ingin mengecek kondisi pasukan di Batavia", null),
            new NPCDialog("Sultan Agung", "Tapi jika saya pergi kesana secara langsung sekarang, nanti akan menimbulkan curiga", null),
            new NPCDialog("Sultan Agung", "Bisakah saya meminta bantuan lagi, dek Player?", null),
            new MainCharacterDialog(true, characterExpression.neutral, " Anda ingin saya...kembali ke Batavia lagi?", null),
            new NPCDialog("Sultan Agung", "Kira-kira begitu, saya tau itu perjalanan yang panjang tapi...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Tidak apa-apa, Sultan, saya akan kesana", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Lebih baik daripada saya berdiam disini tanpa melakukan apapun", null),
            new NPCDialog("Sultan Agung", "Baik kalau begitu, terimakasih dek Player, kembalilah kemari ketika kamu sudah mendapat info terkait perang yang terjadi", new string[]{"War Zone"}),
            
        }, false, null)
                    }, true)
                }), 

                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke Batavia untuk memantau jalannya perang", 1, new string[] {"Batavia NPC"}, new int[] {1}, new Story[]{
                        new Story("Dari luar pintu gerbang benteng, terlihat derunya perang antara pasukan Mataram dan Belanda", new List<Dialogs>

        {
            new NPCDialog("Warga", " Hei, kalau saya jadi kamu, saya tidak akan pergi masuk ke benteng", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Memangnya kenapa, pak?", null),
            new NPCDialog("Warga", "Perang besar sedang terjadi disana, warga-warga dari Mataram itu menyerang dan terjadi perang yang sudah berjalan berbulan-bulan sekarang", null),
            new MainCharacterDialog(true, characterExpression.hurt, "....tidak lagi...", null),
            new NPCDialog("Warga", "ya saya setuju, saya juga sudah lelah dengan berbagai perang yang terjadi", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Sekarang ini...memangnya bulan apa, pak?", null),
            new NPCDialog("Warga", "Bulan Desember, tanggal 6, saya rasa perang itu akan mulai mereda beberapa saat lagi...", null),
            new NPCDialog("T. Baurekhsa", " PASUKAN! MUNDUR!", null),
            new MainCharacterDialog(true, characterExpression.shook, "tu...suara Tumenggung...", null),
            new NPCDialog("Warga", "Yap, orang-orang Mataram itu tidak punya kesempatan melawan Belanda dengan senjata canggih seperti itu", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Tapi...apa yang sebenarnya terjadi disitu...", null),
            new NPCDialog("Warga", "Oh, kamu mau tau? Baiklah, saya tidak biasa memberikan informasi secara gratis, tapi khusus kali ini tidak apa-apa", null),
            new NPCDialog("Warga", "Jadi pada awalnya kapal-kapal dari pasukan orang bernama Tumenggung Baurekhsa itu tiba di Batavia", null),
            new NPCDialog("Warga", "Tidak ada serangan sampai hari ketiga, tapi Belanda curiga dan menaruh pasukan-pasukan mereka di dekat Benteng", null),
            new NPCDialog("Warga", "Sore harinya itu, pasukan Mataram mulai turun dan perang pun diluncurkan", null),
            new NPCDialog("Warga", "Terjadi perlawanan tanpa henti, dan disaat inilah Belanda memanggil pasukan tambahan", null),
            new NPCDialog("Warga", "Kini Belanda memiliki lebih banyak pasukan dan pertempuran menjadi sengit", null),
            new NPCDialog("Warga", "Sekitar 3 bulan setelahnya, pasukan Mataram lain mendarat di Batavia", null),
            new NPCDialog("Warga", " Mereka adalah pasukan dari Tumenggung Sura Agul-Agul, dan kedua bersaudara yaitu Kiai Dipati Mandurojo dan Upa Santa. ", null),
            new NPCDialog("Warga", "Terjadi perang sengit antara kedua kubu", null),
            new NPCDialog("Warga", "Tapi pada akhirnya karena sepertinya mereka kekurangan perbekalan dan senjata Belanda itu sangat canggih, mereka mengalami kekalahan...", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Aku sudah tau ini akan terjadi)", null),
            new NPCDialog("Warga", "Dan hari ini, akhirnya mereka mundur juga kan...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Jadi setelah perjuangan itu mereka kalah juga ", null),
            new NPCDialog("Warga", " Iya tapi memang kita tidak akan selalu menang dalam hidup...hanya bisa berusaha sekuat yang kita bisa", null),
            new MainCharacterDialog(true, characterExpression.happy, "(Wow...warga ini filosofikal juga...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sebaiknya aku mencatat semua informasi itu dan kembali ke Sultan Agung)", null),

        }, false, "War")
                    }, true)
                }), 
                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Kembali ke Sultan Agung", 1, new string[] {"Sultan Agung"}, new int[] {-1}, new Story[]{
                        new Story("Sultan Agung nampak seperti sedang banyak pikiran, tentu hal ini terkait dengan kekalahan mereka dalam perang", new List<Dialogs>
        {
            new NPCDialog("Sultan Agung", ".......", new string[]{"War Zone"}),
            new NPCDialog("Sultan Agung", "Oh, dek Player! Saya sudah lama tidak melihatmu, kamu dari mana saja?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Hmmm...aku selalu disini? Mungkin kita tidak pernah saling bertemu lagi saja sejak saat itu", null),
            new NPCDialog("Sultan Agung", "Haha...benar juga...", null),
            new NPCDialog("Sultan Agung", "Tapi ini bukan saat yang tepat lagi sayangnya untuk berbicara dan ramah tamah", null),
            new NPCDialog("Sultan Agung", "Kami akan menyiapkan serangan kembali ke Batavia", null),
            new MainCharacterDialog(true, characterExpression.shook, "Lagi? Tapi bukannya anda baru saja kalah, akankah lebih baik kalau Mataram memulihkan diri dari perang?", null),
            new NPCDialog("Sultan Agung", "Baru saja? Ini sudah bulan Mei 1629, kami sudah menaruh kekalahan itu di belakang kami", null),
            new MainCharacterDialog(true, characterExpression.shook, "(Tunggu...aku terlempar ke Mei 1629?)", null),
            new MainCharacterDialog(true, characterExpression.shook, "(Tempat ini benar-benar aneh ini membuat kepalaku pusing)", null),
            new MainCharacterDialog(true, characterExpression.shook, "(Bagaimana aku bisa berjalan hanya selama beberapa menit membuat waktu berjalan sejauh itu?)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Aku harus mencari informasi lebih lanjut untuk tau cara keluar dari sini...)", null),
            new NPCDialog("Sultan Agung", "Intinya kami sekarang akan mempersiapkan serangan selanjutnya", null),
            new NPCDialog("Sultan Agung", "Kami sudah belajar dari kekalahan sebelumnya...", null),
            new NPCDialog("Sultan Agung", "Bahwa kami kurang perbekalan saat itu", null),
            new MainCharacterDialog(true, characterExpression.happy, " (Setidaknya mereka belajar dari kesalahan sebelumnya dan mencoba memperbaiki strategi mereka...)", null),
            new NPCDialog("Sultan Agung", "Jadi kami akan mencoba mencari akal untuk membangun lumbung di tempat yang tidak mencurigakan", null),
            new NPCDialog("Sultan Agung", "Untuk perbekalan kami", null),
            new NPCDialog("Sultan Agung", "Kalau saya pikir lagi...", null),
            new NPCDialog("Sultan Agung", "Player, apakah kamu bersedia membantu kami lagi?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Aku rasa tidak masalah karena aku tidak ada rencana apapun", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Tidak sampai aku bisa keluar dari tempat ini)", null),
            new NPCDialog("Sultan Agung", "Hoho bagus...", null),
            new NPCDialog("Sultan Agung", "Saya awalnya ingin mengirim pengintai dari Mataram untuk mencari tempat untuk lumbung itu...", null),
            new NPCDialog("Sultan Agung", "Tapi kamu akan nampak lebih tidak mencurigakan bagi orang-orang Belanda itu", null),
            new NPCDialog("Sultan Agung", "Jadi maukah kamu pergi ke Priyangan dan Cirebon untuk menandai tempat yang dapat digunakan untuk membangun lumbung kami?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Aku cukup menandainya saja?", null),
            new NPCDialog("Sultan Agung", "Iya tandai saja tempatnya di petamu dan kirimkan kepada kami setelah kamu selesai", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Oh baiklah, aku rasa aku bisa coba", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Tapi....dimana arah ke Cirebon dan Priyangan?", null),
            new NPCDialog("Sultan Agung", " Sini saya akan menandai sekitaran lokasinya dipetamu...dan sekarang kamu siap berangkat!", null),
            new NPCDialog("Sultan Agung", "Terimakasih Player, saya akan menunggu kabar baiknya", new string[]{"Spot Tree 1", "Spot Tree 2", "Spot Tree 3"}),
            
        }, false, "Mataram")
                    }, true)
                }), 

                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Membantu mencarikan lokasi lumbung di Priyangan", 2, new string[] {"Spot Tree 2", "Spot Tree 1"}, new int[] {-1, -1}, new Story[]{
                        new Story("Kamu menemukan lokasi di dekat rumah-rumah warga", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.neutral, "Hmmm...tempat ini tidak cukup untuk membangun lumbung perbekalan", new string[] {"Spot Tree 2"}),
        }, false, null),
                        new Story("Kamu menemukan lokasi di dekat pantai", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.neutral, "Tempat ini sepertinya cocok, sebaiknya aku menandainya", new string[] {"Spot Tree 1"}),
        }, false, null)
                    }, true),
                    new ExplorationGoal("Membantu mencarikan lokasi lumbung di Cirebon", 1, new string[] {"Spot Tree 3"}, new int[] {-1}, new Story[]{
                        new Story("Kamu menemukan lokasi di balik kawasan rumah warga", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.neutral, "Tempat ini cukup baik, aku akan menandainya", new string[] {"Spot Tree 3"}),
            new MainCharacterDialog(true, characterExpression.neutral, "Sepertinya aku sudah menemukan tempat yang cukup baik, aku sebaiknya kembali ke Sultan Agung", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Hmmmm?", null),
            new MainCharacterDialog(true, characterExpression.hurt, "(Kenapa aku merasa seperti ada yang mengikutiku?)", null),
            new MainCharacterDialog(true, characterExpression.hurt, "(Mungkin bukan ide yg baik untukku untuk pergi ke tempat Sultan Agung, aku sebaiknya mencari alternatif lain...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Mungkin dengan menitipkan peta ini ke seseorang)", new string[]{"Pedagang Stuff"}),
        }, false, null)
                    }, true)
                }), 

                  new Mission(new List<Goal>
                {
                    new GatherGoal("Mencari orang sekitar yang akan ke Mataram", 1, new string[] {"Pedagang Mataram NPC"}, new Story[]{
                        new Story("Kamu menemukan seorang pedagang yang nampak mempersiapkan bawaannya...", new List<Dialogs>
        {
            new NPCDialog("Pedagang", "Ada yang bisa saya bantu nak?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Ah iya pak, apakah bapak akan mengantarkan barang-barang ini ke Mataram?", null),
            new NPCDialog("Pedagang", "Iya nak, saya akan ke Mataram", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Oh kalau begitu, apa saya boleh titip sesuatu untuk Sultan Agung", null),
            new NPCDialog("Pedagang", "Untuk Sultan? Seharusnya bisa ya nak, karena saya langsung bertemu dengannya juga nanti", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah kalau begitu saya titipkan ini kepada bapak ya", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sekarang aku rapikan dulu petaku, lalu setelah memberikannya pada bapak ini, sebaiknya aku pergi ke arah yang berlawananan agar tidak ada yang curiga..)", null)
            
        }, false, null),
                    }, false),
                     new SubmitGoal("Berikan peta yang sudah ditandai ke pedagang", 1, "Pedagang Mataram NPC", null, new Item[]{
                        new Item("Map", "Peta yang sudah ditandai", "Peta yang sudah ditandai untuk kebutuhan khusus", 1)
                    })
                }), 

                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke arah yang berlawanan ke Batavia", 1, new string[] {"Batavia NPC"}, new int[] {-1}, new Story[]{
                        new Story("Kamu bertemu kembali dengan warga yang waktu itu...", new List<Dialogs>
        {
             new NPCDialog("Warga", "Ah, kamu lagi...kamu mau ke medan pertempuran itu lagi ya, kamu ini memang suka menantang maut ya....", new string[]{"Pedagang Stuff", "War Zone"}),
            new MainCharacterDialog(true, characterExpression.angry, "Bukan urusan bapak.", null), 
            new NPCDialog("Warga", "Dan kamu mau tau satu hal yang saya dengar...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Dan itu adalah?", null),
            new NPCDialog("Warga", "Hehe... ", null),
            new MainCharacterDialog(true, characterExpression.hurt, "(Bapak ini meminta bayaran untuk informasi....)", null),
            new MainCharacterDialog(true, characterExpression.hurt, "(Tapi aku tidak punya uang dan aku tidak tau apakah informasi itu akan berguna untukku atau tidak...)", null),
            new MainCharacterDialog(true, characterExpression.hurt, "Bayaran apa yang bapak minta? Saya tidak punya uang karena saya bukan dari sekitar sini", null),
            new NPCDialog("Warga", "Oh begitu rupanya...", null),
            new NPCDialog("Warga", "Hmmm...", null),
            new NPCDialog("Warga", "Ya sudahlah, anggap saja ini dari kebaikan hatiku saja", null),
            new NPCDialog("Warga", "Aku dengar akibat dari perang yang sedang berlangsung itu air disana menjadi tercemar", null),
            new NPCDialog("Warga", "Dan akibat dari itu sekarang pemimpin orang-orang Belanda itu siapa namanya...", null),
            new MainCharacterDialog(true, characterExpression.neutral,"J.P. Coen?", null),
            new NPCDialog("Warga", "Ah iya dia...sekarang dia sepertinya mulai sakit, terkena wabah kolera yang ada ", null),
            new NPCDialog("Warga", "Aku tidak akan terkejut sih kalau sebentar lagi VOC akan kewalahan dengan pemimpinnya sakit seperti itu...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Hmph...itu upah untuk yang serakah...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Tapi jujur saja aku tidak perlu gosip warga seperti ini, anggaplah aku berbuat budi baik saja)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sebaiknya aku cepat kembali ke Sultan Agung saja sebelum warga ini mengoceh lagi...)", null)

        }, false, "War")
                    }, true)
                }), 

                    new Mission(new List<Goal>
                {
                    new ExplorationGoal("Kembali ke Sultan Agung", 1, new string[] {"Sultan Agung"}, new int[] {1}, new Story[]{
                        new Story("Sultan Agung tampak mulai lebih banyak kerut di wajahnya, terbeban dengan kekalahannya yang kedua kali", new List<Dialogs>
        {
        new MainCharacterDialog(true, characterExpression.neutral, "Sultan! Sultan!", new string[]{"War Zone"}),
        new NPCDialog("Sultan Agung", "Ah, dek Player, saya tidak melihatmu sejak kepergianmu mencari lahan lumbung", null),
        new NPCDialog("Sultan Agung", "Saya juga belum berterimakasih atas bantuanmu waktu itu", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Sama-sama, tapi apa yang terjadi dengan serangan kedua, Sultan?", null),
        new NPCDialog("Sultan Agung", "...", null),
        new NPCDialog("Sultan Agung", "Kamu tau lumbung-lumbung yang di bangun di lahan yang kamu tunjukan itu", null),
        new NPCDialog("Sultan Agung", "Belanda mengirim mata-mata ke Mataram dan ia mengetahui dimana lokasi lumbung kita jadinya", null),
        new NPCDialog("Sultan Agung", "Dan mereka menghancurkannya sehingga pasukan kembali kekurangan perbekalan", null),
        new NPCDialog("Sultan Agung", "Ditambah lagi wabah yang terjadi, malaria dan kolera, jadi kami kehilangan banyak pasukan di perang itu...", null),
        new NPCDialog("Sultan Agung", "Tapi dek Player, kami tidak akan menyerah", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Apa anda akan menyerang lagi?", null),
        new NPCDialog("Sultan Agung", "Iya, sampai kami berhasil mengusir orang-orang luar itu dari sini", null),
        new MainCharacterDialog(true, characterExpression.sad, "Tapi apa semua korban yang jatuh dengan serangan demi serangan akan setimpal dengan yang akan anda dapat?", null),
        new NPCDialog("Sultan Agung", "Saya tau...dan saya turut bersedih tentang hal ini", null),
        new NPCDialog("Sultan Agung", "Tapi saya dan pasukan harus terus berjuang, bukan demi kami sendiri", null),
        new NPCDialog("Sultan Agung", "Tapi demi warga Mataram dan seluruh pulau Jawa", null),
        new NPCDialog("Sultan Agung", "Sebaiknya kamu meninggalkan Mataram, dek Player, saya tidak ingin kamu terkena masala", null),
        new MainCharacterDialog(true, characterExpression.sad, "Tapi......", null),
        new NPCDialog("Sultan Agung", "Jangan mengkhawatirkan kami, dek Player", null),
        new NPCDialog("Sultan Agung", "Sebaiknya kamu pergi ke Pelabuhan Jepara dan mencari kepala pedagang disana", null),
        new NPCDialog("Sultan Agung", "Ia mengenal saya, jadi bilang saja padanya kalau saya mengirim kamu untuk ikut bersama mereka", null),
        new MainCharacterDialog(true, characterExpression.sad, "Baiklah...terimakasih Sultan, saya pamit dulu", null),
        new NPCDialog("Sultan Agung", "Saya yang seharusnya berterimakasih, dek Player, selamat jalan!", null)
        }, false, "Judgement")
                    }, true)
                }), 
                    new Mission(new List<Goal>
                {
                    new ExplorationGoal("Lari ke Pelabuhan Jepara", 1, new string[] {"Kepala Pedagang NPC"}, new int[] {1}, new Story[]{
                        new Story("Sesampainya di pelabuhan, ada sebuah kapal yang mulai bersiap untuk berangkat", new List<Dialogs>
        {
        new NPCDialog("Kepala Pedagang", "Hei, hei, kamu mau apa, anak kecil?", new string[]{"Sultan Agung"}),
        new MainCharacterDialog(true, characterExpression.neutral, "Saya teman dari Sultan Agung, pak...ia menitipkan saya untuk bisa ikut dengan bapak dan kapal bapak", null),
        new NPCDialog("Kepala Pedagang", "Anak kecil...teman dari mendiang Sultan...", null),
        new MainCharacterDialog(true, characterExpression.shook, "Mendiang? Jangan-jangan.", null),
        new NPCDialog("Kepala Pedagang", "Hah? Kamu ini mengaku teman dari mendiang Sultan tapi bahkan tidak tau dia meninggal di tahun 1645 ini", null),
        new MainCharacterDialog(true, characterExpression.shook, "(Sekarang tahun 1645??? Aku tidak paham bagaimana waktu berjalan di dunia ini lagi...kenapa aku terlempar bertahun-tahun hanya dengan berjalan ke tempat yang berdekatan...)", null),
        new NPCDialog("Kepala Pedagang", "Sepeninggal beliau Mataram semakin melemah dan membuka peluang untuk orang-orang Belanda itu untuk menguasai Mataram", null),
        new MainCharacterDialog(true, characterExpression.sad, "(Jadi itu akhir dari serangan Sultan Agung...)", null),
        new MainCharacterDialog(true, characterExpression.sad, "Tapi sampai akhir pun dia tidak menyerah berjuang demi rakyatnya)", null),
        new MainCharacterDialog(true, characterExpression.sad, "(Aku turut berduka dan iba dengan nasib rakyat sepeninggalnya...)", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Aku sebaiknya mencatat dan mengabadikan semangatnya...)", null),
        new NPCDialog("Kepala Pedagang", "Baiklah kalau kamu memang benar teman dari Sultan, kamu tidak keberatan kan kalau aku mengetesmu terlebih dahulu", null),
        new NPCDialog("Kepala Pedagang", "Kalau kamu benar teman dari Sultan seharusnya kamu tau tentang kisah serangan yang dia lakukan...", null),
        new NPCDialog("Kepala Pedagang", "Dan jangan berharap dapat mencontek catatan kamu ya, kamu harusnya bisa menjawab diluar kepalamu kalau kamu memang teman sultan", null),
        new NPCDialog("Kepala Pedagang", " Temui saja aku kalau kamu sudah siap", null),
        new MainCharacterDialog(true, characterExpression.hurt, "Oh tidak...kenapa jadi ada ujian begini...)", null),
        new MainCharacterDialog(true, characterExpression.hurt, "Aku merasa dihakimi deh sama kepala pedagang ini, tapi ya sudahlah...)", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Aku hadapi saja judgment ini", null)
        }, false, null)
                    }, true)
                }), 

                  new Mission(new List<Goal>
                {
                    new JudgementGoal("Menyelesaikan tantangan Judgement dari Kepala Pedagang", 15, "Kepala Pedagang NPC", true)
                }), 

                  new Mission(new List<Goal>
                {
                    new ReviewGoal("Mereview kembali beberapa kejadian bersama Kepala Pedagang", 1, "Kepala Pedagang NPC", new Story[]{
                        new Story("Kepala pedagang selesai memberikan review singkat...", new List<Dialogs>
                        {
                               new NPCDialog("Kepala Pedagang", "Sekarang kamu seharusnya sudah lebih paham...", null),
                               new NPCDialog("Kepala Pedagang", "Coba aku tes kembali pemahamanmu itu ya, dan kita tidak akan berhenti sampai kamu mengerti betul", null),
                              new MainCharacterDialog(true, characterExpression.hurt, "Ya Tuhan....", null)
                        }, false, null)
                    }),
                    new JudgementGoal("Menyelesaikan tantangan Judgement dari Kepala Pedagang", 5, "Kepala Pedagang NPC", false)
                }), 

                 new Mission(new List<Goal>
                {
                     new ExplorationGoal("Lari ke sisi lain pelabuhan", 1, new string[] {"Pedagang Anak NPC"}, new int[] {-1}, new Story[]{
                        new Story("Kamu melihat seorang pedagang yang seperti memanggilmu dari kejauhan...", new List<Dialogs>
        {
        new NPCDialog("Pedagang", "Hei, nak! Disini!", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Itu kan...)", null),
        new MainCharacterDialog(true, characterExpression.neutral, " Bapak pedagang yang waktu itu kan?", null),
        new NPCDialog("Pedagang", "Ah bukan, saya anaknya. Tapi bapak saya pernah cerita tentang kamu, nak", null),
        new NPCDialog("Pedagang", "Tadi saya melihat apa yang terjadi disana, saya bisa membantu kamu agar kepala pedagang itu membiarkan kamu masuk", null),
        new NPCDialog("Pedagang", "Nanti akan saya bantu negosiasikan, tapi sebaiknya kamu dengar dulu...", null),
        new NPCDialog("Pedagang", "Sedikit rekap dari kisah serangan Sultan Agung", null),
        
        }, false, null)
                    }, true),
                    new ReviewGoal("Mereview kembali beberapa kejadian bersama pedagang", 1, "Pedagang Anak NPC", new Story[]{
                        new Story("Pedagang selesai memberikan review singkat...", new List<Dialogs>{
                               new NPCDialog("Pedagang", "Apa kamu sudah lebih paham?", null),
                               new NPCDialog("Pedagang", "Sekarang coba saya negosiasikan dengan kepala pedagang terkait ini", null),
                               new NPCDialog("Pedagang", "Kalau kamu sudah siap nantinya, temui saja dia kembali", null),
                               new MainCharacterDialog(true, characterExpression.hurt, "Baiklah aku rasa kembali ke penghakiman", new string[]{"Pedagang Anak NPC"}),
                        }, false, null)
                    })
                }), 

                new Mission(new List<Goal>{
                     new JudgementGoal("Menyelesaikan tantangan Judgement dari Kepala Pedagang", 5, "Kepala Pedagang NPC", false)
                }),
                new Mission(new List<Goal>
                {
                    new GatherGoal("Berbicara dengan kepala pedagang", 1, new string[] {"Kepala Pedagang NPC"}, new Story[]{
                        new Story("Setelah perjuangan cukup panjang menjawab semua pertanyaan kepala pedagang, sepertinya kamu sudah mulai mendapat kepercayaannya", new List<Dialogs>
        {
        new NPCDialog("Kepala Pedagang", "Baiklah...aku rasa karena kamu sudah membuktikan dirimu dan paham betul situasi disini...", new string[]{"Yudha"}),
        new NPCDialog("Kepala Pedagang", "Kamu boleh menumpang di kapal kami", null),
        new NPCDialog("Kepala Pedagang", "Kita akan segera berangkat, sebaiknya kamu bersiap-siap", null),
        new NPCDialog("Kepala Pedagang", "Andai aku bisa menjual lencana aneh ini dulu sebelum kita berangkat", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Lencana?", null),
        new NPCDialog("Kepala Pedagang", " Iya, aku menemukan lencana ini ketika membersihkan kapal...entah bisa dihargai berapa lencana ini...", null),
        new NPCDialog("Kepala Pedagang", " Atau ini aku berikan padamu saja deh...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Untuk saya?", null),
        new NPCDialog("Kepala Pedagang", "Iya, saya tidak akan butuh ini lagi karena pasti tidak akan terjual di Minang..", null),
        new NPCDialog("Kepala Pedagang", "Apalagi dengan Kaum Adat mereka itu, tapi coba kalau kamu bisa menggunakannya untuk sesuatu", null),
        new MainCharacterDialog(true, characterExpression.neutral, " (Hmmm...ini seperti lencana polisi...tapi apa aku bisa menggunakan ini untuk sesuatu?)", null),
        new MainCharacterDialog(true, characterExpression.neutral, " (Lagi pula bagaimana bisa ada lencana polisi di masa ini...)", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Hmmm..sebaiknya aku berkeliling saja mencari pemiliknya sambil menunggu..)", null),

        }, false, "Mataram")
                    }, true)
                }), 
                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Menghampiri seseorang yang mungkin pemilik lencana tersebut", 1, new string[] {"Yudha"}, new int[] {-1}, new Story[]{
                        new Story("Kamu menghampiri orang itu yang sedang sibuk mencari sesuatu", new List<Dialogs>
        {
        new MainCharacterDialog(true, characterExpression.shook, "Hmmmm? Dia ini kan yang tadi...", null),
        new NPCDialog("???", " ...kamu anak kecil yang tadi", null),
        new NPCDialog("???", " Sebutkan urusanmu dan pergi anak kecil, aku sedang mencari sesuatu...", null),
        new MainCharacterDialog(true, characterExpression.angry, "Mencari apa? Tindakan ilegal baru?", null),
        new NPCDialog("???", "Hah! Aku tidak tau kamu dapat kesimpulan itu dari mana", null),
        new NPCDialog("???", "Tapi aku sedang mencari lencana polisiku", null),
        new MainCharacterDialog(true, characterExpression.shook, "Anda seorang polisi?", null),
        new NPCDialog("???", "Kalau iya memang kenapa?", null),
        new MainCharacterDialog(false, characterExpression.angry, "Aku Yudha, seorang detektif dari kepolisian, spesialisasi di kasus penjelajahan waktu", null),
        new MainCharacterDialog(true, characterExpression.neutral, "......", null),
        new MainCharacterDialog(false, characterExpression.neutral, "Kenapa?", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Tidak...aku pikir...ya sudahlah lupakan", null),
        new MainCharacterDialog(false, characterExpression.neutral, " Kalau kamu berpikir aku adalah penjelajah waktu ilegal...", null),
        new MainCharacterDialog(false, characterExpression.angry, "Kamu harus tau kalau aku juga mengejar penjelajah itu", null),
        new MainCharacterDialog(false, characterExpression.angry, "Karena itu aku mengaktifkan mesin itu yang dicurigai sebagai mesin waktu miliknya untuk mencari petunjuk...", null),
         new MainCharacterDialog(false, characterExpression.neutral, " Tapi sekarang kita berdua disini, di tempat yang seperti kita di masa lalu tapi disisi lain...", null),
         new MainCharacterDialog(false, characterExpression.neutral, "Tempat ini membuatnya seakan kita sedang dalam sebuah game RPG petualangan ", null),
         new MainCharacterDialog(false, characterExpression.neutral, "Sehingga kita dapat berinteraksi dengan semua orang-orang dan perang yang terjadi disana", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Kalau begitu...kita adalah tokoh utama dari permainan ini?", null),
         new MainCharacterDialog(false, characterExpression.neutral, "Itu hanya analogi, anak kecil... ", null),
         new MainCharacterDialog(false, characterExpression.neutral, "Oh iya, aku belum tau namamu dan kenapa kamu bisa disini", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Namaku Player, dan aku kira anda adalah orang yang mencurigakan jadi..", null),
         new MainCharacterDialog(false, characterExpression.hurt, " ...aku rasa kalau aku bekerja aku bisa nampak mencurigakan sih", null),
         new MainCharacterDialog(false, characterExpression.neutral, "Tapi sekarang bukan waktu yang baik untuk berbicara", null),
         new MainCharacterDialog(false, characterExpression.neutral, "Aku masih harus menemukan lencanaku", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Mungkin kalau aku mengembalikan lencananya, dia akan mau membantuku keluar dari sini", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Setidaknya dengan adanya polisi aku akan jauh merasa lebih ama", null),
        
        }, false, "Funny Yudha")
                    }, true),
                     new SubmitGoal("Memberikan lencana kembali kepada Yudha", 1, "Yudha", new Story[]{
                        new Story("Yudha dengan cepat mengambil kembali lencananya darimu", new List<Dialogs>
        {
        new MainCharacterDialog(false, characterExpression.happy,"Oh hei, kamu menemukannya! Terimakasih banyak!", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Sama-sama, kalau begitu karena aku menemukannya untuk anda...", null),
        new MainCharacterDialog(false, characterExpression.neutral, "Ya sudah cukup dulu pembicaraan ini untuk sekarang", null),
        new MainCharacterDialog(false, characterExpression.neutral, "Aku akan kembali ke pekerjaanku dan kamu kembali ke urusanmu ya anak kecil...", null),
        new MainCharacterDialog(true, characterExpression.hurt, "Tunggu, anda akan pergi begitu saja?", null),
        new MainCharacterDialog(false, characterExpression.neutral, " Iya, aku berterima kasih atas bantuanmu menemukan barang-barangku...", null),
        new MainCharacterDialog(false, characterExpression.neutral, " ...tapi aku tidak ada niatan membawa anak kecil sepertimu", null),
        new MainCharacterDialog(false, characterExpression.neutral, "Nanti kamu malah mengganggu  penyelidikanku", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Tapi bukannya akan lebih baik kalau punya lebih dari satu kepala ya...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Lagi pula kita memiliki tujuan yang sama nantinya, keluar dari tempat ini", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Dan anda juga yang membuat saya ikut terjebak di dunia ini bersama anda...", null),
        new MainCharacterDialog(false, characterExpression.shook, " Itu sih karena kamu yang sok ingin menghentikanku melakukan pekerjaanku", null),
        new MainCharacterDialog(false, characterExpression.neutral, "Lagipula aku terbiasa kerja sendiri jadi yahh aku permisi dulu saja, kapalku ke Minang di dunia ini akan segera berangkat", new string[]{"Yudha"}),
        new MainCharacterDialog(true, characterExpression.angry, "Dasar keras kepala..dan tidak tau berterimakasih", null),
        new MainCharacterDialog(true, characterExpression.angry, "Kenapa dia tidak mau bekerja sama denganku padahal tujuan kita sama...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Tapi aku rasa Sultan Agung juga begitu, ia hanya berjuang demi daerahnya saja..", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Andai ia  menerima atau meminta bantuan daerah lain..", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Ya sudahlah aku sebaiknya bergegas, kapalnya akan segera berangkat", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Masih akan jauh sebelum aku bisa keluar dari tempat ini sepertinya...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Hmmm? Apa ini?", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Seperti catatan jurnal...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Aku sebaiknya menyimpannya dan membukanya nanti ", new string[]{"Next Chapter"}),
        }, false, null)
                    }, new Item[]{
                        new Item("police_badge", "Lencana Polisi", "Apa ada polisi di sekitar sini?", 1)
                    })
                }),

               new Mission(new List<Goal>{
                     new ExplorationGoal("Pergi ke lokasi selanjutnya dengan kapal", 1, new string[] {"Next Chapter"}, new int[] {-1}, new Story[]{
                        new Story("Dan perjalananmu pun masih berlanjut..", new List<Dialogs>
        {
       
            new CutsceneDialog("cutscene_indonesia", "Setelah perjuangan yang cukup panjang dari berbagai daerah lainnya, akhirnya kongsi dagang itu bubar pada tahun 31 Desember 1799", null),
            new CutsceneDialog("cutscene_indonesia", "Tapi Nusantara belum lepas dari tangan Belanda", null),
            new CutsceneDialog("cutscene_suffering", "Pemimpin-pemimpin dari Belanda, dan bahkan Inggris sempat membawa alur pemerintahan di Nusantara", null),
            new CutsceneDialog("cutscene_suffering", "Tapi tidak ada dari mereka yang membawa kemakmuran kepada Nusantara, atau Bumiputera, atau Hindia Belanda pada masanya masing-masing", null),
            new CutsceneDialog("cutscene_suffering", "Jalan Anyer-Panarukan hingga tanam paksa, semua hasil pengerjaan paksa dari sekian banyak rakyat pada masanya", null),
            new CutsceneDialog("cutscene_perjuangan", "Bahkan penguasa pribumi yang penuh keserakahan justru menambah penderitaan dari rakyat biasa", null),
            new CutsceneDialog("cutscene_perjuangan", "Dan perjuangan dari rakyat Hindia Belanda pun masih terus berlangsung, untuk mengusahakan kemerdekaan mereka...", null)
        }, true, "Judgement")
                    }, true)
               })
            };
        }

        else
        {
            mission = new List<Mission>
            {// chapter 2 missions
               new Mission(new List<Goal>
                {

                        new ExplorationGoal("Pergi mengikuti Yudha", 1, new string[] {"Yudha Paguruyung"}, new int[] {-1}, new Story[] {
                        new Story("Kamu pun mengikuti detektif itu sampai ke suatu tempat yang ramai dengan aktivitas warga...", new List<Dialogs>
                        {
                            new MainCharacterDialog(false, characterExpression.neutral, "Oh lihat...siapa yang memutuskan untuk mengikutiku...", null),
                            new MainCharacterDialog(true, characterExpression.angry, "Aku juga sebenarnya tidak mau, tapi anda adalah satu-satunya kunci yang aku miliki untuk keluar dari dunia ini...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Tidak salah sih, ya sudahlah aku mengalah saja, penting kamu tidak mengganggu investigasiku", null),
                            new MainCharacterDialog(false, characterExpression.happy, "Bicara soal itu, kamu ini pandai mencari informasi kan?", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Iya...?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Mungkin kamu akan berbakat sebagai detektif", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Oke sebaiknya kita berpencar dan mencari informasi disini", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Kamu coba cari informasi dari warga-warga disana, sementara aku ke arah lainnya", new string[]{"Yudha Paguruyung"}),
                            new MainCharacterDialog(true, characterExpression.shook, "Eh tapi...", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Astaga..dia keburu pergi...", null),
                            new MainCharacterDialog(true, characterExpression.think, " (Ya sudahlah aku akan mencarinya lagi saja setelah aku mencari tau apa yang terjadi disana...)", null),
                        }, false, "Funny Yudha")}, true)}), 

               new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke sekitar Pagaruyung untuk mencari informasi", 2, new string[] { "Sambung Ayam", "Warga Padri"}, new int[] {-1, 3}, new Story[] {
                        new Story("Kamu melihat keramaian mengintari dua orang berpakaian hitam yang sedang melihat 2 ekor ayam yang diadu...", new List<Dialogs>
        {
          new NPCDialog("Warga Kaum Adat 1", "Ho, kamu tertarik menonton sabung ayam, anak kecil?", null),
          new MainCharacterDialog(true, characterExpression.hurt, "(Kasihan ayam-ayam itu, tapi sepertinya ini memang adat mereka..)", null),
          new NPCDialog("Warga Kaum Adat 1", "Ayam-mu sepertinya sudah kewalahan tuh.. Aku yang akan menang.. Hahaha!", null),
          new NPCDialog("Warga Kaum Adat 2", "Tidaaak! Ayamku!!", null),
          new NPCDialog("Warga Kaum Adat 1", "Ayo, berikan duitnya sesuai adat kita sebagai sesama Kaum Adat.", null),
          new MainCharacterDialog(true, characterExpression.sad, "(Sebaiknya aku pergi dari sini...)", null),
          new MainCharacterDialog(true, characterExpression.sad, "(Aku tidak tega melihat ayam diadu sampai terluka seperti itu...)", null),

        }, false, "Chicken"),
                        new Story("Kamu melihat orang berpakaian putih menatap mereka dengan mata tidak senang...", new List<Dialogs>
        {
          new NPCDialog("Warga Kaum Padri", "Dasar kaum Adat itu!", null),
          new MainCharacterDialog(true, characterExpression.think, "Kaum Adat?", null),
          new NPCDialog("Warga Kaum Padri", "Ya, mereka yang berjudi dengan sabung ayam itu.", null),
          new MainCharacterDialog(true, characterExpression.hurt, "Oh, yang barusan kulihat itu.", null),
          new NPCDialog("Warga Kaum Padri", "Padahal hal tersebut dilarang dalam ajaran Islam.", null),
          new NPCDialog("Warga Kaum Padri", "Aku kaum Padri, bersumpah tidak akan melakukan hal-hal seperti yang mereka lakukan!", null),
          new MainCharacterDialog(true, characterExpression.think, "Memangnya mereka melakukan apa saja?", null),
          new NPCDialog("Warga Kaum Padri", "Judi, sabung ayam, dan minum-minuman keras...", null),
          new NPCDialog("Warga Kaum Padri", "Intinya, jangan dekat-dekat dengan mereka.", null),
          new NPCDialog("Warga Kaum Padri", "Mereka hanya akan membawa pengaruh buruk.", null),
          new NPCDialog("Warga Kaum Padri", "Jangan tumbuh menjadi orang seperti mereka, mengerti?", null),
          new MainCharacterDialog(true, characterExpression.shook, "I-iya pak, terima kasih atas peringatannya..", null),
          new MainCharacterDialog(true, characterExpression.think, "(Kelihatannya warga ini sangat menentang apa yang kaum Adat lakukan)", null),

        }, false, "Minang"),
                     }, false)
                }),

               new Mission(new List<Goal>
                {

                        new ExplorationGoal("Kembali ke Yudha", 1, new string[] {"Yudha Paguruyung"}, new int[] {-1}, new Story[] {
                        new Story("Kamu pun kembali dan melihat Yudha sang detektif sudah menunggumu...", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.neutral, "Hei pak Yudha, aku sudah mendapatkan informasi.", null),
                            new MainCharacterDialog(false, characterExpression.happy, "Kerja bagus, anak kecil!", null),
                            new MainCharacterDialog(false, characterExpression.hurt, "Tapi sebaiknya jangan memanggilku pak, aku belum setua itu", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Panggil aku kak atau apapun yang kamu mau, tapi jangan pak, jangan om, jangan daddy apalagi...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah....", null),
                            new MainCharacterDialog(true, characterExpression.neutral, " Ya sudah, aku langsung ke intinya saja.", null),
                            new MainCharacterDialog(true, characterExpression.think, "Sepertinya ada perselisihan antara kaum Adat dan kaum Padri", null),
                            new MainCharacterDialog(true, characterExpression.think, "Karena kaum Adat tidak menuruti ajaran Islam", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Hmmmm....sepertinya kejadian disini menyerupai sejarah perang Padri", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Sepertinya aku pernah mendengar tentang perang itu di sekolah...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Iya, itu diajarkan di sekolah memang...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "...sebentar lagi sepertinya akan terjadi bentrokan antara kaum Padri dan Adat.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Sebaiknya kita pergi dari sini, lagipula sudah tidak ada yang menarik dari tempat ini...", null),
                            new MainCharacterDialog(false, characterExpression.happy, "Aku duluan ya.. Byee..", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Hei, tunggu dulu..", new string[]{"Yudha Paguruyung", "Yudha Paguruyung 2", "James"}),
                        }, false, "Funny Yudha")
                    }, true)
                }),

               new Mission(new List<Goal>
                {

                        new ExplorationGoal("Mengikuti Yudha di Pagaruyung", 1, new string[] {"Yudha Paguruyung 2"}, new int[] {-1}, new Story[] {
                        new Story("Kamu pun kembali menemukan Yudha di balik sebuah rumah...", new List<Dialogs>
                        {
                            new MainCharacterDialog(false, characterExpression.neutral, "Lama sekali jalanmu, anak kecil...", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Hufttt...huftt...aku...sudah...berusaha...secepat...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Sudah-sudah, kamu tarik nafas terlebih dahulu", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Sekarang, sudah terjadi bentrokan yang aku sebutkan itu.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Jadi, sekarang sebaiknya kita berbaur dengan warga sekitar dan melihat jalannya kejadian disini.", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Tunggu...bagaimana ini akan membantu kita keluar dari tempat ini?", null),
                            new MainCharacterDialog(false, characterExpression.angry, "....", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Kenapa anda tidak menjawabku?", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Kamu tau peribahasa itu kan, kalau kadang-kadang rasa penasaran dapat membunuhmu", null),
                            new MainCharacterDialog(false, characterExpression.think, "Terutama untuk anak yang sok ingin tau semua hal seperti kamu...", null),
                            new MainCharacterDialog(true, characterExpression.angry, "Hei!", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Tapi jawaban simpelnya adalah, kalau teoriku benar, kejadian-kejadian sejarah ini nantinya akan mengantarkan kita ke jalan keluar itu dengan sendirinya, dengan mengikutinya saja...", null),
                            new MainCharacterDialog(false, characterExpression.happy, "Oke sebagai bayaranmu untuk bertanya pertanyaan yang tidak sesuai standar detektif seperti tadi, aku akan memberimu tugas", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Coba kamu mendekat ke orang Belanda itu dan cari tau apa yang terjadi...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Aku akan mengawasimu dan mendukungmu dalam doa", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Apa yang dia lakukan disana...dan kenapa jadi aku yang harus mendekat?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Karena kalau aku yang tertangkap kita berdua akan lebih dalam masalah daripada kalau kamu yang tertangkap", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "(Jadi aku ini dijadikan tumbal, terimakasih banyak kak detektif Yudha...)", null),
                        }, false, "Minang")
                    }, true)
                }),

                new Mission(new List<Goal>
                {

                        new ExplorationGoal("Mendekat ke lokasi orang Belanda tanpa diketahui olehnya..", 1, new string[] {"Pohon James"}, new int[] {1}, new Story[] {
                        new Story("Kamu pun mencoba mendekat dan mendengar pembicaraan orang Belanda itu...", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.think, "(Hmmmm... Orang Belanda itu sedang bergumam sendiri di depan sebuah bangunan)", new string[]{"Yudha Paguruyung 2"}),
                            new NPCDialog("???", "Hahahaa... Those fools accepted the agreement (Hahahaa... orang-orang bodoh itu menerima perjanjiannya).", null),
                            new NPCDialog("???", "Their desire to make the Padri lose is very exploitable (Keinginan mereka untuk membuat Padri kalah sangatlah mudah dieksploitasi).", null),
                            new NPCDialog("???", "We will join forces with the Adat to fight the Padri and conquer some areas in the meantime (Kita akan bergabung dengan kaum Adat dan menguasai beberapa area untuk sekarang).", null),
                            new NPCDialog("???", "Imagining it already makes me satisfied.. Hahaha... (Membayangkannya saja sudah membuatku puas... Hahaha...).", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Apa yang orang itu sedang rencanakan...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Ia James Du Puy.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Ia baru saja mengadakan perjanjian persahabatan dengan tokoh Adat. yaitu Tuanku Suruaso dan 14 Penghulu Minangkabau.", null),
                            new MainCharacterDialog(true, characterExpression.think, "Untuk apa?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Yah. singkatnya kaum Adat ingin bantuan Belanda untuk mengalahkan kaum Padri.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Belanda memanfaatkan perjanjian tersebut untuk menguasai beberapa wilayah di Minangkabau.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Sebentar lagi mereka akan menguasai Simawang dan perang tidak akan terhindarkan pada tahun 1821 karena kaum Padri akan menentang pendudukan wilayah tersebut..", null),
                            new MainCharacterDialog(true, characterExpression.sad, "Kalau saja kita bisa menghentikan perang itu...", null),
                            new MainCharacterDialog(false, characterExpression.angry, "....", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Satu hal yang harus kamu tau, seburuk apapun sejarah atau masa lalu, kita tidak boleh berusaha mengubahnya", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Dengan mengubah sejarah apalagi untuk orang lain, kita tidak menghargai kisah perjuangan mereka dengan menambahkan bumbu dari kita sendiri", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Itu seperti memasukan garam ke makanan yang seharusnya tidak asin", null),
                            new MainCharacterDialog(true, characterExpression.sad, "...baiklah kalau anda bilang begitu...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Tidak perlu berpikir aneh-aneh, atau aku akan menangkapmu sebagai potensi ancaman nasional", null),
                            new MainCharacterDialog(true, characterExpression.shook, "....", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Ya sudah, disini sudah tidak ada apa-apa lagi, sebaiknya sekarang kita mencari seseorang", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Mencari siapa?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Mencari seorang tokoh dari kaum padri, ayo kita pergi", null),
                        }, false, null)
                    }, true)
                }),

                new Mission(new List<Goal>
                {

                        new ExplorationGoal("Pergi Mencari Tokoh Kaum Padri", 1, new string[] {"Tuanku Lintau"}, new int[] {-1}, new Story[] {
                        new Story("Kamu dan Kak Yudha telah sampai ke seseorang yang seperti petinggi disana", new List<Dialogs>
                        {
                            new MainCharacterDialog(false, characterExpression.neutral, "Seharusnya kita sudah sampai...semuanya akan baik-baik saja dari sini...", new string[]{"James"}),
                            new NPCDialog("???", "Tahan! Kalian mau apa disini?", null),
                            new NPCDialog("???", "Pasukan, tangkap mereka!", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "(...yap, kita benar-benar tidak baik-baik saja...)", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Hei hei hei, bapak-bapak yang terhormat, sbelum kalian langsung menangkap kami tanpa bukti, bukannya lebih baik memeriksa kami dulu?s", null),
                            new NPCDialog("???", "Pasukan, geledah barang-barang mereka.", null),
                            new NPCDialog("Pasukan 1", "Hanya ada semacam kotak hitam di kantong anak kecil ini.", null),
                            new NPCDialog("Pasukan 2", "Tidak ada hal yang berhubungan dengan Belanda.", null),
                            new NPCDialog("???", "Maaf telah mencurigai kalian karena kami sedang mempersiapkan perang.", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Maaf kalau saya lancang, bolehkah saya tau siapa anda sehingga bisa seenaknya menangka kami seperti tadi?", null),
                            new NPCDialog("???", "Hmmm iya juga, saya malah lupa memperkenalkan diri.", null),
                            new NPCDialog("Tuanku Lintau", "Namaku Tuanku Lintau.", null),
                            new NPCDialog("Tuanku Lintau", "Siapa namamu dan tuan yang disana?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Namaku Player", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Yudha", null),
                            new NPCDialog("Tuanku Lintau", "Saya meminta maaf atas kelancangan saya tadi.", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Aku rasa tidak ada salahnya berhati-hati, Belanda memang banyak melakukan hal yang licik", null),
                            new NPCDialog("Tuanku Lintau", "Belanda juga mengganggu kalian ya?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Aku melihat sendiri apa yang mereka lakukan pada Sultan Agung dan Mataram", null),
                            new MainCharacterDialog(false, characterExpression.shook, "....", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Maksudku...aku sudah mendengar banyak cerita tentang kelicikan mereka", null),
                            new NPCDialog("Tuanku Lintau", "Begitu ya, kalau begitu, apa saya boleh minta bantuan pada kalian?", null),
                            new NPCDialog("Tuanku Lintau", "Sepertinya kalian tidak akan dicurigai karena kalian tidak berpakaian seperti orang sini.", null),
                            new NPCDialog("Tuanku Lintau", "Kami sedang mempersiapkan pasukan untuk mengadakan serangan ke pos-pos di Simawang", null),
                            new MainCharacterDialog(true, characterExpression.think, "Lalu, apa yang harus kami lakukan?", null),
                            new NPCDialog("Tuanku Lintau", "Kamu bisa pergi kesana dan memantau keadaannya untuk kami", null),
                            new NPCDialog("Tuanku Lintau", "Saya sedang bersiap-siap dan memperkirakan seberapa banyak pasukan yang dibutuhkan.", null),
                            new NPCDialog("Tuanku Lintau", "Kita ada sekitar 20.000 sampai 25.000 pasukan untuk sekarang.", null),
                            new NPCDialog("Tuanku Lintau", "Tapi, kami juga harus melihat seberapa pasukan yang mereka siapkan agar kita bisa mengantisipasinya", null),
                            new NPCDialog("Tuanku Lintau", "Seharusnya dengan pasukan dan senjata yang ada sudah cukup untuk menyerang mereka sih.", null),
                            new MainCharacterDialog(true, characterExpression.think, "(Aku tidak yakin dengan pernyataan itu...nanti kejadiannya seperti Sultan Agung lagi)", null),
                            new MainCharacterDialog(true, characterExpression.think, "(Aku akan mempertimbangkannya setelah investigasi nanti..)", null),
                            new MainCharacterDialog(true, characterExpression.happy, "Serahkan saja pada kami", null),
                            new NPCDialog("Tuanku Lintau", "Terimakasih banyak karena mau membantu, nak Player dan Yudha.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Dari sini ke Simawang harusnya tidak jauh tapi-", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Ayo kita bergegas kesana", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "(...aku tidak yakin kita akan kembali ke Lintau tepat waktu)", null),
                        }, false, null)
                    }, true)
                }),

                new Mission(new List<Goal>
                {

                        new ExplorationGoal("Pergi ke Simawang untuk mengintai pos pertahanan di Simawang", 1, new string[] {"Simawang Fort Gate"}, new int[] {-1}, new Story[] {
                        new Story("Kamu dan Kak Yudha telah sampai di pos pertahanan Belanda di Simawang...", new List<Dialogs>
                        {
                            new MainCharacterDialog(false, characterExpression.think, "Oke kita harus selesaikan ini dengan cepat, saatnya kita devide et impera", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Devide et impera? Anda ingin menggunakan strategi orang-orang Belanda itu?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Apa salahnya, itu artinya hanya Divide and Conquer, kok!", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Baiklah, kamu coba cek ke daerah sana, ke pasukan-pasukan itu, dan aku akan tanya ibu-ibu disana", null),
                            new MainCharacterDialog(true, characterExpression.shook, "(Kenapa aku yang selalu dapat tugas yang berbahaya....)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Sabar Player, ini demi perdamaian Minang dan agar kamu bisa keluar dari sini juga...)", null),
                            new MainCharacterDialog(true, characterExpression.think, "(Hmmm...tapi aku tidak bisa masuk lewat gerbang itu tentunya, aku sebaiknya mencari jalan mengintar)", null),
                        }, false, null)
                    }, true)
                }),

                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Mengintai pos pertahanan di Simawang", 1, new string[] { "Eavesdropping"}, new int[] {1}, new Story[] {
                        new Story("Kamu melihat dua orang tentara Belanda yang sedang berbincang...", new List<Dialogs>
       {
                            new NPCDialog("Pasukan Belanda 1", "Untuk sekarang belum ada tanda-tanda perlawanan dari Padri.", null),
                            new NPCDialog("Pasukan Belanda 2", "Yang penting kita sudah menyiapkan pasukan untuk berjaga-jaga kan?", null),
                            new NPCDialog("Pasukan Belanda 2", "Sudah kok, kita menyiapkan 200 serdadu.", null),
                            new NPCDialog("Pasukan Belanda 2", "Kamu yakin segitu cukup untuk mengalahkan mereka?", null),
                            new NPCDialog("Pasukan Belanda 2", "Pasukan Padri itu jumlahnya tidak sedikit.", null),
                            new NPCDialog("Pasukan Belanda 2", "Kamu lupa kita bersatu dengan Kaum Adat ya?", null),
                            new NPCDialog("Pasukan Belanda 2", "Baiklah.. Ayo lanjutkan patrolinya..", null),
                            new MainCharacterDialog(true, characterExpression.think, "(Hmmm..sebaiknya aku mencatat informasi-informasi yang kudengar itu.)", null),
                            new MainCharacterDialog(false, characterExpression.happy, "Hei, Player!", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Waaahh! Jangan menakutiku dengan tiba-tiba muncul seperti itu dong!", null),
                            new MainCharacterDialog(false, characterExpression.hurt, "Maaf-maaf, bukan salahku juga kamu gampang terkejut...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Jadi...bagaimana hasil dari anda menggoda ibu-ibu disana?", null),
                            new MainCharacterDialog(false, characterExpression.think, "Oh mereka menunjukan padaku arah ke gudang persenjataan", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Aku melihat banyak meriam dan senjata api di dalamnya.", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Kita harus memperingatkan Tuanku Lintau mengenai hal ini", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Baiklah...ayo kita bergegas kembali ke-", null),
                            new NPCDialog("???", "KITA DISERANG!!!!", new string[]{"War Zone Sim"}),
                            new MainCharacterDialog(false, characterExpression.neutral, "...sudah aku duga ini akan terjadi...", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Apanya?! Apanya yang anda sudah duga?", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Kalau kamu mungkin sudah menyadarinya, waktu di dunia ini berjalan secara berbeda untuk kita berdua dan orang-orang yang ada disini", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Mungkin untuk kita waktu baru berjalan beberapa menit, tapi untuk mereka sudah lewat berhari-hari, bahkan berbulan-bulan", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Dari situlah aku berani menyimpulkan bahwa dunia antaberanta ini bukan masa lalu yang sebenarnya", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Tapi kita bicarakan itu lagi nanti, sebaiknya kita segera pergi dari sini dulu", null),
                        }, false, "War")
                    }, true)
                }),

                // Ubah dulu

                new Mission(new List<Goal>
                {

                        new ExplorationGoal("Kembali ke pertahanan Tuanku Lintau", 1, new string[] {"Tuanku Lintau"}, new int[] {1}, new Story[] {
                        new Story("Kamu dan Kak Yudha berhasil menhindari mayoritas daerah perang dan kembali ke tempat Tuanku Lintau, dimana sesampainya disana perang nampak sudah mulai mereda...", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.shook, "Lihat! Kita akhirnya sampai!", null),
                            new NPCDialog("Tuanku Lintau", "Oh, nak Player dan tuan Yudha", new string[]{"War Zone Sim"}),
                            new NPCDialog("Tuanku Lintau", "Kami khawatir kenapa kalian tidak kembali sejak mengutus kalian, kami pikir kalian sudah tertangkap oleh serdadu Belanda", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Tidak, kami baru mau kembali untuk melaporkan 200 serdadu dan 10.000 pasukan pribumi yang kami temukan, tapi perang sudah meletus", null),
                            new NPCDialog("Tuanku Lintau", "Iya...tapi akhirnya kami harus mundur kembali ke sini, di Lintau.", null),
                            new NPCDialog("Tuanku Lintau", "Kami kehilangan 350 orang, termasuk putraku...", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Saya turut berduka tuan...", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Iya saya juga turut berduka...", null),
                            new NPCDialog("Tuanku Lintau", "...", null),
                            new NPCDialog("Tuanku Lintau", "Tidak masalah, aku masih bisa membalikan keadaan ini", null),
                            new NPCDialog("Tuanku Lintau", "Aku akan berperang lagi, untuk membalas dendam putraku juga...", null),
                            new NPCDialog("Tuanku Lintau", "Saya akan memusatkan perjuangan dari sini", null),
                            new NPCDialog("Tuanku Lintau", "Temanku Tuanku Nan Renceh sedang memimpin pasukannya di sekitar Baso.", null),
                            new NPCDialog("Tuanku Lintau", "Maaf ya nak Player, Yudha, saya butuh waktu untuk sendiri dan untuk mempersiapkan perang, saya permisi", null),
                            new MainCharacterDialog(true, characterExpression.think, "(Perang sudah mau mulai lagi ya..)", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Hei, kau jangan bengong.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Kita harus mencari informasi lebih lanjut terkait dengan perang ini", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah...aku rasa kita dapat bertanya pada warga dekat sini", null),
                        }, false, "Minang")
                    }, true)
                }),

               // Ubah dulu

                new Mission(new List<Goal>
                {

                        new ExplorationGoal("Mencari informasi mengenai perang yang terjadi", 1, new string[] {"Warga 4"}, new int[] {1}, new Story[] {
                        new Story("Kamu menghampiri warga di sekitar Lintau...", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.neutral, "Permisi, Pak. Apa anda tau tentang perang yang terjadi belakangan ini?", null),
                            new NPCDialog("Warga", "Maksudmu perang padri itu?", null),
                            new NPCDialog("Warga", "Kemana saja kalian 5 tahun ini sampai kalian tidak tau, atau kalian pendatang baru disini ya?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Bisa...dibilang begitu?", null),
                            new NPCDialog("Warga", "Oh kalau begitu kalian datang di saat yang tidak tepat...", null),
                            new NPCDialog("Warga", "Dari tahun 1821 sampai 1825, serangan dari kaum Padri di fase pertama ini meluas di seluruh tanah Minangkabau.", null),
                            new NPCDialog("Warga", "Pada September 1822 kaum Padri berhasil mengusir Belanda dari Sungai Puar, Guguk Sigandang, dan Tajong Alam.", null),
                            new NPCDialog("Warga", "Lalu, pada tahun 1823 pasukan Padri berhasil mengalahkan tentara Belanda di Kapau.", null),
                            new NPCDialog("Warga", "Belanda hanya merasa terdesak sehingga mereka membuat suatu perjanjian pada tanggal 26 Februari 1824.", null),
                            new NPCDialog("Warga", "Tetapi aku tidak tahu jelas isi perjanjian tersebut, tapi itu seperti perjanjian damai", null),
                            new MainCharacterDialog(true, characterExpression.think, "(Hmmm...dimana kami bisa mendapatkan perjanjian itu...)", null),
                            new MainCharacterDialog(false, characterExpression.shook, "Hei, kamu tidak berpikir untuk berusaha mendapatkan perjanjian itu kan...", null),
                            new MainCharacterDialog(true, characterExpression.think, "Tapi kalau kita tidak berusaha mendapatkannya, bagaimana kita dapat mengerti dunia ini secara lebih detail", null),
                            new MainCharacterDialog(false, characterExpression.shook, "Perjanjian itu tidak akan secara random kamu temukan tergeletak di tanah, coba saja kalau kamu bisa menemukannya tapi aku akan menunggu sampai kamu kembali saja", null),
                              new MainCharacterDialog(true, characterExpression.think, "(Hmph, suka-suka dia saja, aku tau sendiri kalau dokumen di dunia ini bisa saja tergeletak begitu saja ditanah, aku akan menemukannya)", new string[]{"Perjanjian Masang"}),
                        }, false, null)
                    }, true)
                }),

                new Mission(new List<Goal>
                {

                        new ExplorationGoal("Mendapatkan Dokumen Perjanjian Masang di sekitar", 1, new string[] {"Perjanjian Masang"}, new int[] {-1}, new Story[] {
                        new Story("Kamu mendapati sebuah dokumen yang tergeletak di tanah...", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.think, "Apa ini?", null),
                            new MainCharacterDialog(true, characterExpression.happy, "Mari kita lihat...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Ini dia, perjanjian masang! Aku akan menyimpannya di jurnalku...)", new string[]{"Perjanjian Masang"}),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Perjanjian ini terjadi pada tanggal 26 Januari 1824 di wilayah Alahan Panjang)", null),
                            new MainCharacterDialog(true, characterExpression.think, "(Sepertinya ini semacam perundingan damai antara Belanda dan kaum Padri)", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "(Sebaiknya aku kembali ke kak Yudha dan menunjukan hasil kerja kerasku!)", new string[]{"Yudha (4)"}),
                        }, false, null)
                    }, false)
                }),

                new Mission(new List<Goal>
                {

                        new ExplorationGoal("Kembali ke Yudha di Lintau", 1, new string[] {"Yudha (4)"}, new int[] {1}, new Story[] {
                        new Story("Kamu kembali pada Yudha yang masih di sekitar Lintau", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.happy, "Kak Yudha, lihat apa yang aku dapatkan!", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Perjanjian Masang...tapi bagaimana bisa?", null),
                            new MainCharacterDialog(true, characterExpression.happy, "Perjanjiannya tergeletak secara random di tanah, haha!", null),
                            new MainCharacterDialog(false, characterExpression.shook, "Dasar dunia yang aneh....", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Tapi dengan ini, apa ini artinya perang sudah selesai?", null),
                            new MainCharacterDialog(false, characterExpression.sad, "Andaikan saja itu akhir dari perang Padri.", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Ini tipu muslihat mereka lagi ya?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Mereka memanfaatkan perdamaian tersebut untuk menduduki daerah-daerah lain.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Kamu tau Tuanku Imam Bonjol kan, pahlawan dari kaum Padri yang sudah dijadikan Pahlawan Nasional? Ia sudah setuju dengan perjanjian ini, tetapi Tuanku Mensiangan menolak dan melakukan perlawanan meski dipaksa.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Akhirnya, pusat pertahanannya dibakar dan Tuanku Mensiangan sendiri ditangkap.", null),
                            new MainCharacterDialog(true, characterExpression.think, "Perjanjiannya berarti batal dong?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Tentu saja, lalu Tuanku Imam Bonjol kembali melawan Belanda.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Selanjutnya adalah fase kedua dari perang Padri.", null),
                            new MainCharacterDialog(true, characterExpression.think, "Tunggu, tapi bagaimana anda bisa mengetahui semua ini? Dan jika anda tau, mengapa tidak langsung memberitahuku saja jadi aku tidak perlu capek mencari dokumen itu?", null),
                            new MainCharacterDialog(false, characterExpression.think, "Itu sih karena kamu yang sok-sok-an mau mencari dokumennya, jadi aku biarkan saja kamu capek sendiri akhirnya kan...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Sudah sebaiknya kita segera bergerak lagi, perjalanan kita menemukan pintu keluar dunia ini masih jauh", null),
                        }, false, null)
                    }, true)
                }),

                new Mission(new List<Goal>
                {

                        new ExplorationGoal("Berbicara pada Pasukan Kaum Padri", 1, new string[] {"Pasukan Kaum Padri"}, new int[] {-1}, new Story[] {
                        new Story("Ada seorang pasukan yang menghalangi jalanmu dan Yudha", new List<Dialogs>
                        {
                            new NPCDialog("Pasukan", "Berhenti, kalian terlihat mencurigakan..", null),
                            new NPCDialog("Pasukan", "Kalian bukan orang Belanda kan?", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Bukan kok..", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Kami kenal kok dengan Tuanku Lintau..", null),
                            new NPCDialog("Pasukan", "Oh, kamu yang sudah membantu tuan ya waktu itu..", null),
                            new NPCDialog("Pasukan", "Kalau tidak salah, namamu Player dan tuan disana itu Yu.. Yudhis?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Yudha, bukan Yudhis.", null),
                            new NPCDialog("Pasukan", "Oh ya, Yudha..", null),
                            new NPCDialog("Pasukan", "Aku masih tidak terlalu yakin dengan kalian", null),
                            new NPCDialog("Pasukan", "Jadi, aku akan menguji kalian dulu dengan pertanyaan-pertanyaan ini!", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "(Ya Tuhan..kenapa dia membahasakannya seakan-akan ia akan menyerang kami, tapi menggunakan pertanyaan)", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Apa kamu mau aku yang membantumu menjawab, anak kecil?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Tidak perlu, aku pasti bisa menjawab pertanyaannya", null),
                        }, false, "Judgement")
                    }, true)
                }),

 
                new Mission(new List<Goal>
                {
                    new JudgementGoal("Menyelesaikan tantangan Judgement dari Pasukan Kaum Padri", 15, "Pasukan Kaum Padri", true)
                }),

                  new Mission(new List<Goal>
                {
                    new ReviewGoal("Mereview kembali beberapa kejadian bersama Pasukan Kaum Padri", 1, "Pasukan Kaum Padri", new Story[]{
                        new Story("Pasukan Kaum Padri selesai memberikan review singkat...", new List<Dialogs>
                        {
                               new NPCDialog("Pasukan Kaum Padri", "Aku sudah coba jelaskan secara singkat...", null),
                               new NPCDialog("Pasukan Kaum Padri", "Seharusnya sekarang kamu bisa menjawab sedikit pertanyaan dariku", null),
                              new MainCharacterDialog(true, characterExpression.hurt, "Astaga...", null)
                        }, false, null)
                    }),
                    new JudgementGoal("Menyelesaikan tantangan Judgement dari Pasukan Kaum Padri", 5, "Pasukan Kaum Padri", false)
                }),

                 new Mission(new List<Goal>
                {
                     new ExplorationGoal("Melapor ke Kepala Bagian Pasukan Padri", 1, new string[] {"Kepala Pasukan Padri"}, new int[] {-1}, new Story[]{
                        new Story("Karena gagal, kamu dan Yudha dibawa menghadap Kepala Bagian dari Pasukan Padri", new List<Dialogs>
        {
        new NPCDialog("Kepala Bagian", "...jadi mereka ini orang mencurigakannya?", null),
        new MainCharacterDialog(true, characterExpression.shook, "Kami tidak mencurigakan, pak...lagian kami hanya menumpang lewat tadi..", null),
        new NPCDialog("Pasukan Kaum Padri", "Mereka mengaku sebagai orang yang pernah membantu Pasukan Tuanku Lintau, tapi ketika aku berikan pertanyaan gagal menjawabnya, pak!", null),
        new NPCDialog("Kepala Bagian", "Hmmm...Tuanku Lintau memang pernah bercerita mengenai kalian kepada saya, sih...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Kalau begitu bisakah bapak membantuku untuk meyakinkan bawahan Anda ini?", null),
        new NPCDialog("Kepala Bagian", "Baiklah, saya bisa coba...", null),
        new NPCDialog("Kepala Bagian", "Tapi setelah kamu berhasil menjawab pertanyaan-pertanyaannya lagi, jadi sebaiknya kamu belajar dulu sembri saya menyakinkannya", null),
        new MainCharacterDialog(true, characterExpression.shook, "(Tidak anak buah, tidak kepalanya, sama saja!!!!)", null),
        new MainCharacterDialog(false, characterExpression.think, "Baiklah, saatnya kita mengulang pelajaran, dek Player..", null),

        }, false, null)
                    }, true),
                    new ReviewGoal("Mereview kembali beberapa kejadian bersama Yudha", 1, "Yudha", new Story[]{
                        new Story("Yudha selesai memberikan review singkat...", new List<Dialogs>{
                               new NPCDialog("Kepala Pasukan Padri", "Apa kamu sudah lebih paham?", null),
                               new NPCDialog("Kepala Pasukan Padri", "Kalau kamu sudah siap nantinya, temui saja bawahan saya kembali", null),
                               new MainCharacterDialog(false, characterExpression.think, "Semoga berhasil ya!", null),
                               new MainCharacterDialog(true, characterExpression.hurt, "Baiklah aku rasa kembali ke penghakiman", new string[]{"Kepala Pasukan Padri"}),
                        }, false, null)
                    })
                }),

                new Mission(new List<Goal>{
                     new JudgementGoal("Menyelesaikan tantangan Judgement dari Pasukan Kaum Padri", 5, "Pasukan Kaum Padri", false)
                }),
                new Mission(new List<Goal>
                {
                    new GatherGoal("Berbicara dengan Pasukan Kaum Padri", 1, new string[] {"Pasukan Kaum Padri"}, new Story[]{
                        new Story("Setelah perjuangan cukup panjang menjawab semua pertanyaan Pasukan Kaum Padri, sepertinya kamu sudah mulai mendapat kepercayaannya", new List<Dialogs>
        {
        new NPCDialog("Pasukan Kaum Padri", "Baiklah...aku rasa karena kamu sudah membuktikan dirimu dan paham betul situasi disini...", new string[]{"Yudha (4)"}),
        new NPCDialog("Pasukan Kaum Padri", "Aku akan membantu mengantarmu sampai keluar dari wilayah perang", null),
        new NPCDialog("Pasukan Kaum Padri", "Oh iya, mungkin kalian tau apa yang bisa dilakukan dengan benda ini...", null),
        new MainCharacterDialog(true, characterExpression.happy, "Eh, sebuah pen? Ini sangat antik!", null),
        new MainCharacterDialog(false, characterExpression.neutral, "...sebaiknya kita bergegas, aku akan pergi duluan kalau kamu masih mau mengagumi pena itu", null),
        new MainCharacterDialog(true, characterExpression.shook, "Hei, kak Yudha, tunggu!", new string[]{"Pasukan Kaum Padri", "Yudha (2)", "Yudha (4)"}),

        }, false, null)
                    }, true)
                }),

        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Pergi ke Padang", 1, new string[] {"Yudha (2)"}, new int[] {1}, new Story[] {
                        new Story("Kamu mengikuti Yudha pergi ke Padang. Awalnya pasukan tadi memang mengantarmu, tapi karena waktu berjalan secara berbeda antara kalian, pasukan itu akhirnya tidak lagi di belakang kalian.", new List<Dialogs>
                        { 
                            new MainCharacterDialog(false, characterExpression.neutral, "Disini sudah lebih tenang ya..", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Loh, katanya masih perang?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Makanya tadi kita dilindungi pasukan itu..", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Bisa dibilang ini masa-masa damai..", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Apakah ini artinya perang sudah mau berakhir?", null),
                            new MainCharacterDialog(false, characterExpression.happy, "Sudah dong!", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Di dalam mimpi.", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "....", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Ehem...mereka sekarang sedang fokus ke perang Diponegoro.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Jadi, Belanda ingin mengadakan perjanjian damai lagi dengan kaum Padri.", null),
                            new MainCharacterDialog(true, characterExpression.happy, "Tapi kan sebelumnya mereka saja melanggar perjanjian mereka sendiri?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Ya karena itu, tidak ada yang mau menanggapinya.", null),
                            new MainCharacterDialog(true, characterExpression.happy, "Untunglah, mereka tidak jatuh lagi ke lubang yang sama...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Tapi karena Belanda licik, mereka meminta bantuan Sulaiman Aljufri untuk membujuk para pemuka kaum Padri agar bersedia berdamai.", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Sulaiman Aljufri?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Intinya, seorang saudagar keturunan Arab..", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Kembali ke topik, Tuanku Imam Bonjol menolaknya.", null),
                            new MainCharacterDialog(true, characterExpression.happy, "(Untung saja...).", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Tetapi, Tuanku Lintau dan Tuanku Nan Renceh menerima tawaran itu.", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Astaga, mereka jatuh ke lubang yang sama lagi...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Jadi pada tanggal 15 November 1825 ditandatanganilah sebuah perjanjian..", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Yang kamu harus cari sendiri tentunya, karena kamu percaya dokumen itu jatuh dari langit secara random di tanah", null),
                            new MainCharacterDialog(true, characterExpression.angry, "(Mulai lagi...)", new string[]{"Perjanjian Padang"})
                        }, false, "Minang")
                    }, true)
                }),

        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Mencari Perjanjian Padang di sekitar", 1, new string[] {"Perjanjian Padang"}, new int[] {-1}, new Story[] {
                        new Story("Kamu menemukan perjanjian yang dimaksud kak Yudha", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.think, "(Jackpot!)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Perjanjian Padang...)", new string[]{"Perjanjian Padang"}),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Akan aku simpan dulu dan diskusi dengan dia...)", null),
                        }, false, null)
                    }, false)
                }),

        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Kembali ke Yudha di Padang setelah mendapatkan dokumen", 1, new string[] {"Yudha (2)"}, new int[] {1}, new Story[] {
                        new Story("Kamu kembali pada Yudha yang masih di sekitar Padang...", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.neutral, "Ini dokumennya...", null),
                            new MainCharacterDialog(false, characterExpression.think, "Apa dia tergeletak di tanah secara random lagi?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Mhhh hmmm...", null),
                             new MainCharacterDialog(false, characterExpression.neutral, "Hmmmm...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Belanda mengakui kekuasaan pemimpin Padri dan menjamin pelaksanaan sistem agama di daerah mereka masing-masing.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Mereka tidak akan saling menyerang.", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Aku tidak yakin ini akan ditepati...)", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Mereka akan melindungi para pedagang dan orang-orang yang sedang melakukan perjalanan.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Dan terakhir, praktik adu ayam akan dilarang secara bertahap.", null),
                            new MainCharacterDialog(true, characterExpression.happy, "(Akhirnya ayam-ayam itu bisa bebas...)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Oke akan kucatat informasi-informasi ini ke dalam jurnal)", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Hmmmm...instinct detektifku mengatakan bahwa ada diskusi yang terjadi di balik bukit itu saat ini", null),
                            new MainCharacterDialog(true, characterExpression.think, "Bagaimana anda bisa tau?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "....kita bahas nanti saja, kita sebaiknya kesana dulu sekarang", new string[]{"Kaum Adat dan Kaum Padri"}),
                        }, false, null)
                    }, true)
                }),



        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Pergi ke Bukit Kamang", 1, new string[] {"Kaum Adat dan Kaum Padri"}, new int[] {-1}, new Story[] {
                        new Story("Setibanya di Bukit Kamang benar saja, ada banyak warga yang sedang berkumpul disana", new List<Dialogs>
                        {
                            new NPCDialog("Kaum Adat", "Perjanjian yang mereka buat selalu saja dilanggar", new string[]{"Yudha (2)"}),
                            new NPCDialog("Kaum Adat", "Maafkan kami ya kaum Padri.", null),
                            new NPCDialog("Kaum Padri", "Tidak apa, kalian juga korban dari kelicikan Belanda", null),
                            new NPCDialog("Kaum Padri", "Saatnya balas dendam pada Belanda dan bersatu melawan mereka dan bergerak ke pos-pos tentara Belanda.", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Syukurlah mereka sekarang sadar juga dan bersatu", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Setidaknya sekarang mereka sudah tersadarkan.", null),
                            new NPCDialog("Kaum Adat", "Tunggu...aku merasa ada yang menguping pembicaraan kita", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Oh tidak...", null),
                            new NPCDialog("Kaum Padri", "Mereka ini...jangan-jangan mata-mata dari Belanda....", null),
                            new MainCharacterDialog(false, characterExpression.shook, "Hei hei hei, jangan asal menuduh dulu, kami itu-", null),
                            new NPCDialog("Kaum Padri", "Semuanya ini gawat! Sepertinya Belanda mendengar tentang persatuan kita, dan mereka sekarang menyerang Kota Tuo!", null),
                             new NPCDialog("Kaum Adat", "Apa kalian berdua yang melaporkan kami? Semuanya tangkap mereka!", null),
                            new MainCharacterDialog(false, characterExpression.shook, "Player, sebaiknya sekarang kita berpencar, nanti kita bertemu lagi di sekitar bent-", null),
                            new MainCharacterDialog(true, characterExpression.shook, "BENT APA? Aku tidak bisa mendengarmu, kak Yudha!", null),
                            new MainCharacterDialog(false, characterExpression.shook, "Dia ngomong sambil berlari sih, tapi sebaiknya sekarang aku mulai lari juga", null),

  
                        }, false, "War")
                    }, true)
                }),

        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Lari ke tempat aman", 1, new string[] {"Warga 5"}, new int[] {2}, new Story[] {
                        new Story("Kamu berlari dan sampai ke daerah yang kamu kurang familiar, dan aura perang kuat sekali disana...", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.hurt, "Huftt...huft...permisi pak, aku mau bertanya sesuatu.", new string[]{"Kaum Adat dan Kaum Padri"}),
                            new NPCDialog("Warga", "Ada apa nak? ", null),
                            new NPCDialog("Warga", "Kamu kelihatan tergesa-gesa sekali", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Sekarang sudah tahun berapa ya?", null),
                            new NPCDialog("Warga", "Tahun 1833.", null),
                            new NPCDialog("Warga", "Anak kecil sepertimu untuk apa disini?", null),
                            new NPCDialog("Warga", "Di sini sedang terjadi perang, sebaiknya kamu menjauh dari sini.", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Tapi, aku butuh penjelasan mengenai apa yang terjadi..", null),
                            new NPCDialog("Warga", "Kamu penasaran ya nak?", null),
                            new NPCDialog("Warga", "Aneh sekali anak kecil sepertimu tertarik akan hal-hal seperti ini tetapi aku ceritakan saja", null),
                            new MainCharacterDialog(true, characterExpression.happy, "Baik, terima kasih banyak Pak.", null),
                            new NPCDialog("Warga", "Jadi, pasukan kaum Padri bersatu dengan kaum Adat dan mulai bergerak ke pos-pos Belanda", null),
                            new NPCDialog("Warga", "Tindakan ini dijadikan alasan Belanda untuk menyerang Koto Tuo di Ampek Angkek yang dipimpin Gillavary.", null),
                            new MainCharacterDialog(true, characterExpression.angry, "(Belanda itu.. Padahal mereka yang mulai duluan...)", null),
                            new NPCDialog("Warga", "Pada tahun 1831 Gillavary digantikan oleh Jacob Elout", null),
                            new NPCDialog("Warga", "Elout mendapat pesan dari Gubernur Jenderal Van den Bosch agar melaksanakan serangan besar-besaran terhadap kaum Padri", null),
                            new NPCDialog("Warga", "Dia segera mengerahkan pasukannya untuk menguasai Manggung, Naras, dan Batipuh.", null),
                            new NPCDialog("Warga", "Setelah itu, mereka mau menguasai Benteng Marapalam yang ada di Lintau.", null),
                            new NPCDialog("Warga", "Akibat dua orang kaum Padri yang berkhianat, maka pada Agustus 1831 Belanda dapat menguasai Benteng Marapalam", null),
                            new MainCharacterDialog(true, characterExpression.happy, "(Astaga, dua orang tersebut kenapa mau saja diperalat Belanda..)", null),
                            new NPCDialog("Warga", "Dengan datangnya bantuan pasukan dari Jawa pada tahun 1832 maka Belanda semakin ofensif pada kaum Padri di berbagai daerah.", null),
                            new NPCDialog("Warga", "Tahun 1833 kekuatan Belanda sudah begitu besar sehingga mereka melakukan peyerangan terhadap pos-pos pertahanan kaum Padri di Banuhampu, Kamang, Guguk Sigandang, Tanjung Alam, Sungai Puar.", null),
                            new NPCDialog("Warga", "Hanya informasi itu saja yang bisa aku berikan padamu.", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Seperti itu saja sudah cukup, Pak.", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Terimakasih banyak Pak.", null),
                            new NPCDialog("Warga", "Sama-sama anak kecil yang misterius.", new string[]{"Pejabat Belanda (2)"}),
                        }, false, "Minang")
                    }, true)
                }),

        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Melihat-lihat daerah sekitar", 1, new string[] {"Pejabat Belanda (2)"}, new int[] {1}, new Story[] {
                        new Story("Kamu melihat ada pemerintah Belanda dan pasukannya dan memutuskan untuk mendengarkan dengan bersembunyi di rumah terdekat...", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.shook, "(Eh, ada pemerintah Belanda dan pasukannya)", null),
                            new MainCharacterDialog(true, characterExpression.shook, "(Aku harus bersembunyi)", null),
                            new NPCDialog("???", "Tch, we need to make a promise of service, we can't fight the Padri and Adat together like this (Ck, kita harus membuat sebuah janji khidmat, kita tidak bisa melawan kaum Padri dan Adat bersama-sama seperti ini)", null),
                            new NPCDialog("???", "Even after I attacked them with all the powers I have since I replace that Ellout guy...(Bahkan setelah aku menyerang mereka dengan kekuatan yang aku punya setelah menggantikan Ellout itu...)", null),
                            new NPCDialog("???", "Guards, announce a rule, so there is no more war between us and them (Pasukan, umumkan aturan agar tidak ada lagi perang diantara kita).", null),
                            new NPCDialog("???", "After this poster has been signed as instructed by our government, we'll enforce no war between the people of Adat and Padri as well (Setelah plakat ini ditandatangani sesuai anjuran dari pemerintah kita, kita akan mengusahakan tidak akan ada perang antara kaum adat dan padri juga).", null),
                            new NPCDialog("Pasukan Belanda", "Okay, sir Francis (Baik, tuan Francis)..", null),
                            new MainCharacterDialog(true, characterExpression.think, "(Sepertinya Elout sudah digantikan oleh seseorang bernama Francis)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Aku akan diam-diam mengikuti pasukan itu..)", new string[]{"Pejabat Belanda (2)"}),
                        }, false, null)
                    }, true)
                }),

        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Pergi mengikuti pasukan Belanda", 1, new string[] {"Belanda Soldier (7)"}, new int[] {1}, new Story[] {
                        new Story("Kamu mengikuti pasukan Belanda tersebut dan terlihat seorang Kaum Padri dengannya...", new List<Dialogs>
                        {
                            new NPCDialog("Pasukan Belanda", "Plakat Panjang ini akan menjadi janji khidmat antara Belanda dan para nagari, bahwa tidak akan ada perang antara kita lagi", null),
                            new NPCDialog("Kaum Padri", "Hmmm...baiklah aku akan mempertimbangkan ajakan damai kalian...", null),
                            new MainCharacterDialog(true, characterExpression.shook, "(Astaga kenapa mereka menerimanya!?)", new string[]{"Belanda Soldier (7)"}),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Aku hanya berharap Belanda tidak melanggarnya lagi...)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Sebaiknya aku kembali mencari kak Yudha sekarang, mungkin dia ada di sektaran benteng itu...)", new string[]{"Yudha Bonjol"}),
                        }, false, null)
                    }, true)
                }),

        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Pergi ke Benteng Bonjol", 1, new string[] {"Yudha Bonjol"}, new int[] {2}, new Story[] {
                        new Story("Kamu ke Benteng Bonjol dan ternyata...", new List<Dialogs>
                        {
                            new MainCharacterDialog(false, characterExpression.neutral, "Player, disitu kamu rupanya!", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Kak Yudha, ini gawat! Orang-orang padri itu...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Hei-hei, tenang dulu....kita belum bebas dari wilayah perang sekarang", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Kamu bisa lihat pasukan Imam Bonjol yang sedang diserang oleh Belanda di Bonjol itu", new string[]{"War Zone Bonjol"}),
                            new MainCharacterDialog(false, characterExpression.neutral, "Dilihat dari keadaan disini, sepertinya kita di tahun 1835..", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Tapi apakah Imam Bonjol sudah menerima janji damai dari Belanda disini?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Tidak, dia tetap berjuang, Belanda tidak menjawab syarat dari Tuanku Imam Bonjol untuk membebaskan rakyatnya dan nagari tidak diduduki Belanda dan malah semakin mengepung pertahanan mereka...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Ia bekerja sama dengan pasukannya untuk mempertahankan benteng Bonjol.", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Meskipun satu per satu pemimpin Padri dapat ditangkap, ia tetap terus berjuang.", null),
                            new MainCharacterDialog(true, characterExpression.think, "(Imam Bonjol pantang menyerah sekali.. Meskipun sudah terpojok, dia tetap berjuang untuk kaumnya..)", null),
                            new MainCharacterDialog(true, characterExpression.think, "(Tidak mudah untuk pantang menyerah di situasi yang genting seperti itu...)", null),
                            new MainCharacterDialog(true, characterExpression.sad, "Apakah kita tidak bisa berbuat sesuatu?", null),
                            new MainCharacterDialog(true, characterExpression.sad, "Benteng ini mulai dikepung oleh Belanda", null),
                            new MainCharacterDialog(false, characterExpression.sad, "Percayalah Player, aku juga ingin mereka menang tapi kita tak boleh mengubah masa lalu", null),
                            new NPCDialog("Tuanku Imam Bonjol", "Semuanya lari!", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Tuan, ayo lari ke arah sini!", null),
                            new NPCDialog("Tuanku Imam Bonjol", "Baik, terima kasih tuan...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Kak Yudha...membantunya?", new string[]{"War Zone Bonjol"}),
                            new MainCharacterDialog(false, characterExpression.neutral, "Karena di sejarah mau aku bantu atau tidak dia juga akan bisa meloloskan diri kok..", null),
                            new MainCharacterDialog(true, characterExpression.happy, "(Untung Imam Bonjol dan yang lainnya berhasil meloloskan diri)", null),
                              new MainCharacterDialog(false, characterExpression.neutral, "Sekarang sebaiknya kita juga menjauh dari tempat ini, ayo!", new string[]{"Tuanku Imam Bonjol (1)", "Pejabat Belanda (3)", "Yudha Bonjol"}),
                        }, false, "War")
                    }, true)
                }),

        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Pergi ke sekitar Benteng Bonjol", 1, new string[] {"Tuanku Imam Bonjol (1)"}, new int[] {-1}, new Story[] {
                        new Story("Langkah kalian terhenti ketika melihat Tuanku Imam Bonjol bersama dengan orang Belanda yang kamu lihat tadi", new List<Dialogs>
                        {
                            new NPCDialog("Francis", "Bagaimana kalau kita berdamai saja?", null),
                            new NPCDialog("Francis", "Toh benteng dekat perbukitan milikmu sudah jatuh ke tangan kami", null),
                            new NPCDialog("Tuanku Imam Bonjol", "....sudah tidak ada pilihan lain lagi...", null),
                            new NPCDialog("Tuanku Imam Bonjol", "Baiklah, demi wargaku, aku terima janji perdamaian kalian...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Yah, saat ini Tuanku Imam Bonjol tidak ada pilihan lain sih selain menerima perdamaian itu)", null),
                            new MainCharacterDialog(false, characterExpression.shook, "Hei, Player, mari kita cari tempat lain untuk berbicara, mungkin kita bisa mencari info lain juga jika kita membiarkan waktu kembali berjalan", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "...baiklah, kak Yudha", null),
                        }, false, null)
                    }, true)
                }),

        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Pergi ke Gerbang Minangkabau", 1, new string[] {"Warga 6"}, new int[] {1}, new Story[] {
                        new Story("Kamu pergi bersama kak Yudha kembali ke tempat kalian mulai di dekat gerbang Minangkabau...tapi saat kamu sampai disana ia malah menghilang", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.neutral, "(Kemana lagi detektif itu pergi...ya sudahlah, aku mencari informasi sendiri dulu)", new string[]{"Tuanku Imam Bonjol (1)", "Pejabat Belanda (3)"}),
                            new MainCharacterDialog(true, characterExpression.neutral, "Permisi Pak, aku mau tanya", null),
                            new NPCDialog("Warga", "Iya nak, ada apa?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Imam Bonjol ada dimana sekarang?", null),
                            new NPCDialog("Warga", "...", null),
                            new NPCDialog("Warga", "Dia ditangkap oleh Belanda di tempat perundingan damai pada 28 Oktober 1837.", null),
                            new NPCDialog("Warga", "Kemudian, dia dibuang ke Cianjur, lalu dipindahkan ke Ambon, dan dipindahkan lagi ke Manado sampai ia meninggal pada November 1864.", null),
                            new NPCDialog("Warga", "Pengikutnya sempat bergerilya di hutan-hutan Minangkabau tapi tidak membuahkan hasil", null),
                            new NPCDialog("Warga", "Daerah ini sekarang sudah menjadi milik Belanda...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Pantas saja suasananya sudah berbeda", null),
                            new MainCharacterDialog(true, characterExpression.sad, "Aku tidak mau akhir yang seperti ini, tapi sudah terlambat untukku mengubah segalanya...", null),
                            new MainCharacterDialog(true, characterExpression.sad, "Apa aku yang terlalu merasa terikat dengan dunia ini, padahal aku tidak tau dunia apa ini...", null),
                            new MainCharacterDialog(true, characterExpression.sad, "Tapi melihat sejarah secara langsung memang berbeda dengan hanya mempelajarinya di sekolah, aku turut merasakan semangat dan perjuangan mereka", null),
                            new MainCharacterDialog(true, characterExpression.sad, "Setidaknya aku dapat mengenang semangat mereka itu...", new string[]{"Yudha", "Putri", "Paman Putri"}),
                          
                        }, false, "Judgement")
                    }, true)
                }),

        new Mission(new List<Goal>
                {

                        new ExplorationGoal("Berbicara dengan Yudha dan perempuan cantik", 1, new string[] {"Putri"}, new int[] {-1}, new Story[] {
                        new Story("Kamu melihat Yudha sedang bersama seorang wanita cantik dan seorang bapak-bapak", new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.hurt, "(Yeee...dia malah disini menggoda cewek lagi...)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Kenapa Kak Yudha tiba-tiba seakan malu-malu seperti itu? Ah jelas, dia sedang menggoda cewek itu...)", null),
                            new NPCDialog("Putri", "Aku Putri... orang tuaku mengungsikanku ke rumah pamanku ketika perang itu terjadi", null),
                            new NPCDialog("Putri", "Tapi bisakah kakak ini memberitahuku tentang apa yang terjadi?", null),
                            new MainCharacterDialog(false, characterExpression.hurt, "Ahaha...bagaiamana ya...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Ehem....", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Oh, Player, ahaha...kamu menemukan saya...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Kak Yudha tidak bisa terus meninggalkanku untuk menggoda cewek, tau!", null),
                            new MainCharacterDialog(false, characterExpression.hurt, "Hei jangan bilang begitu...", null),
                            new NPCDialog("Paman Putri", "Putri, sana kembali ke rumah dulu.", null),
                            new NPCDialog("Putri", "Tapi...", null),
                            new NPCDialog("Paman Putri", "Nanti paman tanyakan tentang semuanya ke adik dan kakak ini lalu nanti paman cerita ke Putri...", null),
                            new NPCDialog("Putri", "Janji ya?", null),
                            new NPCDialog("Paman Putri", "Iya janji...", null),
                            new NPCDialog("Putri", "Baiklah kalau begitu...", null),
                            new MainCharacterDialog(false, characterExpression.sad, "Yah dia pergi..", new string[]{"Putri"}),
                            new MainCharacterDialog(true, characterExpression.neutral, "Apa anda mengatakan sesuatu?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Tidak kok, ngomong-ngomong, bapak mau bertanya apa pada kami?", null),
                            new NPCDialog("Paman Putri", "Jadi apakah kalian tidak keberatan kalau kita berbincang sebentar...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Ya sebenarnya kami sedang sibuk sih...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Kak Yudha...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Baiklah... aku rasa kami punya waktu sedikit", null),
                            new NPCDialog("Paman Putri", "Terimakasih, aku tidak akan lama, aku hanya ingin tau tentang hal-hal yang terjadi di perang itu...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Aku tidak yakin memberitahu mereka kenyataan perang itu ide yang bagus)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Pilihan ini tapi pasti akan mempengaruhi perjalananku kedepan, sebaiknya aku berhati-hati, apakah aku mau memberitahu mereka yang sebenarnya atau tidak...)", null),
                        }, false, null)
                    }, true)
                }),
        //edit later
                new Mission(new List<Goal>
                {
                    new JudgementGoal("Menjawab Pertanyaan Paman Putri", 15, "Paman Putri", true)
                }),

                  new Mission(new List<Goal>
                {
                    new ReviewGoal("Mereview kembali beberapa kejadian bersama Yudha", 1, "Yudha", new Story[]{
                        new Story("Yudha selesai memberikan review singkat...", new List<Dialogs>
                        {
                              new NPCDialog("Paman Putri", "Baiklah, apakah adik sudah siap untuk saya tanyai kembali?", null),
                              new MainCharacterDialog(true, characterExpression.neutral, "Tentu...", null)
                        }, false, null)
                    }),
                    new JudgementGoal("Menjawab Pertanyaan Paman Putri", 5, "Paman Putri", false)
                }),

                 new Mission(new List<Goal>
                {
                     new ExplorationGoal("Berbicara dengan Yudha", 1, new string[] {"Yudha"}, new int[] {-1}, new Story[]{
                        new Story("Yudha menarikmu kesamping dengan tatapan tidak enak di wajahnya", new List<Dialogs>
        {
        new MainCharacterDialog(false, characterExpression.shook, "Hei, kamu ini apa sengaja tidak memberitahu yang sebenarnya pada paman itu?", null),
        new MainCharacterDialog(true, characterExpression.hurt, "Ti...tidak kok! Aku tidak sengaja melakukan itu!", null),
        new MainCharacterDialog(false, characterExpression.angry, "Maaf tapi aku tidak percaya padamu, kita coba buktikan dengan aku melakukan review singkat untukmu", null),
        new MainCharacterDialog(false, characterExpression.angry, "Setelah kita selesai kamu akan kembali ke paman itu dan menyeritakan yang sebenarnya, mengerti?", null),
         new MainCharacterDialog(true, characterExpression.hurt, "....baiklah...", null),
        

        }, false, null)
                    }, true),
                   new ReviewGoal("Mereview kembali beberapa kejadian bersama Yudha", 1, "Yudha", new Story[]{
                        new Story("Yudha selesai memberikan review singkat...", new List<Dialogs>
                        {
                              new NPCDialog("Paman Putri", "Baiklah, apakah adik sudah siap untuk saya tanyai kembali?", null),
                              new MainCharacterDialog(true, characterExpression.neutral, "Tentu...", null)
                        }, false, null)
                    }),
                    new JudgementGoal("Menjawab Pertanyaan Paman Putri", 5, "Paman Putri", false)
                }),

                new Mission(new List<Goal>
                {
                     new GatherGoal("Berbicara dengan Paman Putri", 1, new string[] {"Paman Putri"}, new Story[]{
                        new Story("Setelah perbincangan singkat, sudah saatnya kamu dan Yudha berpamitan dengan Paman Putri", new List<Dialogs>
               {
               new NPCDialog("Paman Putri", "Saya mengerti...jadi kalian berdua ini sedang melakukan penyelidikan dan tidak sengaja ikut campur dalam perang yang ada", null),
               new NPCDialog("Paman Putri", "Saya berterimakasih pada kalian karena sudah mau menceritakan semuanya pada saya, selanjutnya kalian akan kemana?", null),
               new MainCharacterDialog(false, characterExpression.angry, "Yang jelas kami akan keluar dari sini karena penyelidikan kami sudah cukup disini, tapi masalah kemana...", null),
               new NPCDialog("Paman Putri", "Bagaimana kalau kalian menumpang di rumah saya sementara sampai kalian tau mau akan kemana setelah ini?", null),
               new MainCharacterDialog(false, characterExpression.neutral, "Apakah tidak merepotkan pak?", null),
               new NPCDialog("Paman Putri", "Tidak kok, Putri juga akan senang karena memiliki teman disini", null),
               new MainCharacterDialog(false, characterExpression.neutral, "Baiklah kalau begitu, terimakasih pak", null),
               new MainCharacterDialog(false, characterExpression.neutral, "Kalau boleh apakah kami boleh ijin untuk berbicara berdua dulu untuk sekarang?", null),
               new NPCDialog("Paman Putri", "Tentu silahkan lanjutkan untuk pembicaraan kalian", null),
               new MainCharacterDialog(false, characterExpression.neutral, "Baiklah, Player, sekarang saatnya kita berdiskusi tentang hasil penyelidikan kita...", null),
               new MainCharacterDialog(true, characterExpression.neutral, "Aku mendengarkan...", null),
               new MainCharacterDialog(false, characterExpression.neutral, "Sebelumnya aku ingin kamu menyimpan ini...", null),
               new MainCharacterDialog(true, characterExpression.neutral, "Ini apa?", null),
               new MainCharacterDialog(false, characterExpression.angry, "Aku menemukannya ketika bersama dengan Putri tadi disakunya, entah milik siapa jam ini", null),
               new MainCharacterDialog(true, characterExpression.shook, "Tapi di masa ini sepertinya kemungkinan kecil seseorang memiliki jam kantong seperti ini, kecuali...", null),
               new MainCharacterDialog(false, characterExpression.angry, "Mereka ada hubungan dengan orang Belanda atau...", null),
               new MainCharacterDialog(false, characterExpression.angry, "Mereka juga terperangkap di dunia ini seperti kita...", null),
               new MainCharacterDialog(false, characterExpression.angry, "Besok aku akan mencoba memastikan apakah Putri masuk satu diantara kategori ini, tapi sebaiknya sekarang kita beristirahat", null),
               new MainCharacterDialog(true, characterExpression.neutral, "Tentu...", null),
               new MainCharacterDialog(true, characterExpression.neutral, "(Sepertinya misteri dari dunia ini semakin meluas....)", null),
               new MainCharacterDialog(true, characterExpression.neutral, "(Tapi aku tidak boleh menyerah, karena sekarang...)", null),
               new MainCharacterDialog(false, characterExpression.neutral, "...", null),
               new MainCharacterDialog(true, characterExpression.happy, "(Aku tidak berjuang sendiri)", null),
               new CutsceneDialog("cutscene_ending", "Bersambung....", null),
               new CutsceneDialog("cutscene_ending", "Terimakasih sudah memainkan Nusatoria! Jangan lupa untuk selalu mengenang sejarah para pahlawan dan belajar nilai-nilai dari sana!", null)

               }, true, "Judgement")}, true)}),

               new Mission(new List<Goal>
                {
                     new ExplorationGoal("Berbicara dengan Paman Putri", 1, new string[] {"Paman Putri"}, new int[] {-1}, new Story[]{
                        new Story("Setelah perbincangan singkat, sudah saatnya kamu dan Yudha berpamitan dengan Paman Putri", new List<Dialogs>
               {
               new NPCDialog("Paman Putri", "Saya mengerti...jadi kalian berdua ini sedang melakukan penyelidikan dan tidak sengaja ikut campur dalam perang yang ada", null),
               new NPCDialog("Paman Putri", "Saya berterimakasih pada kalian karena sudah mau menceritakan semuanya pada saya, selanjutnya kalian akan kemana?", null),
               new MainCharacterDialog(false, characterExpression.angry, "Yang jelas kami akan keluar dari sini karena penyelidikan kami sudah cukup disini, tapi masalah kemana...", null),
               new NPCDialog("Paman Putri", "Bagaimana kalau kalian menumpang di rumah saya sementara sampai kalian tau mau akan kemana setelah ini?", null),
               new MainCharacterDialog(false, characterExpression.neutral, "Apakah tidak merepotkan pak?", null),
               new NPCDialog("Paman Putri", "Tidak kok, Putri juga akan senang karena memiliki teman disini", null),
               new MainCharacterDialog(false, characterExpression.neutral, "Baiklah kalau begitu, terimakasih pak", null),
               new MainCharacterDialog(false, characterExpression.neutral, "Kalau boleh apakah kami boleh ijin untuk berbicara berdua dulu untuk sekarang?", null),
               new NPCDialog("Paman Putri", "Tentu silahkan lanjutkan untuk pembicaraan kalian", null),
               new MainCharacterDialog(false, characterExpression.neutral, "Baiklah, Player, kamu pikir saya tidak menyadari apa yang kamu lakukan", null),
               new MainCharacterDialog(true, characterExpression.neutral, "Memangnya apa yang aku lakukan...", null),
               new MainCharacterDialog(false, characterExpression.angry, "Memang akhirnya kamu koreksi, tapi sebelumnya kamu sengaja kan tidak memberitahu paman Putri yang sebenarnya...", null),
               new MainCharacterDialog(true, characterExpression.sad, "Itu karena aku tidak tega pada mereka kalau-", null),
               new MainCharacterDialog(false, characterExpression.angry, "Itu bukan alasan untuk berbohong pada mereka, Player!", null),
               new MainCharacterDialog(false, characterExpression.angry, "Aku mengerti sekarang, memang kamu tidak mengubah sejarah secara langsung", null),
               new MainCharacterDialog(false, characterExpression.angry, "Tapi dengan membuat kebohongan menjadi sesuatu yang diketahui orang-orang, kebohongan itu menjadi kebenaran dan menggantikan sejarah yang asli", null),
               new MainCharacterDialog(false, characterExpression.angry, "Tapi kenapa aku berekspektasi sesuatu pada anak kecil sepertimu yang tidak tau apa-apa...", null),
               new MainCharacterDialog(false, characterExpression.angry, "Kita beristirahat dulu sekarang, besok kita bangun pagi dan langsung pergi dari sini", null),
               new MainCharacterDialog(true, characterExpression.sad, "....", null),
                    new CutsceneDialog("cutscene_ending", "Bersambung....", null),
               new CutsceneDialog("cutscene_ending", "Terimakasih sudah memainkan Nusatoria! Jangan lupa untuk selalu mengenang sejarah para pahlawan dan belajar nilai-nilai dari sana!", null)

               }, true, "Judgement")}, true)}),

                
        };
        
        }

        return mission;
    }
   
}
