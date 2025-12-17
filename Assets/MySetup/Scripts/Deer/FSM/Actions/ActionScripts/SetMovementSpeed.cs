using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/SetMovementSpeed")]
public class SetMovementSpeed : SO_StateAction
{
    public float Speed = 6f;
    public float Acceleration = 12f;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        var agent = deerFSM.DeerAI.Agent;
        if (agent == null) return;

        agent.speed = Speed;
        agent.acceleration = Acceleration;
    }
}
