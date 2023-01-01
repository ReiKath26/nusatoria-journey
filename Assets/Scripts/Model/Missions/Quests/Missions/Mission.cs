using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mission
{
    public List <Goal> goals {get; set;}
    public bool completed {get; set;}

    public Mission(List<Goal> goal)
    {
        this.goals = goal;
        completed = false;
    }

    public void evaluate()
    {
        completed = goals.All(g => g.getCompletion());
        Debug.Log("Mission is completed: " + completed);
    }
}
