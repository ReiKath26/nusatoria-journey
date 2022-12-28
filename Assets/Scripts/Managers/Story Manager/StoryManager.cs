using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class StoryManager : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private GameObject storyOverlay;
    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private TextMeshProUGUI nameTextHolder;
    [SerializeField] private TextMeshProUGUI dialogTextHolder;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private GameObject backgroundObject;

    private Story story {get; set;}

    private Coroutine typeNextCoroutine;
    private Coroutine autoPlayCoroutine;

    private int dialogCount = 0;

    public static StoryManager instance;

    void Awake()
    {
        instance = this;
    }

    public void assignStory(Story storytale)
    {
       
        story = storytale;
        dialogTextHolder.text = story.getTitle();
        storyOverlay.SetActive(true);
        gameOverlay.SetActive(false);
    }

    public void nextLine(Dialogs dialog)
    {
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

         if (typeNextCoroutine != null)
         {
            StopCoroutine(typeNextCoroutine);
        }

        typeNextCoroutine = StartCoroutine(TypeLine(dialog.getDialog()));
        dialogCount++;
    }

    private void checkStory()
    {
        if(story.getCompleted() == true)
        {
            storyOverlay.SetActive(false);
            gameOverlay.SetActive(true);
            dialogCount = 0;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       if(autoPlayCoroutine == null && story != null && dialogCount < story.dialogs.Count)
       {
            nextLine(story.dialogs[dialogCount]);
            story.dialogs[dialogCount].showLine();
            story.checkDialogs();
            checkStory();
       }
    }

    public void OnToggleAutoPlay()
    {
        bool toggle = story.switchAutoPlay();

        if(autoPlayCoroutine != null)
        {
            StopCoroutine(autoPlayCoroutine);
        }

        if(toggle == true)
        {
            autoPlayCoroutine = StartCoroutine(AutoLine());
        }
    }

    IEnumerator TypeLine(string line)
    {
        AudioManager.instance.Play("Typewritter");
        foreach( char c in line.ToCharArray())
        {
            dialogTextHolder.text += c;
            yield return new WaitForSeconds(0.1f);
        }
        AudioManager.instance.Stop("Typewritter");
    }

    IEnumerator AutoLine()
    {
        while(dialogCount < story.dialogs.Count)
        {
            nextLine(story.dialogs[dialogCount]);
            story.dialogs[dialogCount].showLine();
            story.checkDialogs();
            checkStory();
            yield return new WaitForSeconds(5f);
        }
       
    }
}
