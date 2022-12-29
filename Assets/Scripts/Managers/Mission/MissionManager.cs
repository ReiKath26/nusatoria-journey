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

    [SerializeField] private TextMeshProUGUI reviewText;

    private Review[] toBeReviewed;
    private int reviewCount = 4;
    

   private SaveSlots slot;

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
       if(slot.missionNumber < MissionProvider.instance.allMissions.Count)
       {
          currentMission = MissionProvider.instance.allMissions[slot.missionNumber];
          slot.goalNumber = 0;

          if(currentMission.loadTriggerStory() != null)
          {
               StoryManager.instance.assignStory(currentMission.loadTriggerStory());
          }

          assignGoal();
       }

       else
       {
          StoryManager.instance.assignStory(MissionProvider.instance.getEndChapterStory(slot.chapterNumber));
       }

       SaveHandler.instance.saveSlot(slot, slot.slot);

   }

   private void assignGoal()
   {
     if(slot.goalNumber < currentMission.goals.Count)
     {
         currentGoal = currentMission.goals[slot.goalNumber];
     }

     else
     {
          checkMission();
     }

     SaveHandler.instance.saveSlot(slot, slot.slot);
   
   }

   private void update()
   {
     checkGoal();
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
                   if(slot.chapterNumber == 1)
                   {
                         slot.missionNumber = 23;
                         needReviewType = new questionType[]{questionType.latarBelakang_sa, questionType.seranganSatu_sa, questionType.seranganDua_sa, questionType.akhir_sa};
                   }

                   else
                   {

                   }
               }

               else if(currentUnderstanding == 2)
               {
                   if(slot.chapterNumber == 1)
                   {
                         slot.missionNumber = 20;
                   }

                   else
                   {
                    
                   }
               }

               else 
               {

                   if(slot.chapterNumber == 1)
                   {
                         slot.missionNumber = 25;
                   }

                   else
                   {
                    
                   }
                    slot.goalNumber++;
                    assignGoal();
                    displayGoal();
               }
          }

          else if(currentGoal is AdditionalJudgementGoal add_goal)
          {
               float finalScore = add_goal.getFinalScore();
               int currentUnderstanding = evaluateResult(finalScore);

                if(currentUnderstanding < 2)
                {
                    add_goal.retractComplete();
                    displayGoal();
                }
                
                else
                {
                    if(slot.chapterNumber == 1)
                   {
                       slot.missionNumber = 25;
                   }

                   else
                   {
                    
                   }
                }
          }

          else
          {
               slot.goalNumber++;
               assignGoal();
               displayGoal();
          }

          checkMission();
        
     }
   }

   private void checkMission()
   {
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

               if(interactor == inst[i])
               {
                    exp_goal.OnInteract(i);

                    int[] listOfConcepts = exp_goal.getListOfKeyConcept();
                    if (listOfConcepts[i] != -1)
                    {
                         string getKey = "";

                         int count = listOfConcepts[i];
                         for(int j=0; j < slot.player_glossary.conceptList.Count; j++)
                         {
                              while(count > 0)
                              {
                                   if(slot.player_glossary.conceptList[j].unlocked != true)
                                   {
                                        SaveHandler.instance.unlockKeyConcept(j, PlayerPrefs.GetInt("choosenSlot"));
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
          for(int i = 0; i < gth_goal.getInstances().Length; i++)
          {
               GameObject[] inst = gth_goal.getInstances();
               if(interactor == inst[i])
               {
                    gth_goal.OnGather(i);
                       if(interactor.TryGetComponent(out Item item))
                      {
                         SaveHandler.instance.saveItem(item, PlayerPrefs.GetInt("choosenSlot"));
                         gotItemSprite.GetComponent<LoadSpriteManage>().loadNewSprite(item.itemSprite);
                         gotItemTextShow.text = "x" + item.itemCount + " " + item.itemName;
                         StartCoroutine(showGetNotification());
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

            bool canSubmit = SaveHandler.instance.submitItem(listOfItem, PlayerPrefs.GetInt("choosenSlot"));

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

     else if(currentGoal is AdditionalJudgementGoal add_goal)
     {
          if(interactor == add_goal.getRecipient())
          {
               startJudgement();
          }
     }

     else if(currentGoal is ReviewGoal rev_goal)
     {

     }


   }

   public void submitItemForSubmitGoal()
   {
      if(currentGoal is SubmitGoal sbm_goal)
      {
          sbm_goal.OnSubmit(0);
      }
   }

   public void answerChoice(int number)
   {
      if(currentGoal is JudgementGoal jd_goal )
      {
          Choice[] choices = questionDataList[jd_goal.getCurrentAmount()].getChoices();

          if(!choices[number].correct)
          {
               if(!needReviewType.Contains(questionDataList[jd_goal.getCurrentAmount()].getQuestionType()))
               {
                    needReviewType[needReviewType.Length] = questionDataList[jd_goal.getCurrentAmount()].getQuestionType();
               }
          }
          jd_goal.OnAnswerQuestion(questionDataList[jd_goal.getCurrentAmount()], number, questionDataList.Count);
          nextQuestion();
      }

      else if(currentGoal is AdditionalJudgementGoal add_goal)
      {
           add_goal.OnAnswerQuestion(questionDataList[add_goal.getCurrentAmount()], number, questionDataList.Count);
           nextQuestion();
      }
   }

   public void startJudgement()
   {
       setUpQuestion();
       questionOverlay.SetActive(true);
       needReviewType = new questionType[] {};
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
                    new Choice("Cirebon dan Karawang", true),
                    new Choice("Karawang dan Jepara", false)
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
          if(slot.missionNumber == 13)//edit later
          {
               easyQuestion = new List<Question>();
               mediumQuestion = new List<Question>();
               hardQuestion = new List<Question>();
               //edit later
          }

          else
          {
               easyQuestion = new List<Question>();
               mediumQuestion = new List<Question>();
               hardQuestion = new List<Question>();
               //edit later
          }    
      }

      if(currentGoal is JudgementGoal jd_goal)
      {
          switch(slot.understandingLevel)
          {
               case 0:
               {
                    randomQuestion(5, easyQuestion);
                    randomQuestion(7, mediumQuestion);
                    randomQuestion(3, hardQuestion);
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

      else if(currentGoal is AdditionalJudgementGoal add_goal)
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

   private void randomQuestion(int num, List<Question> question)
   {
          for(int i=0; i<num;i++)
          {
               int lastRoll = -1;
               int index = -1;
                         

               while(index == lastRoll)
               {
                    index = Random.Range(0, question.Count);
               }

               lastRoll = index;
               questionDataList.Add(question[index]);
          }
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

     private void assignReview()
    {
        List<Review> reviews = new List<Review>
        {
          new Review(),
          new Review(),
          new Review(),
          new Review(),
          new Review(),
          new Review(),
          new Review(),
          new Review(),
          new Review(),
          new Review(),
        };

        foreach(questionType type in needReviewType)
        {
           foreach(Review review in reviews)
           {
               if(review.getReviewType() == type)
               {
                    Story rev = review.getReviewContent();
                    reviewTexts[toBeReviewed.Length].text = rev.getTitle();
                    toBeReviewed[toBeReviewed.Length] =  review;

               }
           }
        }


        reviewOverlay.SetActive(true);

        for(int i=0; i<reviewChoice.Length;i++)
        {
          if(i >= toBeReviewed.Length)
          {
               reviewChoice[i].SetActive(false);
          }
        }

    }

    public void OnChooseReview(int number)
    {
        
    }

    public void checkReview()
    {
        if(reviewCount == 0)
        {
            ReviewGoal goal = currentGoal as ReviewGoal;

            goal.OnAllReviewDone();
        }
    }


    IEnumerator showGetNotification()
    {
          getItemNotification.SetActive(true);
          yield return new WaitForSeconds(1f);
          getItemNotification.SetActive(false);
    }

   
}
