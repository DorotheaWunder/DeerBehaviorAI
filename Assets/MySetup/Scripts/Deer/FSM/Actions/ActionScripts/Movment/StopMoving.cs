using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/StopMoving")]
public class StopMoving : SO_StateAction
{
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (RunOncePerState && executedThisState) return;
        
        var agent = deerFSM.DeerAI.Agent;
        if (agent == null) return;
        
        if(agent.hasPath) agent.ResetPath();
        
        executedThisState = true;
    }
}
