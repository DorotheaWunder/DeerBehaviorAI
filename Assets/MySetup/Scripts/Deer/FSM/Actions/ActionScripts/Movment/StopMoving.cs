using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/StopMoving")]
public class StopMoving : SO_StateAction
{
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;
        bb.Mode = MovementMode.Stop;
    }
}
