using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/MoveCollective")]
public class MoveCollective : SO_StateAction
{
    public float RepathDistance = 2f;
    public float ArrivalDistance = 1.2f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var ai = deerFSM.DeerAI;
        if (ai == null || ai.Agent == null || !ai.Agent.isOnNavMesh)
            return;

        var herd = ai.Herd;
        if (herd == null || herd.CurrentPOI == null)
            return;

        var bb = deerFSM.DeerBlackboard;
        var agent = ai.Agent;
        
        if (!bb.HasDestination)
        {
            Vector3 herdTarget = herd.CurrentPOI.position;
            
            Vector3 direction = (herdTarget - ai.transform.position).normalized;
            direction = ai.ApplyHerdDirection(direction);
            
            bb.GoalPoint = herdTarget + direction * Random.Range(0.5f, 2f);
            bb.HasDestination = true;

            agent.SetDestination(bb.GoalPoint);
            return;
        }

        float sqrDist = (ai.transform.position - bb.GoalPoint).sqrMagnitude;

        if (sqrDist <= ArrivalDistance * ArrivalDistance)
        {
            agent.ResetPath();
            bb.HasDestination = false;
            bb.TimeAtDestination += Time.deltaTime;
            return;
        }

        if (!agent.hasPath ||
            Vector3.Distance(agent.destination, bb.GoalPoint) > RepathDistance)
        {
            agent.SetDestination(bb.GoalPoint);
        }
    }
}
