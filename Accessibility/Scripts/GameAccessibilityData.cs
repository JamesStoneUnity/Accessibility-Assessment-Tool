using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameAccessibilityData
{
    [Header("Basic Goals")] public List<AccessibilityGoal> basicGoals;
    [Header("Intermediate Goals")] public List<AccessibilityGoal> intermediateGoals;
    [Header("Advanced Goals")] public List<AccessibilityGoal> advancedGoals;
}

[System.Serializable]
public class AccessibilityGoal
{
    public enum GoalType { Motor, Cognitive, Vision, Hearing, Speech, General }
    public string name;
    public GoalType goalType;
    public string description;
    public bool isComplete;
    [HideInInspector]
    public bool tooltipTriggered;
    public string url;
    public bool notRelevant;
}
