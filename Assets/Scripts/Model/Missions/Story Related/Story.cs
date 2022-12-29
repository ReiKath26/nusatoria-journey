using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Story 
{
    private string titleTimeFrame {get; set;}
    private bool onAutoPlay {get; set;}
    private bool isEnding {get; set;}
   
    public List<Dialogs> dialogs {get; set;}

    private bool completed {get; set;}

    public void initialize(string title, List<Dialogs> dialog, bool isEnd)
    {
        this.titleTimeFrame = title;
        this.dialogs = dialog;
        this.isEnding = isEnd;
        completed = false;
        onAutoPlay = false;
    }

    public string getTitle()
    {
        return titleTimeFrame;
    }

    public bool switchAutoPlay()
    {
        if(onAutoPlay == true)
        {
            onAutoPlay = false;
        }

        else
        {
            onAutoPlay = true;
        }
       
        return onAutoPlay;
    }

    public void checkDialogs()
    {
        completed = dialogs.All(d => d.shown);
        Debug.Log(completed);
    }

    public bool getCompleted()
    {
        return completed;
    }

    public bool getIsEnding()
    {
        return isEnding;
    }
}


