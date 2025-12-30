using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Conditions/RangeDeer")]
public class RangeConditionDeer : SO_StateCondition
{
    public RangeExpectation Expectation = RangeExpectation.InRange;

    public override bool EvaluateCondition(DeerFSM deerFSM)
    {
        var herd = deerFSM.DeerAI?.Herd;
        var cohesion = herd?.CohesionManager;

        if (cohesion == null)
            return false;

        Transform deer = deerFSM.transform;

        bool tooFar = cohesion.IsTooFarFromNearestDeer(deer);
        bool inRange = !tooFar;

        return Expectation == RangeExpectation.InRange
            ? inRange
            : tooFar;
    }
}
