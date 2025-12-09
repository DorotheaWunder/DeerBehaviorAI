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
        if (ai.Agent == null) return;

        var target = GameObject.FindGameObjectWithTag(TargetTag);
        if(target == null) return;

        ai.Agent.SetDestination(target.transform.position);
    }
}
