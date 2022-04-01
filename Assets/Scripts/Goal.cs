using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal 
{
    public string Name;
    public float Value;

    public Goal(string goalName, float goalValue)
    {
        Name = goalName;
        Value = goalValue;
    }

    public float GainDiscomfort(float newValue)
    {
        return newValue * newValue;
    }
}

public class Action
{
    public string Name;
    public List<Goal> targetGoals;

    public Action(string actionName)
    {
        Name = actionName;
        targetGoals = new List<Goal>();
    }
    public float GetGoalChange(Goal goal)
    {
        foreach(Goal target in targetGoals)
        {
            if (target.Name == goal.Name)
            {
                return target.Value;
            }
        }
        return 0;
    }
}