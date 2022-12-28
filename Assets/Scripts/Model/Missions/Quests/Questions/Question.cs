using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum level
{
    easy, medium, hard
}

public class Question
{
    private string npcName {get; set;}
    private string[] questionDescription {get; set;}
    private Choice[] choices {get; set;}
    private level QuestionLevel {get; set;}
    private bool answered {get; set;}

    public void initialize(string npcname, string[] questionDesc, Choice[] choice, level questLevel)
    {
        this.npcName = npcname;
        this.questionDescription = questionDesc;
        this.choices = choice;
        this.QuestionLevel = questLevel;
        answered = false;
    }

    public float answerQuestion(int choiceChoosen, int questionCount)
    {
        answered = true;

        if(choices[choiceChoosen].correct == true)
        {
            return(float)100 / (float)questionCount;
        }

        else
        {
            return 0f;
        }
    }
}
