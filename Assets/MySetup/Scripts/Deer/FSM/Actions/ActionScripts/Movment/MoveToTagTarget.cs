using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/MoveToTagTarget")]
public class MoveToTagTarget : SO_StateAction
{
    public float GoalTolerance = 0.2f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;

        if (bb.Mode == MovementMode.Stop || bb.Target == null)
            return;

        Vector3 newGoal = bb.GoalPoint;
        bool goalChanged = false;

        switch (bb.TargetType)
        {
            case MovementTargetType.Point:
                newGoal = bb.Target.position;
                goalChanged = !ApproximatelySame(bb.GoalPoint, newGoal);
                break;

            case MovementTargetType.Area:
                if (!bb.HasGoal)
                {
                    Vector3 center = bb.Target.position;
                    float radius = Random.Range(bb.MinRadius, bb.MaxRadius);
                    Vector2 offset = Random.insideUnitCircle * radius;
                    newGoal = center + new Vector3(offset.x, 0, offset.y);
                    goalChanged = true;
                }
                break;

            case MovementTargetType.Direction:
                newGoal = deerFSM.transform.position + bb.Direction;
                goalChanged = true;
                break;
        }

        if (!goalChanged)
            return;

        bb.GoalPoint = newGoal;
        bb.HasGoal = true;
        bb.HasDestination = false;
        bb.TimeAtDestination = 0f;
    }

    private bool ApproximatelySame(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) <= GoalTolerance * GoalTolerance;
    }
}
