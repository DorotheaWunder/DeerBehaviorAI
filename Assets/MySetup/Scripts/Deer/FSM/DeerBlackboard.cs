using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeerBlackboard 
{
    [Header("Movement Goal")]
    public MovementTargetType TargetType = MovementTargetType.None;
    public MovementMode Mode = MovementMode.Stop;

    [Header("References")]
    public Transform FollowTarget;
    public Vector3 GoalPoint;
    public Vector3 Direction;

    [Header("Area Settings")]
    public float MinRadius;
    public float MaxRadius;

    [Header("Wander Settings")]
    public float WanderCooldown;
    public float TimeSinceLastWanderPoint;
    
    [Header("Runtime")]
    public bool HasGoal;
    public bool HasDestination;
    public float TimeAtDestination;

    [Header("Repathing")]
    public float DestinationUpdateCooldown = 0.5f;
    public float RepathDistanceThreshold = 1.5f;

    [Header("Senses")]
    public Transform SensesRoot;
    public Transform Hearing;
    public Transform Sight;
    // public GameObject Smell;
    
    [Header("Debug")]
    public bool DebugDrawDestination;
}
