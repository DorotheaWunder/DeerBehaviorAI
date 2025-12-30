using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/MoveAwayFromTagTarget")]
public class MoveAwayFromTagTarget : SO_StateAction
{
    public float FleeDistance = 20f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;
        if (bb.Target == null) return;
        
        Vector3 dir = (deerFSM.transform.position - bb.Target.position);
        if (dir.sqrMagnitude < 0.001f) dir = Random.insideUnitSphere;
        dir.Normalize();
        
        bb.Mode = MovementMode.Navigate;
        bb.TargetType = MovementTargetType.Direction;
        bb.Direction = dir * FleeDistance;
        bb.HasGoal = true;
        bb.HasDestination = false;
        
        bb.GoalPoint = deerFSM.transform.position + bb.Direction;
    }
}
