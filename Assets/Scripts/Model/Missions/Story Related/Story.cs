using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Story 
{
    private string titleTimeFrame {get; set;}
    private bool isEnding {get; set;}
   
    public List<Dialogs> dialogs {get; set;}

    private bool completed {get; set;}

    private string music_on_scene;

    public Story(string title, List<Dialogs> dialog, bool isEnd, string muse)
    {
        this.titleTimeFrame = title;
        this.dialogs = dialog;
        this.isEnding = isEnd;
        this.music_on_scene = muse;
        completed = false;
    }

    public string getTitle()
    {
        return titleTimeFrame;
    }

    public string getMusic()
    {
        return music_on_scene;
    }

    public void checkDialogs()
    {
        completed = dialogs.All(d => d.shown);
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


