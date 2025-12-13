using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/MoveToTarget")]
public class MoveTowards : SO_StateAction
{
    public string TargetTag;
    
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var ai = deerFSM.DeerAI;
        var agent = ai.Agent;

        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            return;

        var target = GameObject.FindGameObjectWithTag(TargetTag);
        if (target == null)
            return;

        agent.SetDestination(target.transform.position);
    }
}
