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
        var bb = deerFSM.DeerBlackboard;

        bb.Mode = MovementMode.Navigate;
        bb.TargetType = MovementTargetType.Area;

        bb.MinRadius = MinRadius;
        bb.MaxRadius = MaxRadius;
    }
}
