using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

public class Mission : ScriptableObject
{

    public int missionNumber = 0;

    public abstract class QuestGoal: ScriptableObject
    {
        protected string description;
        public int currentAmount {get; protected set;}
        public int requiredAmount = 1;

        public bool completed {get; protected set; }
        [HideInInspector] public UnityEvent goalCompleted;

        public virtual string getDescription()
        {
            return description;
        }

        public virtual void initialize()
        {
            completed = false;
            goalCompleted = new UnityEvent();
        }

        protected void Evaluate()
        {
            if(currentAmount >= requiredAmount)
            {
                Complete();
            }
        }

        private void Complete()
        {
            completed = true;
            goalCompleted.Invoke();
            goalCompleted.RemoveAllListeners();
        }
    }

    public List<QuestGoal> Goals;

    public void initialize()
    {
        completed = false;
        missionCompleted = new MissionCompletedEvent();

        foreach(var goal in Goals)
        {
            goal.initialize();
            goal.goalCompleted.AddListener(delegate {CheckGoals(); });
        }
    }

    private void CheckGoals()
    {
        completed = Goals.All(g => g.completed);

        if(completed)
        {
            missionCompleted.Invoke(this);
            missionCompleted.RemoveAllListeners();
        }
    }

    public bool completed {get; protected set; }
    public MissionCompletedEvent missionCompleted;
}

public class MissionCompletedEvent: UnityEvent<Mission>{}

#if UNITY_EDITOR

[CustomEditor(typeof(Mission))]

public class MissionEditor: Editor 
{
    List<string> m_QuestGoalType;
    SerializedProperty m_QuestGoalListProperty;

    [MenuItem("Assets/Mission", priority = 0)]
    public static void CreateMission()
    {
        var newMission = CreateInstance<Mission>();

        ProjectWindowUtil.CreateAsset(newMission, "mission.asset");
    }

    void OnEnable()
    {
        m_QuestGoalListProperty = serializedObject.FindProperty(nameof(Mission.Goals));

        var lookUp = typeof(Mission.QuestGoal);

        m_QuestGoalType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).
        Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookUp)).Select(type => type.Name).ToList();
    }

    public override void OnInspectorGUI()
    {
        int choice = EditorGUILayout.Popup("Add new quest goal", -1, m_QuestGoalType.ToArray());

        if (choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(m_QuestGoalType[choice]);

            AssetDatabase.AddObjectToAsset(newInstance, target);

            m_QuestGoalListProperty.InsertArrayElementAtIndex(m_QuestGoalListProperty.arraySize);
            m_QuestGoalListProperty.GetArrayElementAtIndex(m_QuestGoalListProperty.arraySize - 1).objectReferenceValue = newInstance;
        }

        Editor ed = null;
        int toDelete = -1;

        for(int i = 0; i < m_QuestGoalListProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();

            var item = m_QuestGoalListProperty.GetArrayElementAtIndex(i);
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);

            Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);

            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            if(GUILayout.Button("-", GUILayout.Width(32)))
            {
                toDelete = i;
            }
            EditorGUILayout.EndHorizontal();

        }

        if(toDelete != -1)
        {
            var item = m_QuestGoalListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item, true);

            m_QuestGoalListProperty.DeleteArrayElementAtIndex(toDelete);
            m_QuestGoalListProperty.DeleteArrayElementAtIndex(toDelete);
        }

    serializedObject.ApplyModifiedProperties();
      
    }
}

#endif
