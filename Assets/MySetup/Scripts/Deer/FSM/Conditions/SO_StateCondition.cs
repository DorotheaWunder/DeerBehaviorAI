using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_StateCondition : ScriptableObject
{
    public abstract bool EvaluateCondition(DeerFSM deerFSM);
}
