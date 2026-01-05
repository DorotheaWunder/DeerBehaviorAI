using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeerBlackboard 
{    
    public DeerAI DeerAI;
    
    [Header("Player Detection")]
    public Transform Player;
    public float TimePlayerVisible;
    public bool HasLineOfSight;

    [Header("Navigation")]
    public MovementIntent MovementIntent;
    public Vector3 AnchorPosition;
    public Vector3 GoalPosition;
    public Vector3 GoalDirection;

    [Header("Wander Settings")]
    public float WanderCooldown;
    public float TimeSinceLastWanderPoint;
    
    [Header("Runtime")]
    public bool HasGoal;
    public bool HasDestination;
    public bool HasArrived; 
    public float TimeAtDestination;

    [Header("Repathing")]
    public float DestinationUpdateCooldown = 0.5f;
    public float RepathDistanceThreshold = 3f;
    
    [Header("Senses")]
    public Transform SensesRoot;
    public Transform Hearing;
    public Transform Sight;
    // public GameObject Smell;
    
    [Header("Debug")]
    public bool DebugDrawDestination;

//--------------------------------------------------------- clear blackboard
    public void ClearNavigation()
    {
        HasGoal = false;
        HasDestination = false;
        HasArrived = false;

        AnchorPosition = Vector3.zero;
        GoalPosition = Vector3.zero;
        GoalDirection = Vector3.zero;

        MovementIntent = MovementIntent.None;
        TimeAtDestination = 0f;
    }
}
