using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/MoveToTagTarget")]
public class MoveToTagTarget : SO_StateAction
{
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (RunOncePerState && executedThisState) return;
        
        var bb = deerFSM.DeerBlackboard;

        if (bb.Mode == MovementMode.Stop) return;
        
        switch (bb.TargetType)
        {
            case MovementTargetType.Point:
                bb.GoalPoint = bb.FollowTarget.position;
                break;

            case MovementTargetType.Area:
                Vector3 center = bb.FollowTarget.position;
                float radius = Random.Range(bb.MinRadius, bb.MaxRadius);
                Vector2 offset = Random.insideUnitCircle * radius;
                bb.GoalPoint = center + new Vector3(offset.x, 0, offset.y);
                break;

            case MovementTargetType.Direction:
                bb.GoalPoint = bb.Direction + deerFSM.transform.position;
                break;
        }

        bb.HasGoal = true;
        bb.HasDestination = false;
        bb.TimeAtDestination = 0f;
        
        executedThisState = true;
    }
}
