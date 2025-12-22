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
        if (!enabled || FSM == null || BB == null)
            return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        switch (BB.Mode)
        {
            case MovementMode.Stop:
                Stop();
                break;

            case MovementMode.Navigate:
                HandleNavigate();
                break;

            case MovementMode.Wander:
                HandleWander();
                break;
        }
    }
    
    private void HandleWander()
    {
        if (!Agent.enabled || !Agent.isOnNavMesh) return;

        BB.TimeSinceLastWanderPoint += Time.deltaTime;

        if (!BB.HasGoal || BB.TimeSinceLastWanderPoint >= BB.WanderCooldown)
        {
            Vector3 center = transform.position;
            float radius = Random.Range(BB.MinRadius, BB.MaxRadius);
            Vector2 offset = Random.insideUnitCircle * radius;
            BB.GoalPoint = center + new Vector3(offset.x, 0, offset.y);

            BB.HasGoal = true;
            BB.HasDestination = false;
            BB.TimeAtDestination = 0f;
            
            BB.WanderCooldown = Random.Range(1f, 3f);
            BB.TimeSinceLastWanderPoint = 0f;
        }
        
        HandleNavigate();
    }
    
    private void HandleNavigate()
    {
        if (!Agent.enabled || !Agent.isOnNavMesh) return;
        if (!BB.HasGoal) return;

        Vector3 destination = BB.GoalPoint;

        if (!BB.HasDestination || NeedsRepath(destination))
        {
            Agent.SetDestination(destination);
            BB.HasDestination = true;
        }

        if (Agent.remainingDistance <= Agent.stoppingDistance && !Agent.pathPending)
            BB.TimeAtDestination += Time.deltaTime;
        else
            BB.TimeAtDestination = 0f;
    }
    
    
    private bool NeedsRepath(Vector3 newDestination)
    {
        if (!Agent.hasPath)
            return true;

        float dist = Vector3.Distance(Agent.destination, newDestination);
        return dist > 1.5f;
    }
    
    private void Stop()
    {
        if (Agent.hasPath)
            Agent.ResetPath();

        BB.HasDestination = false;
        BB.TimeAtDestination = 0f;
    }
}

