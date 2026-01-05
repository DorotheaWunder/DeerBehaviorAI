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
        if (!BB.HasGoal && BB.MovementIntent != MovementIntent.Stop)
            return;

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
        
        ApplyMovementYielding();
        ApplyIdleYielding();
    }

    private void MoveToPosition()
    {
        if (NeedsRepath(BB.GoalPosition))
            Agent.SetDestination(BB.GoalPosition);

        float distance = Vector3.Distance(transform.position, BB.GoalPosition);
        if (distance < 1f) 
        {
            BB.HasArrived = true;
            BB.HasDestination = false;
            BB.MovementIntent = MovementIntent.None;
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
    
    private void ApplyMovementYielding()
    {
        if (BB.MovementIntent == MovementIntent.Stop) return;

        HerdCohesionManager herd = BB.DeerAI?.Herd?.CohesionManager;
        if (herd == null) return;

        float lateralOffset = 0f;
        Vector3 yieldDirection = Vector3.zero;

        foreach (var other in BB.DeerAI.Herd.DeerList)
        {
            if (other == BB.DeerAI) continue;

            Vector3 toOther = other.transform.position - transform.position;
            float distance = toOther.magnitude;

            if (distance < herd.MinToOtherDeer)
            {
                Vector3 moveDir = Agent.velocity.normalized;
                Vector3 perp = Vector3.Cross(Vector3.up, moveDir).normalized;

                float side = Vector3.Dot(perp, toOther) > 0 ? 1f : -1f;

                float strength = (herd.MinToOtherDeer - distance) / herd.MinToOtherDeer;

                yieldDirection += perp * side * strength;
            }
        }

        if (yieldDirection != Vector3.zero)
        {
            Vector3 nudge = yieldDirection.normalized * 0.5f;
            Agent.velocity += nudge;
            Agent.velocity *= 0.9f;
        }
    }
    
    private void ApplyIdleYielding()
    {
        if (BB.MovementIntent != MovementIntent.Stop)
            return;

        HerdCohesionManager herd = BB.DeerAI?.Herd?.CohesionManager;
        if (herd == null) return;

        Vector3 totalOffset = Vector3.zero;

        foreach (var other in BB.DeerAI.Herd.DeerList)
        {
            if (other == BB.DeerAI) continue;

            NavMeshAgent otherAgent = other.GetComponent<NavMeshAgent>();
            if (otherAgent == null || otherAgent.velocity.sqrMagnitude < 0.01f)
                continue;

            float distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance > herd.MinToOtherDeer)
                continue;
            
            if (!herd.IsApproaching(transform, otherAgent))
                continue;
            
            Vector3 moveDir = otherAgent.velocity.normalized;
            Vector3 toSelf = transform.position - other.transform.position;

            Vector3 lateral = Vector3.Cross(Vector3.up, moveDir).normalized;
            float side = Vector3.Dot(lateral, toSelf) > 0 ? 1f : -1f;

            float strength = (herd.MinToOtherDeer - distance) / herd.MinToOtherDeer;

            totalOffset += lateral * side * strength;
        }

        if (totalOffset != Vector3.zero)
        {
            transform.position += totalOffset.normalized * Time.deltaTime * 0.5f;
        }
    }
}
