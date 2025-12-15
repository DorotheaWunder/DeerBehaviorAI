using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/NotifyHerd")]
public class NotifyHerd : SO_StateAction
{
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        Debug.Log("The herd is being notified");
    }
}
