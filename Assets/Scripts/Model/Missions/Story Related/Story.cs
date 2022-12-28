using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Story 
{
    private int storyNumber {get; set;}
    private string titleTimeFrame {get; set;}
    private bool onAutoPlay {get; set;}
   
    public List<Dialogs> dialogs {get; set;}

    private bool completed {get; set;}

    public void initialize(int storyNum, string title, List<Dialogs> dialog)
    {
        this.storyNumber = storyNum;
        this.titleTimeFrame = title;
        this.dialogs = dialog;
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
    }

    public bool getCompleted()
    {
        return completed;
    }
}


