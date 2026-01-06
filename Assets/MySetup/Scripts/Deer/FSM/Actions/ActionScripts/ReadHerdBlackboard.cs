using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/ReadHerdBlackboard")]
public class ReadHerdBlackboard : SO_StateAction
{
    
    public float FleeDistance = 20f; // optional, distance to run from player

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var deer = deerFSM.DeerAI;
        var bb = deerFSM.DeerBlackboard;
        var herdBB = deer.Herd.HerdBB;
        var cohesion = deer.Herd.CohesionManager;

        if (herdBB == null) return;

        switch (herdBB.MovementIntent)
        {
            case HerdMovementIntent.Migrate:
                // For migration, anchor is the shared herd goal position (POI)
                if (herdBB.HasGoal)
                {
                    bb.AnchorPosition = herdBB.GoalPosition;
                }
                break;

            case HerdMovementIntent.Flee:
                // For fleeing, anchor is the deer-specific position in flee direction
                Vector3 fleeDir = cohesion.GetHerdFleeDirection(herdBB.PlayerPosition);
                Vector3 finalDir = deer.ApplyHerdDirection(fleeDir).normalized;

                bb.AnchorPosition = deer.transform.position + finalDir * FleeDistance;
                break;

            case HerdMovementIntent.None:
            default:
                bb.AnchorPosition = Vector3.zero;
                break;
        }

        // Optional debug
        Debug.Log($"Deer {deerFSM.name} sets anchor from herd: intent={herdBB.MovementIntent}, anchor={bb.AnchorPosition}");
    }
}
