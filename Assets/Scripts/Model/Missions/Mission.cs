using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuests
{

    string missionDesc {get; }

    bool onTriggerQuest();

    bool completeQuest();
}
