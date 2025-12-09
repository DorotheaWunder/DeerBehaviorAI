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
        if(ai.Player == null || ai.Agent == null) return;

        Vector3 deerPos = deerFSM.transform.position;
        Vector3 targetPos = ai.Player.transform.position;

        Vector3 direction = (deerPos - targetPos).normalized;
        Vector3 targetTrackingPos = deerPos + direction * MovementDistance;

        if (ai.Agent.hasPath ||
            Vector3.Distance(ai.Agent.destination, targetTrackingPos) > RepathDistance)
        {
            ai.Agent.SetDestination(targetTrackingPos);
        }
    }
}
