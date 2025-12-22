using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/FacePlayerDirection")]
public class FacePlayerDirection : SO_StateAction
{
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (RunOncePerState && executedThisState) return;
        
        if (deerFSM.DeerAI.Player != null)
            deerFSM.transform.LookAt(deerFSM.DeerAI.Player.transform);
        
        executedThisState = true;
    }
}
