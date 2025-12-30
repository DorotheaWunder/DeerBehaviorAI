using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/RefillNeed")]
public class RefillNeed : SO_StateAction
{
    public NeedType NeedType;
    
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (IsOneShotAction && _hasExecuted) return;
        
        var need = deerFSM.DeerAI.Needs.GetNeed(NeedType);
        need.Recover(Time.deltaTime);
        
        _hasExecuted = true;
    }
}
