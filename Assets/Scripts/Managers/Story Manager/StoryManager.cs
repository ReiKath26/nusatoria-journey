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

    private bool beginningTutorial;

    void Start()
    {
        beginningTutorial = true;
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
                        new MainCharacterDialog(true, characterExpression.hurt, "Huftt...huft...", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "(Aku Player, siswa sekolah menengah yang hanya menjalani hidupnya seperti biasa...)", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "(Aku mau bilang sampai hari ini..tapi tidak mungkin kan sebuah petualangan yang tak terduga muncul begitu saja)", null),
                        new MainCharacterDialog(true, characterExpression.think, "Hmmmm? Poster apa ini?", null),
                        new MainCharacterDialog(true, characterExpression.think, "Dicari penjelajah waktu ilegal...bila melihat tanda-tanda penjelajahan waktu ilegal hubungi Detektif Yudha dari...", null),
                        new MainCharacterDialog(true, characterExpression.think, "Tapi bagaimana kita bisa tau orang itu penjelajah waktu ilegal atau tidak...", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "Lagian kenapa mereka anti sekali dengan penjelajahan waktu", null),
                        new MainCharacterDialog(true, characterExpression.happy, "Kalau aku punya dana, aku pasti mau melakukan pertualangan...", null),
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
                        new MainCharacterDialog(false, characterExpression.neutral, "Hmmmm? Oh hanya anak kecil...", null),
                        new MainCharacterDialog(false, characterExpression.neutral, "Jangan ikut campur anak kecil, kecuali kamu ingin berurusan dengan kepolisian...", null),
                        new MainCharacterDialog(true, characterExpression.shook, "Anda seorang polisi?", null),
                        new MainCharacterDialog(false, characterExpression.neutral, "Memangnya tidak terlihat ya?", null),
                        new MainCharacterDialog(false, characterExpression.neutral, "Namaku Yudha, seorang detektif kepolisian, itu saja yang perlu kamu tau", null),
                        new MainCharacterDialog(false, characterExpression.neutral, "Sekarang sebaiknya kamu minggir dan jangan menyentuh apapun", null),
                        new MainCharacterDialog(true, characterExpression.think, "Apa anda menyelidiki sesuatu sampai masuk ke rumah terpencil seperti ini?", null),
                        new MainCharacterDialog(false, characterExpression.angry, "Bukan urusanmu, anak kecil...", null),
                        new MainCharacterDialog(true, characterExpression.neutral, "Oke terserah saja, ngomong-ngomong gelap sekali disini, aku nyalakan lampunya ya...", null),
                        new MainCharacterDialog(false, characterExpression.shook, "Hei sudah aku bilang, jangan sentuh apapun!", null),
                        new NPCDialog("PC", "Mengaktifkan Projek Nusatoria", null),
                        new MainCharacterDialog(false, characterExpression.shook, "Oh tidak...", null),
                         new NPCDialog("PC", "*glitch*", null),
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
                        new NPCDialog("PC", "Pemain, selamat datang di Projek Nusatoria!", null),
                        new MainCharacterDialog(true, characterExpression.shook, "Pemain apa maksudmu? Siapa itu yang berbicara?", null),
                        new NPCDialog("PC", "Saat ini anda sedang berada di dunia simulasi sejarah perjuangan kemerdekaan Indonesia", null),
                        new NPCDialog("PC", "Terdapat 3 bagian dari dunia ini yang harus anda jelajahi satu persatu", null),
                        new MainCharacterDialog(true, characterExpression.shook, "Hah? Ini bukan game RPG, bicara yang jelas, bagaimana aku bisa keluar dari sini?", null),
                        new NPCDialog("PC", "Untuk dapat menyelesaikan eksplorasi satu bagian dunia, anda harus menemukan pintu keluar dari bagian tersebut...", null),
                        new NPCDialog("PC", "Informasi terkait dimana pintu keluar tersebut tersimpan di sebuah peti rahasia...", null),
                        new NPCDialog("PC", "...yang memerlukan sebuah kunci untuk membukanya", null),
                        new NPCDialog("PC", "Untuk mendapatkan kunci tersebut, anda harus mencari tau NPC yang mana di dunia ini yang memiliki kunci tersebut", null),
                        new NPCDialog("PC", "Hal ini dapat anda lakukan dengan mengikuti simulasi kisah sejarah yang digambarkan di daerah tersebut", null),
                        new NPCDialog("PC", "Sampai akhirnya rangkaian NPC NPC di cerita tersebut akan mengarahkan anda ke NPC tersebut yang akan memberikan anda tantangan terakhir untuk mendapatkan kunci tersebut", null),
                        new NPCDialog("PC", "Untuk informasi lain terkait sistematis dari petualangan di dunia ini, sudah diupload ke ponsel anda dan anda bisa buka melalui tab (?) atau tutorial", null),
                        new MainCharacterDialog(true, characterExpression.shook, "Wah jadi aku benar-benar terperangkap di dunia simulasi petualangan sejarah yang aneh...", null),
                        new MainCharacterDialog(true, characterExpression.think, "Tapi sepertinya aku tidak punya pilihan lain, sebaiknya aku cek dulu detail teknis lainnya dan mulai mengikuti simulasi seperti yang orang itu bilang", null),
                       
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
                            new MainCharacterDialog(true, characterExpression.hurt, "Jadi...daerah selanjutnya Minangkabau...", new string[]{"Yudha_Pier"}),
                            new MainCharacterDialog(true, characterExpression.hurt, "Suasana disini sangat berbeda dari di Jawa, tapi aku penasaran apakah aku akan menemui konflik yang sama disini...", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Tapi disini aku tidak punya pengetahuan apapun atau harus kemana, jadi aku sepertinya stuck untuk sekarang...", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Tapi kalau begitu bagaimana aku bisa keluar dari tempat ini...", null),
                            new MainCharacterDialog(true, characterExpression.hurt, "Seingatku ada kejadian bersejarah juga di Minang ini...tapi apa ya....", new string[]{"Yudha_Pier"}),
                            new MainCharacterDialog(true, characterExpression.think, "Hmmm..orang yang berdiri di tembok itu sepertinya ke arah sana, aku ikuti dia saja deh daripada tidak bergerak sama sekali...", null),
                        };
                    }


                    else
                    {
                        dialouge = new List<Dialogs>
                        {
                            new MainCharacterDialog(true, characterExpression.hurt, "Jadi...daerah selanjutnya Minangkabau...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Oh hei...jurnal yang aku temukan di peti tadi masuk ke inventarisku", null),
                            new MainCharacterDialog(false, characterExpression.think, "Bagus dong kalau begitu!", new string[]{"Yudha_Pier"}),
                            new MainCharacterDialog(true, characterExpression.angry, "Ahh! Jangan muncul tiba-tiba begitu!", null),
                            new MainCharacterDialog(false, characterExpression.think, "Maaf maaf, kebiasaan emang jadi detektif haha", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Oke sekarang karena kita sudah di daerah baru kembali ke mencari NPC mana yang memiliki kunci", null),
                            new MainCharacterDialog(false, characterExpression.neutral, "Mohon kerja samanya ya, partner, aku duluan!", null),
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
            StartCoroutine(backToGame());
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
                    if(slot.understandingLevel > 1)
                    {
                         SaveHandler.instance.saveItem(clues[1], PlayerPrefs.GetInt("choosenSlot"));
                    }
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
        SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
        nextButton.SetActive(false);
        AudioManager.instance.Play("Typewritter");
        foreach( char c in line.ToCharArray())
        {
            dialogTextHolder.text += c;

            if(slot.chapterNumber == 0)
            {
                 yield return new WaitForSeconds(0.01f);
            }

            else
            {
                 yield return new WaitForSeconds(0.0001f);
            }
           
        }
         nextButton.SetActive(true);
        AudioManager.instance.Stop("Typewritter");
    }

    IEnumerator backToGame()
    {
        SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
        AudioManager.instance.Stop("Typewritter");
        if(slot.chapterNumber == 1 && slot.missionNumber == 0 && beginningTutorial == true)
        {
            TutorialTriggerManager.instance.setTutorial();
        }

        beginningTutorial = false;
        storyOverlay.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        gameOverlay.SetActive(true);
        dialogCount = 0;
    }

}
