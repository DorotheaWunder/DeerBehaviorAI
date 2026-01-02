using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/MoveCollective")]
public class MoveCollective : SO_StateAction
{
    public float RepathDistance = 2f;
    public float ArrivalDistance = 1.2f;
    public float SpreadRadiusMin = 0.5f;
    public float SpreadRadiusMax = 2f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var ai = deerFSM.DeerAI;
        if (ai == null || ai.Agent == null || !ai.Agent.isOnNavMesh) return;

        var herd = ai.Herd;
        if (herd == null || herd.CurrentPOI == null) return;

        var bb = deerFSM.DeerBlackboard;
        var agent = ai.Agent;
        
        if (!bb.HasDestination && !bb.HasArrived)
        {
            Vector3 herdTarget = herd.CurrentPOI.position;

            Vector2 randomCircle = Random.insideUnitCircle;
            float radius = Random.Range(SpreadRadiusMin, SpreadRadiusMax);
            Vector3 offset = new Vector3(randomCircle.x, 0f, randomCircle.y) * radius;

            bb.GoalPosition = herdTarget + offset;
            bb.HasDestination = true;

            agent.SetDestination(bb.GoalPosition);
            return;
        }

        float sqrDist = (ai.transform.position - bb.GoalPosition).sqrMagnitude;

        if (sqrDist <= ArrivalDistance * ArrivalDistance)
        {
            agent.ResetPath();
            bb.HasDestination = false;
            bb.HasArrived = true;
            return;
        }
        
        if (!agent.hasPath || Vector3.Distance(agent.destination, bb.GoalPosition) > RepathDistance)
        {
            agent.SetDestination(bb.GoalPosition);
        }
    }
}
