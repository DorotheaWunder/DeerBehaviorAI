using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Conditions/SuspicionAbove")]
public class SuspicionAbove : SO_StateCondition
{
    public float Threshold;

    public override bool EvaluateCondition(DeerFSM deerFSM)
    {
        return deerFSM.DeerAI.TotalSuspicion >= Threshold;
    }
}
