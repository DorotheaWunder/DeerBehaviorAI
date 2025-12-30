using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/SetMovementMode")]
public class SetMovementMode : SO_StateAction
{
    public MovementMode Mode = MovementMode.Navigate;
    
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;
        bb.Mode = Mode;
    }
}
