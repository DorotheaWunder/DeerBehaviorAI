using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/FacePlayerDirection")]
public class FacePlayerDirection : SO_StateAction
{
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (IsOneShotAction && _hasExecuted) return;
        
        if (deerFSM.DeerAI.Player != null)
            deerFSM.transform.LookAt(deerFSM.DeerAI.Player.transform);
        
        _hasExecuted = true;
    }
}
