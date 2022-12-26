using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public string eventDescription;
}

public class InteractionEvent: GameEvent
{
    public GameObject interactorName;

    public InteractionEvent(GameObject name)
    {
        interactorName = name;
    }

}

public class GatheringGameEvent: GameEvent
{
    public GameObject itemObject;

    public GatheringGameEvent(GameObject gathereditem)
    {
        itemObject = gathereditem;
    }
}

public class SubmitItemEvent: GameEvent
{
    public string[] submitItemName;

    public SubmitItemEvent(string[] itemName)
    {
        submitItemName = itemName;
    }
}

