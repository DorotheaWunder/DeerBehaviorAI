using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Conditions/RangeHerd")]
public class RangeConditionHerd : SO_StateCondition //maybe have combined condition later?
{
    public RangeExpectation Expectation = RangeExpectation.InRange;

    public override bool EvaluateCondition(DeerFSM deerFSM)
    {
        var herd = deerFSM.DeerAI?.Herd;
        var cohesion = herd?.CohesionManager;

        if (cohesion == null)
            return false;

        Transform deer = deerFSM.transform;

        bool isOutside = cohesion.IsOutsideMaxCenterRange(deer);
        bool isInside = !isOutside;

        return Expectation == RangeExpectation.InRange
            ? isInside
            : isOutside;
    }
}