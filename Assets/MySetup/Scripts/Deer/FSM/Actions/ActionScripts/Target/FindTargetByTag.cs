using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/FindTargetByTag")]
public class FindTargetByTag : SO_StateAction//maybe alter later to select one of many
{
    public string TargetTag;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (RunOncePerState && executedThisState) return;
        
        var bb = deerFSM.DeerBlackboard;

        if (bb.HasGoal) return;

        var target = GameObject.FindGameObjectWithTag(TargetTag);
        if (target == null) return;

        bb.FollowTarget = target.transform;
        
        executedThisState = true;
    }
}
