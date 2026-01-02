using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/CalculateGoalDirection")]
public class CalculateGoalDirection : SO_StateAction
{
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;
        Vector3 selfPos = deerFSM.transform.position;

        Vector3 dir = (selfPos - bb.AnchorPosition).normalized;

        bb.GoalDirection = dir;
        bb.HasGoal = true;
    }
}
