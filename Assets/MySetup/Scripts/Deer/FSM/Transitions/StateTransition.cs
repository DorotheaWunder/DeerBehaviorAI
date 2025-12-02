using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateTransition
{
    public SO_StateCondition Condition;
    public SO_DeerState TargetState;
    
    public bool ShouldTransition(DeerFSM deerFSM)
    {
        return Condition != null && Condition.EvaluateCondition(deerFSM);
    }
}
