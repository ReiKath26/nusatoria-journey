using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Story 
{

    public int storyNumber;
    public string titleTimeFrame;
    protected bool onAutoPlay;
   
    public List<Dialogs> dialogs;

    public bool completed {get; protected set; }

    public class Dialogs
    {
        protected string nameText;
        protected string dialogText;
        public bool shown {get; protected set;}

        public virtual string getName()
        {
            return nameText;
        }

        public virtual string getDialog()
        {
            return dialogText;
        }

        public virtual void initialize()
        {
            shown = false;
        }

        public void showLine()
        {
            shown = true;
        }

    }

    public void initialize()
    {
        completed = false;
        onAutoPlay = false;

        foreach(var dialog in dialogs)
        {
            dialog.initialize();
        }
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

    public bool checkDialogs()
    {
        completed = dialogs.All(d => d.shown);

        return completed;
    }
}


