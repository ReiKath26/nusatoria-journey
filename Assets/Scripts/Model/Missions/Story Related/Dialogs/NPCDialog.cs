using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : Dialogs
{
    private string npcObject {get; set;}

    public NPCDialog(string  npcobject, string npcName, string dialog, string[] objects)
    {
        this.npcObject = npcobject;
        base.initialize(npcName, dialog, objects);
    }

    public void lookAtPlayer(Transform playerTarget)
    {
        GameObject npc = GameObject.Find(npcObject);

        if(npc != null)
        {
            npc.transform.LookAt(playerTarget);
        }
    }
}
