using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum level
{
    easy, medium, hard
}

public enum questionType
{
    latarBelakang_sa, seranganSatu_sa, seranganDua_sa, akhir_sa, latarBelakang_padri, perjuangan_tl, perjuangan_kp, faseKedua_padri, faseKetiga_padri, akhirPerang_padri
}

public class Question
{
    private string npcName {get; set;}
    private string questionDescription {get; set;}
    private Choice[] choices {get; set;}
    private level QuestionLevel {get; set;}
    private questionType type {get; set;}
    private bool answered {get; set;}

    public Question(string npcname, string questionDesc, Choice[] choice, level questLevel, questionType type)
    {
        this.npcName = npcname;
        this.questionDescription = questionDesc;
        this.choices = choice;
        this.QuestionLevel = questLevel;
        this.type = type;
        answered = false;
    }

    public string getnpcName()
    {
        return npcName;
    }

    public string getQuestionDescription()
    {
        return questionDescription;
    }

    public questionType getQuestionType()
    {
        return type;
    }

    public Choice[] getChoices()
    {
        return choices;
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
