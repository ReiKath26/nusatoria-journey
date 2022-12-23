using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    //note to cath: input autoplay logic 
    [SerializeField] private Dialog [] dialogues;

    public Dialog[] _dialogues;
    private bool AutoPlay;

    public event EventHandler<OnTriggerNextLineEventArgs> OnTriggerNextLine;

   //note to cath: input autoplay logic 
    public event EventHandler OnTriggerAutoPlay;

    public static DialogueManager instance;

    void Awake()
    {
        instance = this;
        _dialogues = dialogues;
    }

    public void setDialogues(Dialog [] dialogue)
    {
        _dialogues = dialogue;
    }

    public class OnTriggerNextLineEventArgs: EventArgs
    {
        public Dialog nextDialog;
        public int count;
    }

    private int lineCount = 0;

    public void restartTheCounter()
    {
        lineCount = 0;
    }

    public void NextLine()
    {
        OnTriggerNextLine?.Invoke(this, new OnTriggerNextLineEventArgs {nextDialog = _dialogues[lineCount], count = dialogues.Length});
        Debug.Log(dialogues.Length);
        lineCount++;
    }
    
}
