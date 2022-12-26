using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

public class Story : ScriptableObject
{
    public int storyNumber = 0;

    public abstract class Dialogs: ScriptableObject
    {
        public int dialogNumber = 0;
        protected string nameText;
        protected string dialogText;

        public bool canContinueNextLine {get; protected set;}
        [HideInInspector] public UnityEvent triggerNextLine;
    }

    public List<Dialogs> dialogs;

        public bool completed {get; protected set; }
        public StoryCompletedEvent storyCompleted;
}

public class StoryCompletedEvent: UnityEvent<Story>{}
