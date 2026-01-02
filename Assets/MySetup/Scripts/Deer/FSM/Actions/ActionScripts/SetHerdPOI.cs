using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/SetHerdPOI")]
public class SetHerdPOI : SO_StateAction
{
    public string TargetTag;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var herd = deerFSM.DeerAI.Herd;
        if (herd == null)
            return;
        
        var poi = GameObject.FindWithTag(TargetTag);

        if (poi != null)
        {
            herd.CurrentPOI = poi.transform;
        }
        else
        {
            herd.CurrentPOI = null;
        }
    }
}
