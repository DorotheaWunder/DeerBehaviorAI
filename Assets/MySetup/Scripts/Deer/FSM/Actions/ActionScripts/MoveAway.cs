using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "DeerFSM/Actions/MoveFromTarget")]
public class MoveAway : SO_StateAction
{
    public float MovementDistance = 20f;
    public float RepathDistance = 10f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var ai = deerFSM.DeerAI;
        if (ai == null || ai.Player == null || ai.Agent == null)
            return;

        Vector3 deerPos = deerFSM.transform.position;
        Vector3 threatPos = ai.Player.transform.position;
        
        Vector3 herdFleeDirection =
            (ai.Herd.CohesionManager.HerdCenter.position - threatPos).normalized;

        Vector3 finalDirection =
            ai.ApplyHerdDirection(herdFleeDirection);

        Vector3 targetPosition =
            deerPos + finalDirection * MovementDistance;
        
        if (!ai.Agent.hasPath ||
            Vector3.Distance(ai.Agent.destination, targetPosition) > RepathDistance)
        {
            ai.Agent.SetDestination(targetPosition);
        }
    }
}
