using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/CalculateGoalPosition")]
public class CalculateGoalPosition : SO_StateAction
{
    public float MinRadius;
    public float MaxRadius;
    
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;

        if (bb.AnchorPosition == Vector3.zero)
            return;

        float radius = Random.Range(MinRadius, MaxRadius);
        Vector2 randomCircle = Random.insideUnitCircle * radius;

        Vector3 offset = new Vector3(randomCircle.x, 0f, randomCircle.y);
        bb.GoalPosition = bb.AnchorPosition + offset;

        bb.HasGoal = true;
        bb.TimeSinceLastWanderPoint = 0f;
    }
}
