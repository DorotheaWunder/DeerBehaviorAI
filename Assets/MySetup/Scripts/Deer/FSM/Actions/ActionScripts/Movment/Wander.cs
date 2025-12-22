using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/Wander")]
public class Wander : SO_StateAction
{
    [Header("Area Settings")]
    public float MinRadius = 2f;
    public float MaxRadius = 5f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (RunOncePerState && executedThisState) return;

        var bb = deerFSM.DeerBlackboard;
        
        bb.Mode = MovementMode.Wander;

        bb.MinRadius = MinRadius;
        bb.MaxRadius = MaxRadius;

        executedThisState = true;
    }
}
