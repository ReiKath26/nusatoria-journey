using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class StoryManager : MonoBehaviour
{

    [SerializeField] private GameObject storyOverlay;
    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private TextMeshProUGUI nameTextHolder;
    [SerializeField] private TextMeshProUGUI dialogTextHolder;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject missionManager;
    private GameObject player;
    private Vector3 initialPosition;

    private Story story {get; set;}

    private Coroutine typeNextCoroutine;

    private int dialogCount = 0;

    public static StoryManager instance;

    private bool beginningTutorial = true;

    void Start()
    {
       if(FindInactiveObject.instance == null)
     {
          Debug.Log("It's null!");
     }

     else
     {
        Debug.Log("tdk Null");
     }
        instance = this;

         SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

        if(slot.chapterNumber == 1)
        {
            if(slot.playerGender == 0)
            {
                initialPosition = new Vector3(1125.646f, 201.7f, 796.8095f);
            }

            else
            {
                initialPosition = new Vector3(1130.106f, 206.8f, 804.8292f);
            }

            float distance = Vector3.Distance(initialPosition, new Vector3(slot.lastPosition.x_pos, slot.lastPosition.y_pos, slot.lastPosition.z_pos));
            if(slot.missionNumber == 0 && distance <= 50f)
            {
                assignStory(getBeginningOfChapterStory());
            }
        }

        else if(slot.chapterNumber == 2)
        {
            initialPosition = new Vector3(555.7f, 193.94f, 2891.723f);

            float distance = Vector3.Distance(initialPosition, new Vector3(slot.lastPosition.x_pos, slot.lastPosition.y_pos, slot.lastPosition.z_pos));
            if(slot.missionNumber == 0 && distance <= 50f)
            {
                assignStory(getBeginningOfChapterStory());
            }
        }

        else
        {
            assignStory(getBeginningOfChapterStory());
        }

        if(slot.playerGender == 0)
        {
            player = FindInactiveObject.instance.find("Male MC Model");
        }

        else
        {
            player = FindInactiveObject.instance.find("Female MC Model");
        }
       
    }

    private Story getBeginningOfChapterStory()
    {
         SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

        switch(slot.chapterNumber)
            {
               case 0:
               {
                    List<Dialogs> dialouge = new List<Dialogs>
                    {
                        new MainCharacterDialog(true, characterExpression.hurt, "Huftt...huft...lelah sekali", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "(Namaku Player, aku adalah siswa di sekolah menengah ternama di Jakarta)", null),
                        new MainCharacterDialog(true, characterExpression.hurt, "(Atau setidaknya aku harap akan tetap seperti itu melihat kondisi kota sekarang...)", null),
                        new MainCharacterDialog(true, characterExpression.hurt, "(Orang-orang anti penjelajah waktu dan yang pro penjelajahan waktu itu bertengkar terus menerus...)", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "(Kalau mereka mau ribut bawa keributan di tempat lain saja, aku tidak mau tau...)", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "(Lagian kalau kota ini hancur karena mereka, itu bukan urusanku...)", null),
                        new MainCharacterDialog(true, characterExpression.think, "Hmmmm?", null),
                        new MainCharacterDialog(true, characterExpression.think, "Dicari penjelajah waktu ilegal...bila melihat tanda-tanda penjelajahan waktu ilegal hubungi Detektif Yudha dari...", null),
                        new MainCharacterDialog(true, characterExpression.think, "Tapi bagaimana kita bisa tau orang itu penjelajah waktu ilegal atau tidak...", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "Lagian kenapa mereka anti sekali dengan penjelajahan waktu", null),
                        new MainCharacterDialog(true, characterExpression.happy, "Kalau aku punya dana, aku pasti mau melakukan perjalanan...", null),
                        new MainCharacterDialog(false, characterExpression.neutral, "...", null),
                        new MainCharacterDialog(true, characterExpression.think, "Hmmm...orang itu baru saja masuk ke rumah yang lama tak berpenghuni itu...", null),
                        new MainCharacterDialog(true, characterExpression.think, "Saaangatt tidak mencurigakan...", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "Aku sebaiknya mengikutinya untuk memastikan", null),
                        new CutsceneDialog("rumah_kosong", "14 November 2195, 11.20, Rumah Terpencil", new string[] {"Rumah Kosong Background"}),
                        new MainCharacterDialog(true, characterExpression.think, "Apa yang orang itu lakukan disini...", null),
                        new CutsceneDialog("cutscene_tm", "!!!", null),
                        new MainCharacterDialog(false, characterExpression.neutral, "Hmmm...", null),
                        new MainCharacterDialog(true, characterExpression.shook, "(Apa itu...tidak mungkin...mesin waktu?!)", null),
                        new MainCharacterDialog(false, characterExpression.neutral, "Baiklah...sekarang melakukannya seperti biasa...", null),
                        new MainCharacterDialog(true, characterExpression.shook, "(Apa dia menghidupkan mesin waktu itu?", null),
                        new MainCharacterDialog(true, characterExpression.angry, "Hei, apa yang anda lakukan?!", null),
                        new MainCharacterDialog(false, characterExpression.shook, "Ah..anak kecil, jangan mendekat, nanti kamu ikut terba-", null),
                        new MainCharacterDialog(true, characterExpression.shook, "AHHHHHH", null),
                        new CutsceneDialog("cutscene_ship", "Semua bermula dari kedatangan yang tidak diantisipasi oleh Nusantara", null),
                        new CutsceneDialog("cutscene_ship", "Pada tahun 1596, Cornelis de Houtman mendarat di Banten melalui Selat Sunda", null),
                        new CutsceneDialog("cutscene_ship", "Pelayaran ini membuka perdagangan antara Belanda dan Nusantara", null),
                        new CutsceneDialog("cutscene_ship", "Dan dimulailah masa penjajahan Belanda", null),
                        new CutsceneDialog("cutscene_voc", "Di mulai dengan kemunculan kongsi dagang milik mereka, Vereenigde Oostindische Compagnie, atau VOC", null),
                        new CutsceneDialog("cutscene_voc", "Dibuat dengan tujuan menghindari persaingan tidak sehat antara pedagang Belanda dan pedagang Eropa lainnya", null),
                        new CutsceneDialog("cutscene_voc", "Tapi dalam praktiknya, kongsi dagang ini membawa penderitaan bagi rakyat Indonesia", null),
                        new CutsceneDialog("cutscene_voc", "Mereka terus memecah belah dengan politik devide et impera mereka, untuk menguasai seluruh penjuru Indonesia", null),
                        new CutsceneDialog("cutscene_voc", "Dan itu mendorong segala macam usaha, bagi rakyat Nusantara, untuk memperjuangkan kemerdekaan mereka", null)

                    };
                    Story thisStory = new Story("Prologue: Permulaan dari Segalanya || 14 November 2195, 11:00 - Jalanan Kota Jakarta", dialouge, true, null);
                    return thisStory;
               }
              
               case 1:
               {
                    List<Dialogs> dialouge = new List<Dialogs>
                    {
                        new MainCharacterDialog(true, characterExpression.hurt, "Aduh...", null),
                        new MainCharacterDialog(true, characterExpression.shook, "(Hah...aku dimana?)", null),
                        new MainCharacterDialog(true, characterExpression.shook, "(Tempat ini...apa aku benar-benar terbawa ke masa waktu lain karena mesin tadi...", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "(Sebaiknya aku mencari informasi dan mencatat petunjuk yang aku dapat untuk mencari tau tempat apa ini...)", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "(Dan kalau aku sudah tau, baru aku dapat memikirkan cara keluar dari sini...)", null)
                    };
                    Story thisStory = new Story("Chapter 1: Terjebak dalam Kegelapan || ?? ?? ??, ?? - ???", dialouge, false, null);
                    return thisStory;
               }
               
               case 2:
               {
                    List<Dialogs> dialouge;
                    

                    if(slot.understandingLevel == 1)
                    {
                        dialouge = new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.hurt, "Jadi...ini Minangkabau...", new string[]{"Yudha_Pier"}),
                            new MainCharacterDialog(true, characterExpression.hurt, "Suasana disini sangat berbeda dari di Jawa, tapi aku penasaran apakah aku akan menemui konflik yang sama disini...", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Tapi disini aku tidak punya pengetahuan apapun atau harus kemana, jadi aku sepertinya stuck untuk sekarang...", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Tapi kalau begitu bagaimana aku bisa keluar dari tempat ini...", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Seingatku ada kejadian bersejarah juga di Minang ini...tapi apa ya....", new string[]{"Yudha_Pier"}),
                            new MainCharacterDialog(true, characterExpression.think, "Hmmm..orang yang berdiri di tembok itu sepertinya ke arah sana, aku ikuti dia saja deh daripada tidak bergerak sama sekali...", null),
                        };
                    }

                    else if(slot.understandingLevel == 2)
                    {
                        dialouge = new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.hurt, "Jadi...ini Minangkabau...", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Suasana disini sangat berbeda dari di Jawa, tapi aku penasaran apakah aku akan menemui konflik yang sama disini...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Oh iya, jurnal yang aku temukan tadi sebaiknya aku...)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Ah...dimana aku menaruhnya?)", null),
                            new MainCharacterDialog(false, characterExpression.think, "Hmmm...menarik...", new string[]{"Yudha_Pier"}),
                            new MainCharacterDialog(true, characterExpression.angry, "Ahh! Jangan muncul tiba-tiba begitu dan jangan mengambil barang orang seenak jidat!", null),
                            new MainCharacterDialog(false, characterExpression.hurt, "Apa? Kamu menemukan petunjuk maka seharusnya kamu memberikannya pada polisi terpercaya", null),
                            new MainCharacterDialog(true, characterExpression.angry, "Polisi terpercaya tidak akan meninggalkan warga tidak bersalah...", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Hmph!", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Jika kamu harus tau, jurnal ini milik seseorang yang memulai projek dengan nama yang disamarkan", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Dia bercerita tentang semua kejadian yang kita lalui tadi dan bagaimana dia melihatnya langsung", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Jadi dia itu...", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Penjelajah waktu, iya, dan kemungkinan dia penjelajah waktu ilegal yang aku cari", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Sisanya tidak penting...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Nih aku kembalikan, sekarang aku permisi dulu ya...", null),
                            new MainCharacterDialog(true, characterExpression.angry, "Hmph! Masih keras kepala...", new string[]{"Yudha_Pier"}),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Tapi sekarang sebaiknya aku kemana...)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Apa aku ikut dia saja ya..dia sepertinya lebih familiar dengan dunia ini dari aku)", null),     
                            new MainCharacterDialog(true, characterExpression.neutral, "(Dia pasti akan mengeluh lagi kalau aku mengikutinya, tapi ya sudahlah aku tidak peduli juga)", null),       
                        };
                    }

                    else
                    {
                        dialouge = new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.hurt, "Jadi...ini Minangkabau...", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Suasana disini sangat berbeda dari di Jawa, tapi aku penasaran apakah aku akan menemui konflik yang sama disini...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Oh iya, jurnal yang aku temukan tadi sebaiknya aku...)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Ah...dimana aku menaruhnya?)", null),
                            new MainCharacterDialog(false, characterExpression.think, "Hei lagi, anak kecil! Bagi dong bacaannya!", new string[]{"Yudha_Pier"}),
                            new MainCharacterDialog(true, characterExpression.angry, "Ahh! Jangan muncul tiba-tiba begitu dan jangan mengambil barang orang seenak jidat!", null),
                            new MainCharacterDialog(false, characterExpression.think, "Hei hei, kan kita sudah bekerja sama, jadi kita harus saling berbagi petunjuk dong!", null),
                            new MainCharacterDialog(true, characterExpression.angry, "Terserah anda saja, jadi apa isi jurnal itu?", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Sabar-sabar, ini aku bacakan isinya untukmu ya...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Intinya jurnal ini milik seseorang yang memulai projek dengan nama yang disamarkan", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Dia bercerita tentang semua kejadian yang kita lalui tadi dan bagaimana dia melihatnya langsung", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Jadi dia itu...", null),
                            new MainCharacterDialog(false, characterExpression.angry, "Penjelajah waktu, iya, dan kemungkinan dia penjelajah waktu ilegal yang aku cari", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Sisanya tidak penting...", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Nih aku kembalikan, sekarang aku ini sebenarnya masih lebih suka kerja sendiri sih....", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Tapi aku akan membiasakan diri dengan adanya kamu di tim, jadi sekarang sebaiknya kita bergegas...", null),
                            new MainCharacterDialog(true, characterExpression.shook, "Hei, tunggu dulu!", new string[]{"Yudha_Pier"}),
                        };
                    }
                    
                    Story thisStory = new Story("Chapter 2: Warna yang Berbeda || 1821, Pagi Hari - Pelabuhan Minang", dialouge, false, null);
                    return thisStory;
               }
              
               default: 
               {
                 return null;
               }
            }
    }

    public void assignStory(Story storytale)
    {
        story = storytale;
        spriteObject.SetActive(false);
        dialogTextHolder.text = story.getTitle();
        nameTextHolder.text = "";
        storyOverlay.SetActive(true);
        gameOverlay.SetActive(false);

        if(story.getMusic() != null)
        {
            foreach(Sound s in AudioManager.instance.sounds)
            {
                if(s.source.isPlaying)
                {
                    s.source.Stop();
                }
            }

            AudioManager.instance.Play(story.getMusic());
        }
    }

    public void nextLine(Dialogs dialog)
    {
        Debug.Log("Next line!");
        if(dialog is MainCharacterDialog mc_dialog)
        {
            spriteObject.SetActive(true);
            backgroundObject.SetActive(false);
            spriteObject.GetComponent<LoadSpriteManage>().loadNewSprite(mc_dialog.getSpriteToBeShown());
        }

        else if(dialog is NPCDialog npc_dialog)
        {
            spriteObject.SetActive(false);
            backgroundObject.SetActive(false);
        }

        else if(dialog is CutsceneDialog cut_dialog)
        {
            spriteObject.SetActive(false);
            backgroundObject.SetActive(true);
            backgroundObject.GetComponent<LoadSpriteManage>().loadNewSprite(cut_dialog.getSprites());
        }

        nameTextHolder.text = dialog.getName();

        if(dialog.objectChange != null)
        {
            foreach(string objName in dialog.objectChange)
            {
                GameObject obj = FindInactiveObject.instance.find(objName);

                if(obj.activeSelf == true)
                {
                    obj.SetActive(false);
                }

                else
                {
                    obj.SetActive(true);
                }
            }
        }

         if (typeNextCoroutine != null)
         {
            StopCoroutine(typeNextCoroutine);
         }

        dialogTextHolder.text = "";
        typeNextCoroutine = StartCoroutine(TypeLine(dialog.getDialog()));
    }

    public void onButtonClick()
    {
        Debug.Log(dialogCount);
        Debug.Log(story.dialogs.Count);
        if( story != null && dialogCount < story.dialogs.Count)
        {
            nextLine(story.dialogs[dialogCount]);
            story.dialogs[dialogCount].showLine();
            dialogCount++;
        }

        else
        {

            story.checkDialogs();
            checkStory();
        }

    }

    private void checkStory()
    {
        if(story.getCompleted() == true && story.getIsEnding() == false)
        {
            SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
            AudioManager.instance.Stop("Typewritter");

            if(slot.chapterNumber == 1 && slot.missionNumber == 0 && beginningTutorial == true)
            {
                TutorialTriggerManager.instance.setTutorial();
            }

            beginningTutorial = false;
            storyOverlay.SetActive(false);
            gameOverlay.SetActive(true);
            dialogCount = 0;
        }

        else if (story.getCompleted() == true && story.getIsEnding() == true)
        {
            SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
            AudioManager.instance.Stop("Typewritter");

            Item[] clues = new Item[]
            {
                new Item("wanted_poster", "Poster Dicari", "Poster ini mencari penjelajah waktu ilegal, tapi karena poster ini aku jadi menjelajah waktu?", 1),
                new Item("Journal", "Jurnal #1", "Jurnal ini ditinggalkan oleh seseorang, berisi terkait projek yang ia kerjakan setelah menjelajah ke waktu sejarah di masa perjuangan Sultan Agung", 1),
                new Item("Journal", "Jurnal #2", "Jurnal ini ditinggalkan oleh seseorang, membahas lebih lanjut terkait projek yang ia kerjakan dan keinginannya untuk membuat ending yang bahagia, membuat kami menduga kami berada di sebuah dunia simulasi kejadian sejarah", 1)
            };

             switch(slot.chapterNumber)
            {
               case 0:
               {
                    slot.chapterNumber = 1;
                    slot.missionNumber = 0;
                    slot.goalNumber = 0;
                    SaveHandler.instance.saveItem(clues[0], PlayerPrefs.GetInt("choosenSlot"));
                    SaveHandler.instance.saveSlot(slot, slot.slot);
                    SceneManage.instance.LoadScene(3);
                    break;
               }
              
               case 1:
               {
                    slot.chapterNumber = 2;
                    slot.missionNumber = 0;
                    slot.goalNumber = 0;
                    slot.lastPosition = new PlayerPosition() {x_pos = 555.7f, y_pos = 205f, z_pos = 2891.723f};
                    SaveHandler.instance.saveItem(clues[1], PlayerPrefs.GetInt("choosenSlot"));
                    SaveHandler.instance.saveSlot(slot, slot.slot);
                    SceneManage.instance.LoadScene(4);
                    Destroy(missionManager);
                    Destroy(player);
                    break;
               }
               
               case 2:
               {
                    slot.chapterNumber = 3;
                    slot.missionNumber = 0;
                    slot.goalNumber = 0;
                    SaveHandler.instance.saveSlot(slot, slot.slot);
                    SceneManage.instance.LoadScene(0);
                    Destroy(missionManager);
                    Destroy(player);
                break;
               }
              
               default: break;
             }
        }
    }


    IEnumerator TypeLine(string line)
    {
        nextButton.SetActive(false);
        AudioManager.instance.Play("Typewritter");
        foreach( char c in line.ToCharArray())
        {
            dialogTextHolder.text += c;
            yield return new WaitForSeconds(0.01f);
        }
         nextButton.SetActive(true);
        AudioManager.instance.Stop("Typewritter");
    }

}
