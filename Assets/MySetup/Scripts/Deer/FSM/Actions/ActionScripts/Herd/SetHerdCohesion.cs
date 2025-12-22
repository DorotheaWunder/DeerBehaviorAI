using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/SetHerdCohesion")]
public class NewBehaviourScript : SO_StateAction
{
    [Range(0.1f, 2f)] public float CenterMultiplier = 1.3f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (RunOncePerState && executedThisState) return;
        
        var herd = deerFSM.DeerAI.Herd;
        if (herd == null || herd.CohesionManager == null)
            return;

        herd.CohesionManager.CenterRangeMultiplier = CenterMultiplier;
        
        executedThisState = true;
    }
}
