using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice
{
    public string choiceDescription;
    public bool correct;

    public Choice(string desc, bool right)
    {
        this.choiceDescription = desc;
        this.correct = right;
    }
}
