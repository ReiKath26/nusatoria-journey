using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Mission
{
    public int getMissionNumber();

    public int objectiveCleared();

    public int requiredObjective();

    public string getMissionPrompt();

    public Lore[] getLore();

    public void OnTriggerMission();

    public void OnFinishObjectives();

    public bool OnCheckQuestFinished();

}
