using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/CalculateFleeGoal")]
public class CalculateFleeGoalPos : SO_StateAction
{
    public float FleeDistance = 20f;
    public float LateralSpread = 5f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;
        if (bb.GoalDirection == Vector3.zero)
            return;

        Vector3 forward = bb.GoalDirection;
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        Vector3 offset =
            forward * FleeDistance +
            right * Random.Range(-LateralSpread, LateralSpread);

        bb.GoalPosition = deerFSM.transform.position + offset;
        bb.HasGoal = true;
    }
}
