using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryUIManager : MonoBehaviour
{
    public GameObject autoPlayButton;
    public GameObject characterSprite;
    public GameObject nameBox;
    public GameObject dialogueBox;
    public GameObject background;

    public GameObject lightFrame;
    public GameObject glitchFrame;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    public float textSpeed;

    private int lineCount = 1;

    private Coroutine typeNextCoroutine;

    private SaveSlots slot;

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
              case Dialog.DialogTypes.questDialogue:
              {
                background.SetActive(false);
                nameBox.SetActive(true);
                dialogueBox.SetActive(true);
                lightFrame.SetActive(false);
                glitchFrame.SetActive(false);

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

                    case Dialog.Speaker.unknown:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "???";
                        break;
                    }

                    case Dialog.Speaker.sultanAgung:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Sultan Agung";
                        break;
                    }

                    case Dialog.Speaker.tumenggungBaurekhsa:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "T. Baurekhsa";
                        break;
                    }

                    case Dialog.Speaker.pedagang:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Pedagang";
                        break;
                    }

                    case Dialog.Speaker.kepalaPedagang:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Kepala Pedagang";
                        break;
                    }

                    case Dialog.Speaker.wargaMataram:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Warga";
                        break;
                    }

                    case Dialog.Speaker.wargaKaumAdat:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Kaum Adat";
                        break;
                    }

                    case Dialog.Speaker.wargaKaumPadri:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Kaum Padri";
                        break;
                    }

                    case Dialog.Speaker.james:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "James Du Puy";
                        break;
                    }

                    case Dialog.Speaker.tuankuLintau:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "T. Lintau";
                        break;
                    }

                    case Dialog.Speaker.tuankuImamBonjol:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "T. Imam Bonjol";
                        break;
                    }

                    case Dialog.Speaker.pasukanTuankuLintau:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Pasukan T. Lintau";
                        break;
                    }

                    case Dialog.Speaker.pasukanBelanda:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Pasukan Belanda";
                        break;
                    }

                    case Dialog.Speaker.putri:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Putri";
                        break;
                    }

                    case Dialog.Speaker.pamanPutri:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Paman Putri";
                        break;
                    }

                    case Dialog.Speaker.francis:
                    {
                        characterSprite.SetActive(false);
                        nameText.text = "Francis";
                        break;
                    }

                    case Dialog.Speaker.yudha:
                    {
                        characterSprite.SetActive(true);
                        nameText.text = "Yudha";

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
                    //change camera? 
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
                glitchFrame.SetActive(false);

                if (e.nextDialog.changeScenery == true)
                {
                    //change cutscene
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
                glitchFrame.SetActive(false);
                break;
              }

              case Dialog.DialogTypes.glitchTransition:
              {
                background.SetActive(false);
                characterSprite.SetActive(false);
                nameBox.SetActive(false);
                dialogueBox.SetActive(false);
                lightFrame.SetActive(false);
                glitchFrame.SetActive(true);
                break;
              }



              default: break;
           }

           lineCount++;
        }

        else
        {
            
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
