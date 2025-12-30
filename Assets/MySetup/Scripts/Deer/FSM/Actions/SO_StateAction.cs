using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_StateAction : ScriptableObject
{
    [Header("Execution Settings")]
    public bool IsOneShotAction = true;
    [HideInInspector] public bool _hasExecuted = false;
    
    public abstract void ExecuteAction(DeerFSM deerFSM);
    
    public void ResetExecution()
    {
        _hasExecuted = false;
    }
    //have overwrite animation here?
}
