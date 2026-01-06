using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/SetHerdFleeDirection")]
public class SetFleeDirection : SO_StateAction
{
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;
        var ai = deerFSM.DeerAI;

        if (ai?.Player == null || ai.Herd?.CohesionManager == null)
            return;

        Vector3 herdDir =
            ai.Herd.CohesionManager.GetHerdFleeDirection(
                ai.Player.transform.position);

        bb.GoalDirection = herdDir;
        bb.HasDestination = herdDir != Vector3.zero;
    }
}
