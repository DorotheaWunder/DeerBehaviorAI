using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/SetDeerCohesion")]
public class SetDeerCohesion : SO_StateAction
{
    [Range(0.1f, 2f)] public float IndividualMultiplier = 0.8f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var herd = deerFSM.DeerAI.Herd;
        if (herd == null || herd.CohesionManager == null)
            return;
        
        herd.CohesionManager.IndividualRangeMultiplier = IndividualMultiplier;
    }
}
