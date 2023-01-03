using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgementGoal : Goal
{
    private float score;
    private GameObject recipient;
    private string recipientName {get; set;}
    private bool isMain;

    public JudgementGoal(string desc, int required, string rec, bool main)
    {
        this.recipientName = rec;
        base.initialize(desc, required, null);
        this.isMain = main;
            
        recipient = GameObject.Find(this.recipientName);
        score = 0f;
    }

    public GameObject getRecipient()
    {
        return recipient;
    }

    public void OnAnswerQuestion(Question question, int choice, int number)
    {
       finishObjective();
       float addToScore = question.answerQuestion(choice, number);
       score+= addToScore;
       evaluate();
    }

    public bool getMain()
    {
        return isMain;
    }

    public float getFinalScore()
    {
        return score;
    }

}
