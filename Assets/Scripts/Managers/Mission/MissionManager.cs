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

    [SerializeField] private GameObject reviewStoryOverlay;
    [SerializeField] private TextMeshProUGUI reviewText;

    private Review[] toBeReviewed;
    private int reviewCount = 4;
    private int reviewLineCount = 0;
    private int choosenReview = 0;

   private SaveSlots slot;

   public static MissionManager instance;

   void Awake()
   {      
       slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
       instance = this;
       assignMission();
   }

   private void assignMission()
   {
          List<Mission> allMissions = getChapterMissions(slot.chapterNumber);
          if(slot.missionNumber < allMissions.Count)
          {
               currentMission = allMissions[slot.missionNumber];
               slot.goalNumber = 0;

               assignGoal();
               displayGoal();
               setCurrGoal();
          }

          else
          {
               StoryManager.instance.assignStory(getEndChapterStory(slot.chapterNumber));
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
                            slot.missionNumber = 23;
                            needReviewType = new questionType[]{questionType.latarBelakang_sa, questionType.seranganSatu_sa, questionType.seranganDua_sa, questionType.akhir_sa};
                            checkMission();

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
                         checkMission();

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
                         checkMission();

                   }

                   else
                   {
                    
                   }
                    slot.goalNumber++;
                    assignGoal();
                    displayGoal();
                    }
               }

               else
               {
                    if(currentUnderstanding < 2)
                    {
                         jdg_goal.retractComplete();
                         displayGoal();
                    }
                
                    else
                    {
                         if(slot.chapterNumber == 1)
                         {
                              slot.missionNumber = 25;
                              checkMission();

                         }

                         else
                         {
                    
                         }
                    }
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
                    for(int j=0; j<18; j++)
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
                    inst[i].SetActive(false);
                       if(interactor.TryGetComponent(out Interactable interactable))
                      {
                         Item item = interactable.getPocketItem();

                         if(item != null)
                         {
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
                         needReviewType[needReviewType.Length] = questionDataList[jd_goal.getCurrentAmount()].getQuestionType();
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
          if(jd_goal.getMain() == true)
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
           new Review(new Story("Latar Belakang Serangan Sultan Agung", new List<Dialogs>
               {
                    new NPCDialog("Review", "Pada awalnya hubungan Mataram dan Belanda cukup baik", null),
                    new NPCDialog("Review", "Sampai pada tanggal 18 November 1618, jendral VOC saat itu, Jan Pieterszoon Coen", null),
                    new NPCDialog("Review", "Meminta Van den Marct untuk menyerang Jepara, menyebabkan kerugian yang besar bagi Mataram", null),
                    new NPCDialog("Review", "Sejak saat itu hubungan Belanda dan Mataram memburuk", null),
                    new NPCDialog("Review", "Hal ini juga dikarenakan keberadaan Belanda di tanah jawa menghalangi cita-cita dari Sultan Agung", null),
                    new NPCDialog("Review", "Ia bercita-cita untuk menyatukan tanah jawa dan mengusir kekuasaan bangsa asing dari tanah jawa", null)
               }, false), questionType.latarBelakang_sa),
          new Review(new Story("Latar Belakang Serangan Sultan Agung", new List<Dialogs>
               {
                    new NPCDialog("Review", "Dan akhirnya pada Agustus 1628, Mataram melancarkan serangan ke Batavia", null),
                    new NPCDialog("Review", "Pasukan Tumenggung Baureksa sampai ke Batavia terlebih dahulu dan mulai melakukan serangan", null),
                    new NPCDialog("Review", "Selanjutnya pada Oktober 1628, menyusul pasukan dari Tumenggung Sura Agul-Agul, Kiai Dipati Mandurojo dan Upa Santa", null),
                    new NPCDialog("Review", "Setelah perang yang cukup lama, Mataram mengalami kekalahan karena kekurangan perbekalan", null),
                    new NPCDialog("Review", "Juga karena persenjataan Belanda lebih modern", null),
                    new NPCDialog("Review", "Sehingga pada 6 Desember 1628, pasukan Mataram mundur", null)
               }, false), questionType.seranganSatu_sa),
          new Review(new Story("Latar Belakang Serangan Sultan Agung", new List<Dialogs>
               {
                    new NPCDialog("Review", "Setelah kekalahan di serangan pertama, Mataram mulai mempersiapkan lebih matang untuk serangan kedua", null),
                    new NPCDialog("Review", "Sebelum penyerangan untuk menyebabkan perbekalan lebih, mereka membangun lumbung di Karawang dan Cirebon", null),
                    new NPCDialog("Review", "Pada 1629, mereka pun melancarkan serangan lagi ke Batavia", null),
                    new NPCDialog("Review", "Akan tetapi karena adanya wabah kolera dan malaria", null),
                    new NPCDialog("Review", "Juga karena Belanda menghancurkan lumbung yang mereka bangun", null),
                    new NPCDialog("Review", "Akhirnya serangan ini juga mengalami kegagalan", null)
               }, false), questionType.seranganDua_sa),
          new Review(new Story("Latar Belakang Serangan Sultan Agung", new List<Dialogs>
               {
                    new NPCDialog("Review", "Namun setelah 2 kegagalan pun Sultan Agung tidak menyerah untuk menyerang Batavia dan mengusir VOC", null),
                    new NPCDialog("Review", "Sayangnya sepeninggal Sultan Agung di tahun 1945, Mataram mengalami kemunduran", null),
                    new NPCDialog("Review", "Dan ini membuka peluang untuk Belanda menguasai Mataram", null)
               }, false), questionType.akhir_sa)
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
        reviewStoryOverlay.SetActive(true);
        reviewText.text = "Jadi yang mana yang ingin kamu pelajari terlebih dahulu?";
        reviewCount = toBeReviewed.Length;

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
            goal.OnAllReviewDone();
        }
    }

    public void nextReview()
    {
        
        Story rev = toBeReviewed[choosenReview].getReviewContent();
        StartCoroutine(TypeLine(rev.dialogs[reviewLineCount].getDialog()));
        reviewLineCount++;
        rev.checkDialogs();
        OnCheckReview(rev);

    }

    public void OnCheckReview(Story story)
    {
          if(story.getCompleted())
          {
               reviewLineCount = 0;
               reviewOverlay.SetActive(true);
               reviewText.text = "Jadi yang mana yang ingin kamu pelajari terlebih dahulu?";
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
                            new MainCharacterDialog(true, characterExpression.neutral, "(Eh? Apa orang itu berbicara padaku?)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Aneh...jangan-jangan aku benar-benar terlempar ke masa lalu)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Tapi sebaiknya aku menanggapinya agar tidak tampak mencurigakan)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Iya...pak?", null),
                            new NPCDialog("???", "Hahaha, baru kali ini saya dipanggil pak oleh warga saya", null),
                            new NPCDialog("???", "Atau jangan-jangan kamu orang baru disini", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Bisa...dibilang begitu?", null),
                            new NPCDialog("???", "Oh kalau begitu, selamat datang di Mataram!", null),
                            new NPCDialog("???", "Saya harap saya dapat menjamu tamu seperti kamu dengan lebih baik", null),
                            new NPCDialog("???", "Tapi kami ditengah situasi cukup genting", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Situasi cukup genting?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Oh iya, tapi saya belum tau siapa bapak", null),
                            new NPCDialog("???", "Ah iya benar, saya belum memperkenalkan diri", null),
                            new NPCDialog("Sultan Agung", "Saya Sultan Agung, raja dari Kerajaan Mataram ini", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Raja?!)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Aduh, maafkan ketidaksopanan saya, paduka raja", null),
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
                            new MainCharacterDialog(true, characterExpression.neutral, "Tapi bukannya itu jauh sekali dari sini...pasukan anda akan menempuh perjalanan yang cukup panjang untuk itu...", null),
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

                        }, false)})}), 

                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi Mengintai ke Kota Batavia", 1, new string[] {"Batavia Fort Gate"}, new int[] {-1}, new Story[] {
                        new Story("Setelah perjalanan cukup panjang, kamu sampai di Batavia...", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.neutral, "Huftt...hufftt...", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Lelah sekali...untungnya jarak Batavia dan Mataram tidak sebegitu jauh seperti Jakarta dan Jawa Tengah di masaku...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Tapi apakah memang seharusnya begitu?)", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Memang ada sesuatu yang aneh tentang tempat ini...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Sebaiknya aku mencari petunjuk di kota ini untuk aku laporkan kepada Sultan Agung)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Dan siapa tau juga aku bisa menemukan petunjuk tentang kemana mesin aneh tadi itu membawaku)", null),

        }, false)
                    })
                }), 

                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Mencari petunjuk di Batavia", 3, new string[] {"Red Book", "Crates Box", "Belanda Soldier"}, new int[] {1, -1, -1}, new Story[] {
                        new Story("Kamu menemukan sebuah buku merah tergeletak di tanah...", new List<Dialogs>
        {
          new MainCharacterDialog(true, characterExpression.neutral, "(Siapa yang menaruh file-file berserakan seperti ini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Tapi file apa ini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, " (Hmmm....)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Eh kenapa ada info-info terkait J.P. Coen disini?)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Mari kita lihat...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Hmmmm...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Jadi J.P. Coen memerintah dari 1618-1623 dan 1627-sekarang...aku asumsikan 1628...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Ada hal lain kah yang harus aku tau...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Hmmmm...dari data-data disini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Pengeluaran yang dikeluarkan VOC untuk membeli sebanyak ini hasil bumi sangat sedikit)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku tidak perlu belajar akuntansi untuk mengetahui ini aneh...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Mereka pasti mengeksploitasi hasil bumi disini dan angkanya terus meningkat)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Memang serakah orang-orang itu, tapi sebaiknya aku menyimpan info ini untuk aku review kembali nantinya)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku sebaiknya cepat menyelesaikan urusanku disini dan kembali ke Sultan Agung agar tidak dicurigai oleh seluruh pasukan itu...", null),
    

        }, false),
                        new Story("Diujung kota terdapat banyak kotak-kotak barang yang tertata rapi...", new List<Dialogs>
        {
          new MainCharacterDialog(true, characterExpression.neutral, "(Kotak-kotak ini semua hasil bumi...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Tapi terutama rempah-rempah)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Disini dituliskan semuanya akan dikirimkan ke VOC)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Jadi mereka memang menguasai seluruh jual beli disini ya...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Atau itu hasil dari monopoli mereka...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Hmmm...pandangan orang-orang di sini tidak enak sekali padaku, tapi apa hanya itu saja petunjuk yang bisa aku dapat?)", null),

        }, false),
                        new Story("Kamu memperhatikan penjaga yang berjaga di barak secara seksama...", new List<Dialogs>
        {
          new MainCharacterDialog(true, characterExpression.neutral, "(Penjaga-penjaga itu...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Sepertinya mereka membawa senapan yang cukup modern...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku tidak tau senjata semacam itu sudah ada di masa ini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Ini bisa jadi berbahaya jika Sultan Agung tidak mempersiapkan senjata yang setara)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku sebaiknya memberitahunya secepatnya)", null),
        }
, false)
                    })
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
        }, false)
                    })
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
            new MainCharacterDialog(true, characterExpression.neutral, "(Kan tinggal aku masukan ke fitur inventory canggih di smartphoneku dan aku bisa membawa barang-barang itu seakan-akan mereka microchip)", null),
            new NPCDialog("T.Baurekhsa", "Baiklah kalau kamu bersikeras", null),
            new NPCDialog("T.Baurekhsa", "Kami membutuhkan 150 ekor sapi, 5.900 karung gula, 25.000 buah kelapa dan 12.000 karung beras lagi", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Itu banyak sekali??", null),
            new NPCDialog("T.Baurekhsa", "Haha..itu jumlah total semuanya saja, dek Player", null),
            new NPCDialog("T.Baurekhsa", "Kami hanya kurang 15 karung gula, 2 buah kelapa, dan 10 karung beras lagi..", null),
            new NPCDialog("T.Baurekhsa", "Biasanya tumpukan karung gula dan beras ada 5 karung sekaligus jadi kamu cari 3 dan 2 tumpukan masing-masing saja", null),
            new NPCDialog("T.Baurekhsa", "Aku tunggu disini saat kamu sudah mendapatkan semua barang itu", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Baik, pak", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah, saatnya bekerja..", null),
        }, false)
                    })
                }), 

                 new Mission(new List<Goal>
                {
                    new GatherGoal("Mengambil 2 karung beras untuk perbekalan", 2, new string[] {"Rice Sack", "Rice Sack (1)"}, null),
                    new GatherGoal("Mengambil 3 karung gula untuk perbekalan", 3, new string[] {"Sugar sack", "Sugar sack (1)", "Sugar sack (2)"}, null),
                    new GatherGoal("Mengambil 2 buah kelapa untuk perbekalan", 2, new string[] {"Coconut nut", "Coconut nut (1)"}, null)
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
            new MainCharacterDialog(true, characterExpression.neutral, "Kenapa tidak sempat? Aku saja berjalan dari Mataram ke Batavia hanya dalam hitungan menit kok!", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sesuatu yang aneh sih...)", null),
            new NPCDialog("T.Baurekhsa", "ya, tapi waktu sepertinya berjalan secara berbeda untuk orang-orang seperti kamu, dek Player", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Orang seperti aku?", null),
            new NPCDialog("T.Baurekhsa", ".......", null), 
            new NPCDialog("T.Baurekhsa", "Lupakan saja, sebaiknya kamu mulai berjalan ke Mataram sekarang", null),
            new MainCharacterDialog(true, characterExpression.neutral, "......", null),


        }, false)
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
            new NPCDialog("Sultan Agung", "Dek Player, kamu akhirnya sampai...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Akhirnya?", null),
            new NPCDialog("Sultan Agung", " Iya, aku mendapat pesan dari Tumenggung Baurekhsa kalau kamu sedang berjalan kemari sekitar sebulan yang lalu", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Sebulan?! Aku hanya berjalan selama beberapa menit?!", null),
            new NPCDialog("Sultan Agung", "Mungkin karena kamu menikmati keindahan alam dan perjalanan jadi tidak terasa", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Aneh sekali...tadi itu benar-benar tidak terasa seperti satu bulan)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Bahkan aku tidak tau tanggal berapa sekarang di dunia antaberanta ini...)", null),
            new NPCDialog("Sultan Agung", "Kamu baik-baik saja, dek Player?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Sultan, kalau boleh bertanya, sekarang ini bulan dan tahun berapa ya?", null),
            new NPCDialog("Sultan Agung", " Agustus 1628, dek MC...Tumenggung Baurekhsa dan pasukannya baru saja akan berangkat ke Batavia", null),
            new NPCDialog("Sultan Agung", "Dan waktu aku pergi ke Batavia waktu itu...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Berapa lama Sultan menunggu sampai aku kembali kemari?", null),
            new NPCDialog("Sultan Agung", "Hmmm...sekitar 3 atau 4 bulan? Saya lupa sih", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Oke...ini tidak benar...waktu memang berjalan sangat aneh di tempat ini....)", null),
            new NPCDialog("Sultan Agung", "Ngomong-ngomong soal itu, saya ingin mengecek kondisi pasukan di Batavia", null),
            new NPCDialog("Sultan Agung", "Tapi jika saya pergi kesana secara langsung sekarang, nanti akan menimbulkan curiga", null),
            new NPCDialog("Sultan Agung", "Bisakah saya meminta bantuan lagi, dek Player?", null),
            new MainCharacterDialog(true, characterExpression.neutral, " Anda ingin saya...kembali ke Batavia lagi?", null),
            new NPCDialog("Sultan Agung", "Kira-kira begitu, saya tau itu perjalanan yang panjang tapi...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Tidak apa-apa, Sultan, saya akan kesana", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Lebih baik daripada saya berdiam disini tanpa melakukan apapun", null),
            new NPCDialog("Sultan Agung", "Baik kalau begitu, terimakasih dek Player, kembalilah kemari ketika kamu sudah mendapat info terkait perang yang terjadi", null),
            
        }, false)
                    })
                }), 

                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke Batavia untuk memantau jalannya perang", 1, new string[] {"Batavia NPC"}, new int[] {1}, new Story[]{
                        new Story("Dari luar pintu gerbang benteng, terlihat derunya perang antara pasukan Mataram dan Belanda", new List<Dialogs>

        {
            new NPCDialog("Warga", " Hei, kalau saya jadi kamu, saya tidak akan pergi masuk ke benteng", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Memangnya kenapa, pak?", null),
            new NPCDialog("Warga", "Perang besar sedang terjadi disana, warga-warga dari Mataram itu menyerang dan terjadi perang yang sudah berjalan berbulan-bulan sekarang", null),
            new MainCharacterDialog(true, characterExpression.neutral, "....tidak lagi...", null),
            new NPCDialog("Warga", "ya saya setuju, saya juga sudah lelah dengan berbagai perang yang terjadi", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Sekarang ini...memangnya bulan apa, pak?", null),
            new NPCDialog("Warga", "Bulan Desember, tanggal 6, saya rasa perang itu akan mulai mereda beberapa saat lagi...", null),
            new NPCDialog("T. Baurekhsa", " PASUKAN! MUNDUR!", null),
            new MainCharacterDialog(true, characterExpression.neutral, "tu...suara Tumenggung...", null),
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
            new NPCDialog("Warga", "Tapi pada akhirnya karena sepertinya mereka kekurangan perbekalan mereka mengalami kekalahan...", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Aku sudah tau ini akan terjadi)", null),
            new NPCDialog("Warga", "Dan hari ini, akhirnya mereka mundur juga kan...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Jadi setelah perjuangan itu mereka kalah juga ", null),
            new NPCDialog("Warga", " Iya tapi memang kita tidak akan selalu menang dalam hidup...hanya bisa berusaha sekuat yang kita bisa", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Wow...warga ini filosofikal juga...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sebaiknya aku mencatat semua informasi itu dan kembali ke Sultan Agung)", null),

        }, false)
                    })
                }), 
                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Kembali ke Sultan Agung", 1, new string[] {"Sultan Agung"}, new int[] {-1}, new Story[]{
                        new Story("Sultan Agung nampak seperti sedang banyak pikiran, tentu hal ini terkait dengan kekalahan mereka dalam perang", new List<Dialogs>
        {
            new NPCDialog("Sultan Agung", ".......", null),
            new NPCDialog("Sultan Agung", "Oh, dek Player! Saya sudah lama tidak melihatmu, kamu dari mana saja?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Hmmm...aku selalu disini? Mungkin kita tidak pernah saling bertemu lagi saja sejak saat itu", null),
            new NPCDialog("Sultan Agung", "Haha...benar juga...", null),
            new NPCDialog("Sultan Agung", "Tapi ini bukan saat yang tepat lagi sayangnya untuk berbicara dan ramah tamah", null),
            new NPCDialog("Sultan Agung", "Kami akan menyiapkan serangan kembali ke Batavia", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Lagi? Tapi bukannya anda baru saja kalah, akankah lebih baik kalau Mataram memulihkan diri dari perang?", null),
            new NPCDialog("Sultan Agung", "Baru saja? Ini sudah bulan Mei 1629, kami sudah menaruh kekalahan itu di belakang kami", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Tunggu...aku terlempar ke Mei 1629?)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Apa setiap kepalaku menjadi pusing seperti tadi aku terlempar ke masa lain dari sejarah...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Tempat ini benar-benar aneh...jelas ini bukan masa lalu yang sebenarnya)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Aku harus mencari informasi lebih lanjut untuk tau cara keluar dari sini...)", null),
            new NPCDialog("Sultan Agung", "Intinya kami sekarang akan mempersiapkan serangan selanjutnya", null),
            new NPCDialog("Sultan Agung", "Kami sudah belajar dari kekalahan sebelumnya...", null),
            new NPCDialog("Sultan Agung", "Bahwa kami kurang perbekalan saat itu", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Setidaknya mereka belajar dari kesalahan sebelumnya dan mencoba memperbaiki strategi mereka...)", null),
            new NPCDialog("Sultan Agung", "Jadi kami akan mencoba mencari akal untuk membangun lumbung di tempat yang tidak mencurigakan", null),
            new NPCDialog("Sultan Agung", "Untuk perbekalan kami", null),
            new NPCDialog("Sultan Agung", "Kalau saya pikir lagi...", null),
            new NPCDialog("Sultan Agung", "Player, apakah kamu bersedia membantu kami lagi?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Aku rasa tidak masalah karena aku tidak ada rencana apapun", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Tidak sampai aku bisa keluar dari tempat ini)", null),
            new NPCDialog("Sultan Agung", "Hoho bagus...", null),
            new NPCDialog("Sultan Agung", "Saya awalnya ingin mengirim pengintai dari Mataram untuk mencari tempat untuk lumbung itu...", null),
            new NPCDialog("Sultan Agung", "Tapi kamu akan nampak lebih tidak mencurigakan bagi orang-orang Belanda itu", null),
            new NPCDialog("Sultan Agung", "Jadi maukah kamu pergi ke Karawang dan Cirebon untuk menandai tempat yang dapat digunakan untuk membangun lumbung kami?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Aku cukup menandainya saja?", null),
            new NPCDialog("Sultan Agung", "Iya tandai saja tempatnya di petamu dan kirimkan kepada kami setelah kamu selesai", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Oh baiklah, aku rasa aku bisa coba", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Tapi....dimana arah ke Cirebon dan Karawang?", null),
            new NPCDialog("Sultan Agung", " Sini saya akan menandai sekitaran lokasinya dipetamu...dan sekarang kamu siap berangkat!", null),
            new NPCDialog("Sultan Agung", "Terimakasih Player, saya akan menunggu kabar baiknya", null),
            
        }, false)
                    })
                }), 

                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Membantu mencarikan lokasi lumbung di Karawang", 2, new string[] {"Spot Tree 2", "Spot Tree 1"}, new int[] {-1}, new Story[]{
                        new Story("Kamu menemukan lokasi di dekat rumah-rumah warga", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.neutral, "Hmmm...tempat ini tidak cukup untuk membangun lumbung perbekalan", null),
        }, false),
                        new Story("Kamu menemukan lokasi di dekat pantai", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.neutral, "Tempat ini sepertinya cocok, sebaiknya aku menandainya", null),
        }, false)
                    }),
                    new ExplorationGoal("Membantu mencarikan lokasi lumbung di Cirebon", 2, new string[] {"Spot Tree 4", "Spot Tree 3"}, new int[] {-1}, new Story[]{
                        new Story("Kamu menemukan lokasi di pinggir jalan", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.neutral, "Tidak, sebaiknya kita coba cari tempat lain", null),
        }, false),
                        new Story("Kamu menemukan lokasi di balik kawasan rumah warga", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.neutral, "Tempat ini cukup baik, aku akan menandainya", null),
        }, false)
                    })
                }), 

                  new Mission(new List<Goal>
                {
                    new GatherGoal("Mencari pedagang sekitar yang akan ke Mataram", 1, new string[] {"Pedagang Mataram NPC"}, new Story[]{
                        new Story("Kamu menemukan seorang pedagang yang nampak mempersiapkan bawaannya...", new List<Dialogs>
        {
            new NPCDialog("Pedagang", "Ada yang bisa saya bantu nak?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Ah iya pak, apakah bapak akan mengantarkan barang-barang ini ke Mataram?", null),
            new NPCDialog("Pedagang", "Iya nak, saya akan ke Mataram", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Oh kalau begitu, apa saya boleh titip sesuatu untuk Sultan Agung", null),
            new NPCDialog("Pedagang", "Untuk Sultan? Seharusnya bisa ya nak, karena saya langsung bertemu dengannya juga nanti", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah kalau begitu saya titipkan ini kepada bapak ya", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sekarang aku rapikan dulu petaku, lalu setelah memberikannya pada bapak ini, sebaiknya aku pergi ke arah yang berlawananan agar tidak ada yang curiga..)", null)
            
        }, false),
                    }),
                     new SubmitGoal("Berikan peta yang sudah ditandai ke pedagang", 1, "Pedagang Mataram NPC", null, new Item[]{
                        new Item("Map", "Peta yang sudah ditandai", "Peta yang sudah ditandai untuk kebutuhan khusus", 1)
                    })
                }), 

                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke arah yang berlawanan ke Batavia", 1, new string[] {"Batavia NPC"}, new int[] {-1}, new Story[]{
                        new Story("Kamu bertemu kembali dengan warga yang waktu itu...", new List<Dialogs>
        {
             new NPCDialog("Warga", "Ah, kamu lagi...kamu mau ke medan pertempuran itu lagi ya, kamu ini memang suka menantang maut ya....", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Bukan urusan bapak.", null), 
            new NPCDialog("Warga", "Dan kamu mau tau satu hal yang saya dengar...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Dan itu adalah?", null),
            new NPCDialog("Warga", "Hehe... ", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Bapak ini meminta bayaran untuk informasi....)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Tapi aku tidak punya uang dan aku tidak tau apakah informasi itu akan berguna untukku atau tidak...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Bayaran apa yang bapak minta? Saya tidak punya uang karena saya bukan dari sekitar sini", null),
            new NPCDialog("Warga", "Oh begitu rupanya...", null),
            new NPCDialog("Warga", "Hmmm...", null),
            new NPCDialog("Warga", "Ya sudahlah, anggap saja ini dari kebaikan hatiku saja", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Aku dengar akibat dari perang yang sedang berlangsung itu air disana menjadi tercemar", null),
            new NPCDialog("Warga", "Dan akibat dari itu sekarang pemimpin orang-orang Belanda itu siapa namanya...", null),
            new MainCharacterDialog(true, characterExpression.neutral,"J.P. Coen?", null),
            new NPCDialog("Warga", "Ah iya dia...sekarang dia sepertinya mulai sakit, terkena wabah kolera yang ada ", null),
            new NPCDialog("Warga", "Aku tidak akan terkejut sih kalau sebentar lagi VOC akan kewalahan dengan pemimpinnya sakit seperti itu...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Hmph...itu upah untuk yang serakah...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Tapi jujur saja aku tidak perlu gosip warga seperti ini, anggaplah aku berbuat budi baik saja)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sebaiknya aku cepat kembali ke Sultan Agung saja sebelum warga ini mengoceh lagi...)", null)

        }, false)
                    })
                }), 

                    new Mission(new List<Goal>
                {
                    new ExplorationGoal("Kembali ke Sultan Agung", 1, new string[] {"Sultan Agung"}, new int[] {1}, new Story[]{
                        new Story("Sultan Agung tampak mulai lebih banyak kerut di wajahnya, terbeban dengan kekalahannya yang kedua kali", new List<Dialogs>
        {
        new MainCharacterDialog(true, characterExpression.neutral, "Sultan! Sultan!", null),
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
        new MainCharacterDialog(true, characterExpression.neutral, "Tapi apa semua korban yang jatuh dengan serangan demi serangan akan setimpal dengan yang akan anda dapat?", null),
        new NPCDialog("Sultan Agung", "Saya tau...dan saya turut bersedih tentang hal ini", null),
        new NPCDialog("Sultan Agung", "Tapi saya dan pasukan harus terus berjuang, bukan demi kami sendiri", null),
        new NPCDialog("Sultan Agung", "Tapi demi warga Mataram dan seluruh pulau Jawa", null),
        new NPCDialog("Sultan Agung", "Sebaiknya kamu meninggalkan Mataram, dek Player, saya tidak ingin kamu terkena masala", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Tapi......", null),
        new NPCDialog("Sultan Agung", "Jangan mengkhawatirkan kami, dek Player", null),
        new NPCDialog("Sultan Agung", "Sebaiknya kamu pergi ke Pelabuhan Jepara dan mencari kepala pedagang disana", null),
        new NPCDialog("Sultan Agung", "Ia mengenal saya, jadi bilang saja padanya kalau saya mengirim kamu untuk ikut bersama mereka", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Baiklah...terimakasih Sultan, saya pamit dulu", null),
        new NPCDialog("Sultan Agung", "Saya yang seharusnya berterimakasih, dek Player, selamat jalan!", null)
        
        }, false)
                    })
                }), 
                    new Mission(new List<Goal>
                {
                    new ExplorationGoal("Lari ke Pelabuhan Jepara", 1, new string[] {"Kepala Pedagang NPC"}, new int[] {1}, new Story[]{
                        new Story("Sesampainya di pelabuhan, ada sebuah kapal yang mulai bersiap untuk berangkat", new List<Dialogs>
        {
        new NPCDialog("Kepala Pedagang", "Hei, hei, kamu mau apa, anak kecil?", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Saya teman dari Sultan Agung, pak...ia menitipkan saya untuk bisa ikut dengan bapak dan kapal bapak", null),
        new NPCDialog("Kepala Pedagang", "Anak kecil...teman dari mendiang Sultan...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Mendiang? Jangan-jangan.", null),
        new NPCDialog("Kepala Pedagang", "Hah? Kamu ini mengaku teman dari mendiang Sultan tapi bahkan tidak tau dia meninggal di tahun 1645 ini", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Sekarang tahun 1645??? Aku tidak paham bagaimana waktu berjalan di dunia ini lagi...kenapa aku terlempar bertahun-tahun hanya dengan berjalan ke tempat yang berdekatan...)", null),
        new NPCDialog("Kepala Pedagang", "Sepeninggal beliau Mataram semakin melemah dan membuka peluang untuk orang-orang Belanda itu untuk menguasai Mataram", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Jadi itu akhir dari serangan Sultan Agung...)", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Tapi sampai akhir pun dia tidak menyerah berjuang demi rakyatnya)", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Aku turut berduka dan iba dengan nasib rakyat sepeninggalnya...)", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Aku sebaiknya mencatat dan mengabadikan semangatnya...)", null),
        new NPCDialog("Kepala Pedagang", "Baiklah kalau kamu memang benar teman dari Sultan, kamu tidak keberatan kan kalau aku mengetesmu terlebih dahulu", null),
        new NPCDialog("Kepala Pedagang", "Kalau kamu benar teman dari Sultan seharusnya kamu tau tentang kisah serangan yang dia lakukan...", null),
        new NPCDialog("Kepala Pedagang", "Dan jangan berharap dapat mencontek catatan kamu ya, kamu harusnya bisa menjawab diluar kepalamu kalau kamu memang teman sultan", null),
        new NPCDialog("Kepala Pedagang", " Temui saja aku kalau kamu sudah siap", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Oh tidak...kenapa jadi ada ujian begini...)", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Aku merasa dihakimi deh sama kepala pedagang ini, tapi ya sudahlah...)", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Aku hadapi saja judgment ini", null)
        }, false)
                    })
                }), 

                  new Mission(new List<Goal>
                {
                    new JudgementGoal("Menyelesaikan tantangan Judgement dari Kepala Pedagang", 1, "Kepala Pedagang NPC", true)
                }), 

                  new Mission(new List<Goal>
                {
                    new ReviewGoal("Mereview kembali beberapa kejadian bersama Kepala Pedagang", 1, "Kepala Pedagang NPC", new Story[]{
                        new Story("Kepala pedagang selesai memberikan review singkat...", null, false)
                    }),
                    new JudgementGoal("Menyelesaikan tantangan Judgement dari Kepala Pedagang", 1, "Kepala Pedagang NPC", false)
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
        new NPCDialog("Pedagang", "Nanti akan saya bantu negosiasikan, tapi sebaiknya kamu dengar dulu…", null),
        new NPCDialog("Pedagang", "Sedikit rekap dari kisah serangan Sultan Agung", null),
        
        }, false)
                    }),
                    new ReviewGoal("Mereview kembali beberapa kejadian bersama pedagang", 1, "Pedagang Anak NPC", new Story[]{
                        new Story("Pedagang selesai memberikan review singkat...", null, false)
                    }),
                    new JudgementGoal("Menyelesaikan tantangan Judgement dari Kepala Pedagang", 1, "Kepala Pedagang NPC", false)
                }), 
                new Mission(new List<Goal>
                {
                    new GatherGoal("Berbicara dengan kepala pedagang", 1, new string[] {"Kepala Pedagang NPC"}, new Story[]{
                        new Story("Setelah perjuangan cukup panjang menjawab semua pertanyaan kepala pedagang, sepertinya kamu sudah mulai mendapat kepercayaannya", new List<Dialogs>
        {
        new NPCDialog("Kepala Pedagang", "Baiklah...aku rasa karena kamu sudah membuktikan dirimu dan paham betul situasi disini...", null),
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

        }, false)
                    })
                }), 
                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Menghampiri seseorang yang mungkin pemilik lencana tersebut", 1, new string[] {"Yudha"}, new int[] {-1}, new Story[]{
                        new Story("Kamu menghampiri orang itu yang sedang sibuk mencari sesuatu", new List<Dialogs>
        {
        new MainCharacterDialog(true, characterExpression.neutral, "Hmmmm? Dia ini kan yang tadi...", null),
        new NPCDialog("???", " ...kamu anak kecil yang tadi", null),
        new NPCDialog("???", " Sebutkan urusanmu dan pergi anak kecil, aku sedang mencari sesuatu...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Mencari apa? Tindakan ilegal baru?", null),
        new NPCDialog("???", "Hah! Aku tidak tau kamu dapat kesimpulan itu dari mana", null),
        new NPCDialog("???", "Tapi aku sedang mencari lencana polisiku", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Anda seorang polisi?", null),
        new NPCDialog("???", "Kalau iya memang kenapa?", null),
        new NPCDialog("???", "Aku Yudha, seorang detektif dari kepolisian, spesialisasi di kasus penjelajahan waktu", null),
        new MainCharacterDialog(true, characterExpression.neutral, "......", null),
        new NPCDialog("Yudha", "Kenapa?", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Tidak...aku pikir...ya sudahlah lupakan", null),
        new NPCDialog("Yudha", " Kalau kamu berpikir aku adalah penjelajah waktu ilegal...", null),
        new NPCDialog("Yudha", "Kamu harus tau kalau aku juga mengejar penjelajah itu", null),
        new NPCDialog("Yudha", "Karena itu aku mengaktifkan mesin itu yang dicurigai sebagai mesin waktu miliknya untuk mencari petunjuk...", null),
        new NPCDialog("Yudha", " Tapi sekarang kita berdua disini, di tempat yang seperti kita di masa lalu tapi disisi lain...", null),
        new NPCDialog("Yudha", "Tempat ini membuatnya seakan kita sedang dalam sebuah game RPG petualangan ", null),
        new NPCDialog("Yudha", "Sehingga kita dapat berinteraksi dengan semua orang-orang dan perang yang terjadi disana", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Kalau begitu...kita adalah tokoh utama dari permainan ini?", null),
        new NPCDialog("Yudha", "Itu hanya analogi, anak kecil... ", null),
        new NPCDialog("Yudha", "Oh iya, aku belum tau namamu dan kenapa kamu bisa disini", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Namaku Player, dan aku kira anda adalah orang yang mencurigakan jadi..", null),
        new NPCDialog("Yudha", " Hah, aku rasa kalau aku bekerja aku bisa nampak mencurigakan sih", null),
        new NPCDialog("Yudha", "Tapi sekarang bukan waktu yang baik untuk berbicara", null),
        new NPCDialog("Yudha", "Aku masih harus menemukan lencanaku", null),
        new MainCharacterDialog(true, characterExpression.neutral, ": (Mungkin kalau aku mengembalikan lencananya, dia akan mau membantuku keluar dari sini", null),
        new MainCharacterDialog(true, characterExpression.neutral, "(Setidaknya dengan adanya polisi aku akan jauh merasa lebih ama", null),
        
        }, false)
                    }),
                     new SubmitGoal("Memberikan lencana kembali kepada Yudha", 1, "Yudha", new Story[]{
                        new Story("Yudha dengan cepat mengambil kembali lencananya darimu", new List<Dialogs>
        {
        new NPCDialog("Yudha", "Oh hei, kamu menemukannya! Terimakasih banyak!", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Sama-sama, kalau begitu karena aku menemukannya untuk anda...", null),
        new NPCDialog("Yudha", "Ya sudah cukup dulu pembicaraan ini untuk sekarang", null),
        new NPCDialog("Yudha", "Aku akan kembali ke pekerjaanku dan kamu kembali ke urusanmu ya anak kecil...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Tunggu, anda akan pergi begitu saja?", null),
        new NPCDialog("Yudha", " Iya, aku berterima kasih atas bantuanmu menemukan barang-barangku...", null),
        new NPCDialog("Yudha", " ...tapi aku tidak ada niatan membawa anak kecil sepertimu", null),
        new NPCDialog("Yudha", "Nanti kamu malah mengganggu  penyelidikanku", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Tapi bukannya akan lebih baik kalau punya lebih dari satu kepala ya...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Lagi pula kita memiliki tujuan yang sama nantinya, keluar dari tempat ini", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Dan anda juga yang membuat saya ikut terjebak di dunia ini bersama anda...", null),
        new NPCDialog("Yudha", " Itu sih karena kamu yang sok ingin menghentikanku melakukan pekerjaanku", null),
        new NPCDialog("Yudha", "Tapi aku terbiasa kerja sendiri jadi yahh aku permisi dulu saja sebelum kamu mulai berbicara tidak jelas lagi", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Dasar keras kepala..dan tidak tau berterimakasih", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Kenapa dia tidak mau bekerja sama denganku padahal tujuan kita sama...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Tapi aku rasa Sultan Agung juga begitu, ia hanya berjuang demi daerahnya saja..", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Aku tidak melihatnya menerima atau meminta bantuan daerah lain..", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Ya sudahlah aku sebaiknya bergegas, kapalnya akan segera berangkat", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Masih akan jauh sebelum aku bisa keluar dari tempat ini sepertinya...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Hmmm? Apa ini?", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Seperti catatan jurnal...", null),
        new MainCharacterDialog(true, characterExpression.neutral, "Aku sebaiknya menyimpannya dan membukanya nanti ", null),
        }, false)
                    }, new Item[]{
                        new Item("police_badge", "Lencana Polisi", "Apa ada polisi di sekitar sini?", 1)
                    })
                })
            };
        }

        else
        {
            mission = new List<Mission>
            {

            };
        }

        return mission;
    }

    public Story getEndChapterStory(int chapter)
    {
        Story story_end;

        if(chapter == 1)
        {
             story_end = new Story("Dan perjalananmu pun masih berlanjut..", new List<Dialogs>
        {
            new CutsceneDialog("rumah_kosong", "Setelah perjuangan yang cukup panjang dari berbagai daerah lainnya, akhirnya kongsi dagang itu bubar pada tahun 31 Desember 1799", null),
            new CutsceneDialog("rumah_kosong", "Tapi Nusantara belum lepas dari tangan Belanda", null),
            new CutsceneDialog("rumah_kosong", "Pemimpin-pemimpin dari Belanda, dan bahkan Inggris sempat membawa alur pemerintahan di Nusantara", null),
            new CutsceneDialog("rumah_kosong", "Tapi tidak ada dari mereka yang membawa kemakmuran kepada Nusantara, atau Bumiputera, atau Hindia Belanda pada masanya masing-masing", null),
            new CutsceneDialog("rumah_kosong", "Jalan Anyer-Panarukan hingga tanam paksa, semua hasil pengerjaan paksa dari sekian banyak rakyat pada masanya", null),
            new CutsceneDialog("rumah_kosong", "Bahkan penguasa pribumi yang penuh keserakahan justru menambah penderitaan dari rakyat biasa", null),
            new CutsceneDialog("rumah_kosong", "Dan perjuangan dari rakyat Hindia Belanda pun masih terus berlangsung, untuk mengusahakan kemerdekaan mereka...", null)
        }, true);

        }

        else
        {
            story_end = new Story("", null, true);
        }

        return story_end;
    }

   
}
