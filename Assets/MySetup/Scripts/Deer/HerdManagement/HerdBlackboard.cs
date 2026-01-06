using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdBlackboard : MonoBehaviour
{
    public Vector3 PlayerPosition;
    public Vector3 GoalPosition;
    public Vector3 AnchorPosition;
    
    public HerdMovementIntent MovementIntent;
    
    public bool HasGoal;
    public bool HasArrived;

    public int Version;
    
    public void Clear()
    {
        GoalPosition = Vector3.zero;
        PlayerPosition = Vector3.zero;
        AnchorPosition = Vector3.zero;
        HasGoal = false;
        HasArrived = false;
        MovementIntent = HerdMovementIntent.None;
    }
}
