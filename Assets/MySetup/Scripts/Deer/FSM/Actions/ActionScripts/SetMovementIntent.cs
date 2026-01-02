using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/SetMovementIntent")]
public class SetMovementIntent : SO_StateAction
{
    public MovementIntent Intent;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;

        switch (Intent)
        {
            case MovementIntent.MoveToPosition:
                if (bb.HasGoal)
                    bb.MovementIntent = MovementIntent.MoveToPosition;
                break;

            case MovementIntent.MoveTowards:
                if (bb.GoalDirection != Vector3.zero)
                    bb.MovementIntent = MovementIntent.MoveTowards;
                break;
            
            case MovementIntent.MoveAway:
                if (bb.GoalDirection != Vector3.zero)
                    bb.MovementIntent = MovementIntent.MoveAway;
                break;

            case MovementIntent.Stop:
                bb.MovementIntent = MovementIntent.Stop;
                break;

            case MovementIntent.None:
            default:
                bb.MovementIntent = MovementIntent.None;
                break;
        }

        bb.HasDestination = true;
    }
}

public enum MovementIntent
{
    None,
    MoveToPosition,
    MoveTowards,
    MoveAway,
    Stop
}