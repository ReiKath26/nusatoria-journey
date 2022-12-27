using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{

    [HideInInspector] public Mission currentMission;
    [HideInInspector] public Mission.Goals currentGoal;
    public Queue<Mission.QuestGoal>  onGoingMissionGoal;

   [SerializeField] private GameObject[] missionTriggerObjects;
   [SerializeField] private GameObject missionToggle;
   [SerializeField] private TextMeshProUGUI missionText;
   
   private SaveSlots slot;

   private void Awake()
   {
        foreach(Mission mission in listMissionHere)
        {
            mission.missionCompleted.AddListener(OnMissionCompleted);
            allMissions.Enqueue(mission);
        }

        updateCurrentMissionGoal();
   }

   private void Update()
   {

   }

   public void triggerNextQuest()

   public void interact(GameObject interactedObject)
   {
        EventManager.Instance.QueueEvent(new InteractionEvent(interactedObject));
   }

   public void gather(GameObject gatheredObject)
   {
        EventManager.Instance.QueueEvent(new GatheringGameEvent(gathereddObject));
   }

   public void submit()
   {
        bool canSubmit = SaveHandler.instance.submitItem(neededItem, PlayerPrefs.GetInt("choosenSlot"));
        EventManager.Instance.QueueEvent(new SubmitItemEvent(canSubmit));
   }

   private void updateCurrentMissionGoal()
   {
        currentMission = allMissions.Dequeue();
        foreach(var goal in currentMission.goals)
        {
            goal.goalCompleted.AddListener(OnGoalCompleted);
            onGoingMissionGoal.Enqueue(goal);
        }

        currentGoal = onGoingMissionGoal.Dequeue();
        displayGoal();

   }

   private void displayGoal()
   {
        missionToggle.SetActive(true);
        missionText.text = currentMission.missionName + ": " + currentGoal.getDescription + "(" + currentGoal.currentAmount + "/" + currentGoal.requiredAmount;
   }

   public void displayPathToGoal()
   {
           //show direction to the quest's object (pathfinder)
   }

    private void OnMissionCompleted(Mission mission)
    {
        updateCurrentMissionGoal();
    }

   private void OnGoalCompleted(Mission.Goals goal)
    {
        currentGoal = onGoingMissionGoal.Dequeue();
        displayGoal();
    }
}
