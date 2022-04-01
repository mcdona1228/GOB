using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSeeker : MonoBehaviour
{
    Goal[] myGoals;
    Action[] myActions;
    Action myChangeOverTime;
    const float TICK_LENGTH = 10f;

    void Start()
    {
        myGoals = new Goal[3];
        myGoals[0] = new Goal("Eat", 4);
        myGoals[1] = new Goal("Sleep", 3);
        myGoals[2] = new Goal("Bathroom", 3);


        myActions = new Action[3];
        myActions[0] = new Action("eating food");
        myActions[0].targetGoals.Add(new Goal("Eat", -2));
        myActions[0].targetGoals.Add(new Goal("Sleep", +3));
        myActions[0].targetGoals.Add(new Goal("Bathroom", +1));

        myActions[1] = new Action("taking a nap");
        myActions[1].targetGoals.Add(new Goal("Eat", +1));
        myActions[1].targetGoals.Add(new Goal("Sleep", -4));
        myActions[1].targetGoals.Add(new Goal("Bathroom", +2));

        myActions[2] = new Action("using the bathroom");
        myActions[2].targetGoals.Add(new Goal("Eat", +2));
        myActions[2].targetGoals.Add(new Goal("Sleep", +1));
        myActions[2].targetGoals.Add(new Goal("Bathroom", -3));

        Debug.Log("Starting clock. A single hour will pass every " + TICK_LENGTH + " seconds.");
        InvokeRepeating("TimeTick", 0f, TICK_LENGTH);

        Debug.Log("Hit W to begin actions");
    }

    void TimeTick()
    {
        //rateSinceLastTime = changeSinceLastTime / timeSinceLast
        //basicRate = 0.95 * basicRate + 0.05 * rateSinceLastTime

        foreach(Goal goal in myGoals)
        {
            goal.Value += myChangeOverTime.GetGoalChange(goal);
            goal.Value = Mathf.Max(goal.Value, 0);
        }
    }

    void PrintGoals()
    {
        string stringGoal = "";
        foreach (Goal goal in myGoals)
        {
            stringGoal += goal.Name + ": " + goal.Value + "; ";
        }
        stringGoal += "Discomfort: " + CurrentDiscomfort();
        Debug.Log(stringGoal);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Action bestAction = DecideAction(myActions, myGoals);
            Debug.Log("I feel like " + bestAction.Name);

            foreach(Goal goal in myGoals)
            {
                goal.Value += bestAction.GetGoalChange(goal);
                goal.Value = Mathf.Max(goal.Value, 0);
            }
            PrintGoals();
        }
    }
    Action DecideAction(Action[] actions, Goal[] goals)
    {
        Action urgentAction = null;
        float urgentValue = float.PositiveInfinity;

        foreach (Action action in actions)
        {
            float thisValue = Discomfort(action, goals);

            if(thisValue < urgentValue)
            {
                urgentValue = thisValue;
                urgentAction = action;
            }
        }
        return urgentAction;
    }
    float CurrentDiscomfort()
    {
        float total = 0;
        foreach(Goal goal in myGoals)
        {
            total += (goal.Value * goal.Value);
        }
        return total;
    }
    float Discomfort(Action action, Goal[] goals)
    {
        float discomfort = 0;
        foreach(Goal goal in goals)
        {
            float newValue = goal.Value + action.GetGoalChange(goal);
            newValue = Mathf.Max(newValue);

            discomfort += goal.GainDiscomfort(newValue);
        }
        return discomfort;
    }
}
