using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_StateAction : ScriptableObject
{
    [Header("Execution Settings")]
    public bool RunOncePerState = true;
    [HideInInspector] public bool executedThisState = false;
    
    public abstract void ExecuteAction(DeerFSM deerFSM);
    
    public void ResetExecution()
    {
        executedThisState = false;
    }
    //have overwrite animation here?
}
