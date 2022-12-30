using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mission
{
    private Story onTriggerStory;
    public List <Goal> goals {get; set;}
    public bool completed {get; set;}

    public Mission(List<Goal> goal, Story storyType )
    {
        this.onTriggerStory = storyType;
        this.goals = goal;
        completed = false;
    }

    public Story loadTriggerStory()
    {
        if(onTriggerStory != null)
        {
            return onTriggerStory;
        }

        else
        {
            return null;
        }
    }


    public void evaluate()
    {
        completed = goals.All(g => g.getCompletion());
    }
}
