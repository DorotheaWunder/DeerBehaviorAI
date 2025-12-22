using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/MoveAwayFromTagTarget")]
public class MoveAwayFromTagTarget : SO_StateAction
{
    public Transform Threat;
    public float FleeDistance = 20f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (RunOncePerState && executedThisState) return;
        
        var bb = deerFSM.DeerBlackboard;
        if (Threat == null) return;

        Vector3 dir = (deerFSM.transform.position - Threat.position).normalized;

        bb.Mode = MovementMode.Flee;
        bb.TargetType = MovementTargetType.Direction;
        bb.Direction = dir;
        bb.GoalPoint = deerFSM.transform.position + dir * FleeDistance;

        bb.HasGoal = true;
        bb.HasDestination = false;
        
        executedThisState = true;
    }
}
