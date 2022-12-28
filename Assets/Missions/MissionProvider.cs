using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionProvider : MonoBehaviour
{
    public static MissionProvider instance;
    public List<Mission> allMissions;

    void Awake()
    {
        instance = this;
    }

    public void getChapterMissions(int chapter)
    {
        if(chapter == 0)
        {
            //set all mission to chapter 1 mission
        }

        else
        {
            //set all mission to chapter 2 mission
        }
    }

    public Story getEndChapterStory(int chapter)
    {
        Story story = new Story();
         if(chapter == 0)
        {
            //set all mission to chapter 1 mission
        }

        else
        {
            //set all mission to chapter 2 mission
        }

        return story;
    }
}
