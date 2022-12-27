using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgementGoal : Goal
{
    private float score;

    [SerializeField] private AnimationCurve good;
    [SerializeField] private AnimationCurve decent;
    [SerializeField] private AnimationCurve poor;

    private float goodValue = 0f;
    private float decentValue = 0f;
    private float poorValue = 0f;

    private int questionAnswered = 0;
    [SerializeField] private int requiredQuestion;

    public override void initialize()
    {
        base.initialize();
        score = 0f;
 
    }

    public void OnAnswerQuestion(float gotScore)
    {
        score += gotScore;
        questionAnswered++;

        if(questionAnswered == requiredQuestion)
        {
            evaluateResult();
        }
    }

    private int evaluateResult()
    {
        goodValue = good.Evaluate(score);
        decentValue = decent.Evaluate(score);
        poorValue = poor.Evaluate(score);

        SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

        if(goodValue > decentValue && goodValue > poorValue)
        {
            slot.understandingLevel = 3;
        }

        else if(decentValue >= goodValue && decentValue > poorValue )
        {
            slot.understandingLevel = 2;
        }

        else if(poorValue >= decentValue)
        {
            slot.understandingLevel = 1;
        }

        return understandingLevel;
    }
}
