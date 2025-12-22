using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/SetTagTargetType")]
public class SetTagTargetType : SO_StateAction
{
    public MovementTargetType TargetType = MovementTargetType.Point;
    
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (RunOncePerState && executedThisState) return;
        
        var bb = deerFSM.DeerBlackboard;

        if (bb.HasGoal) return;
        
        bb.TargetType = TargetType;
        
        executedThisState = true;
    }
}
