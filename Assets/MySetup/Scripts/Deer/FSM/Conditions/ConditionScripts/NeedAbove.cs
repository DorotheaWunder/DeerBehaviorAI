using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Conditions/NeedAbove")]
public class NeedAbove : SO_StateCondition
{
    public NeedType NeedType;

    public override bool EvaluateCondition(DeerFSM deerFSM)
    {
        var need = deerFSM.DeerAI.Needs.GetNeed(NeedType);
        return need.Value >= need.NeedProfile.HighThreshold;
    }
}
