using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgementGoal : Goal
{
    private float score;
    private GameObject recipient;
    private string recipientName {get; set;}

    public JudgementGoal(string desc, int current, int required, string rec)
    {
        this.recipientName = rec;
        base.initialize(desc, current, required, null);
            
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

    public float getFinalScore()
    {
        return score;
    }

}
