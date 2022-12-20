using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrologueManager : MonoBehaviour
{

    public GameObject autoPlayButton;
    public GameObject background;
    public GameObject characterSprite;
    public GameObject nameBox;
    public GameObject dialogueBox;

    public GameObject lightFrame;

    public string sceneChange;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    public float textSpeed;

    private int lineCount = 0;

    private Coroutine typeNextCoroutine;

    private SaveSlots slot;

    private Sprite nextLineSprite;


    void Awake()
    {
        slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
    }

    private void Start()
    {
        DialogueManager dialogueManager = GetComponent<DialogueManager>();
        dialogueManager.OnTriggerNextLine += DialogueManager_OnTriggerNextLine;
    }

    private void DialogueManager_OnTriggerNextLine(object sender, DialogueManager.OnTriggerNextLineEventArgs e)
    {
        if (lineCount < e.count)
        {
           switch(e.nextDialog.dialogType)
           {
              case Dialog.DialogTypes.prologueDialogue:
              {
                background.SetActive(true);
                nameBox.SetActive(true);
                dialogueBox.SetActive(true);
                lightFrame.SetActive(false);

                switch(e.nextDialog.speaker)
                {
                    case Dialog.Speaker.mainCharacter:
                    {
                        characterSprite.SetActive(true);
                        nameText.text = slot.playerName;


                        switch(e.nextDialog.expression)
                        {
                            case Dialog.Expression.happy:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("happy_mc_" + slot.playerGender);
                                break;
                            }

                            case Dialog.Expression.sad:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("sad_mc_" + slot.playerGender);
                                break;
                            }

                            case Dialog.Expression.angry:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("angry_mc_" + slot.playerGender);
                                break;
                            }

                            case Dialog.Expression.shook:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("shook_mc_" + slot.playerGender);
                                break;
                            }

                            case Dialog.Expression.hurt:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("hurt_mc_" + slot.playerGender);
                                break;
                            }

                            case Dialog.Expression.neutral:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("neutral_mc_" + slot.playerGender);
                                break;
                            }

                            case Dialog.Expression.think:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("think_mc_" + slot.playerGender);
                                break;
                            }

                            default: break;
                        }
                        break;
                    }

                  


                    case Dialog.Speaker.yudhaUnknown:
                    {
                        characterSprite.SetActive(true);
                        nameText.text = "???";

                        switch(e.nextDialog.expression)
                        {
                            case Dialog.Expression.happy:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("happy_yudha");
                                break;
                            }

                            case Dialog.Expression.sad:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("sad_yudha");
                                break;
                            }

                            case Dialog.Expression.angry:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("angry_yudha");
                                break;
                            }

                            case Dialog.Expression.shook:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("shook_yudha");
                                break;
                            }

                            case Dialog.Expression.hurt:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("hurt_yudha");
                                break;
                            }

                            case Dialog.Expression.neutral:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("neutral_yudha");
                                break;
                            }

                            case Dialog.Expression.think:
                            {
                                characterSprite.GetComponent<LoadSpriteManage>().loadNewSprite("think_yudha");
                                break;
                            }

                            default: break;
                        }
                        break;
                    }

                    case Dialog.Speaker.other:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "";
                        break;
                    }

                    default: break;
                }

                if (e.nextDialog.changeScenery == true)
                {
                    background.GetComponent<LoadSpriteManage>().loadNewSprite(sceneChange);
                    AudioManager.instance.Stop("Crowd");
                }

                dialogText.text = string.Empty;

                if (e.nextDialog.dialog.Contains("Player"))
                {
                    e.nextDialog.dialog = e.nextDialog.dialog.Replace("Player", slot.playerName);
                }

                if (typeNextCoroutine != null)
                {
                    StopCoroutine(typeNextCoroutine);
                }

                typeNextCoroutine = StartCoroutine(TypeLine(e.nextDialog.dialog));
                break;
              }

              case Dialog.DialogTypes.cutsceneDialogue:
              {
                background.SetActive(true);
                characterSprite.SetActive(false);
                nameBox.SetActive(false);
                dialogueBox.SetActive(true);
                lightFrame.SetActive(false);

                if (e.nextDialog.changeScenery == true)
                {
                    background.GetComponent<LoadSpriteManage>().loadNewSprite(sceneChange);
                    AudioManager.instance.Stop("Crowd");
                }

                dialogText.text = string.Empty;
                 if (typeNextCoroutine != null)
                {
                    StopCoroutine(typeNextCoroutine);
                }

                typeNextCoroutine = StartCoroutine(TypeLine(e.nextDialog.dialog));
                break;
              }

              case Dialog.DialogTypes.lightTransition:
              {
                background.SetActive(false);
                characterSprite.SetActive(false);
                nameBox.SetActive(false);
                dialogueBox.SetActive(false);
                lightFrame.SetActive(true);
                break;
              }

              default: break;
           }

           lineCount++;
        }

        else
        {
            characterSprite.GetComponent<LoadSpriteManage>().OnDestroy();
            background.GetComponent<LoadSpriteManage>().OnDestroy();
            slot.chapterNumber = 1;
            SaveHandler.instance.saveSlot(slot, slot.slot);
            SceneManage.instance.LoadScene(5);
        }
    }


     IEnumerator TypeLine(string line)
    {
        AudioManager.instance.Play("Typewritter");
        foreach( char c in line.ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        AudioManager.instance.Stop("Typewritter");
    }

}
