using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_StateAction : ScriptableObject
{
    public abstract void ExecuteAction(DeerFSM deerFSM);
}
