using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Mission
{
    public int missionNumber {get; }
    public string missionPrompt {get; }
    public int requiredNumber {get; }

    public bool OnTriggerQuestCheckFinished();


}
