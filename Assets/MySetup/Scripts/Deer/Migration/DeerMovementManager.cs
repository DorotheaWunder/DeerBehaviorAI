using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DeerMovementManager : MonoBehaviour
{
    public DeerFSM FSM;
    public NavMeshAgent Agent;
    
    private DeerBlackboard BB => FSM.DeerBlackboard;

    private void Awake()
    {
        if (!FSM) FSM = GetComponent<DeerFSM>();
        if (!Agent) Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!BB.HasGoal) return;

        switch (BB.MovementIntent)
        {
            case MovementIntent.MoveToPosition:
                MoveToPosition();
                break;

            case MovementIntent.MoveTowards:
                MoveTowards();
                break;
            
            case MovementIntent.MoveAway:
                MoveAway();
                break;

            case MovementIntent.Stop:
                Stop();
                break;
        }
    }

    private void MoveToPosition()
    {
        if (NeedsRepath(BB.GoalPosition))
            Agent.SetDestination(BB.GoalPosition);

        float distance = Vector3.Distance(transform.position, BB.GoalPosition);
        if (distance < 1f) 
        {
            BB.HasDestination = false;
            BB.HasArrived = true;       
            BB.TimeAtDestination = 0f;
        }
        else
        {
            BB.HasArrived = false;       
        }
    }
    
    private void MoveTowards()
    {
        Vector3 target = transform.position + BB.GoalDirection * 5f * -1;
        Agent.SetDestination(target);
    }

    private void MoveAway()
    {
        Vector3 target = transform.position + BB.GoalDirection * 5f;
        Agent.SetDestination(target);
    }

    private void Stop()
    {
        Agent.ResetPath();
        BB.HasDestination = false;
        BB.TimeAtDestination = 0f;
    }

    private bool NeedsRepath(Vector3 newDestination)
    {
        if (!Agent.hasPath) return true;
        return Vector3.Distance(Agent.destination, newDestination) > BB.RepathDistanceThreshold;
    }
}
