using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgementGoal : Goal
{
    private float score;
    private Question[] questions;
    private GameObject recipient;
    private string recipientName {get; set;}

    public void initialize(string desc, int current, int required, string rec)
    {
        this.recipientName = rec;
        base.initialize(desc, current, required, null);
            
        recipient = GameObject.Find(this.recipientName);
        score = 0f;
    }

    public void OnAnswerQuestion(int choice, int number)
    {
       finishObjective();
       float addToScore = questions[number].answerQuestion(choice, questions.Length);
       score+= addToScore;
       evaluate();
    }

    public float getFinalScore()
    {
        return score;
    }

}
