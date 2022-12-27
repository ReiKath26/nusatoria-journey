using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mission
{
    [SerializeField] private string missionName;
    [SerializeField] private List <Goal> goals;
    private bool completed;

    public void initialize()
    {
        completed = false;
    }

    public bool evaluate()
    {
        completed = goals.All(g => g.completed);

        return completed;
    }
}
