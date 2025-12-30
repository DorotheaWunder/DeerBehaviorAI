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
        if (!Agent.enabled || !Agent.isOnNavMesh || BB.Mode != MovementMode.Wander)
            return;

        BB.TimeSinceLastWanderPoint += Time.deltaTime;

        bool needNewGoal = !BB.HasGoal || BB.TimeSinceLastWanderPoint >= BB.WanderCooldown;

        if (needNewGoal)
        {
            Vector3 center;

            if (BB.Target != null)
                center = BB.Target.position;
            else
                center = transform.position;

            if (BB.TargetType == MovementTargetType.Area)
            {
                float radius = Random.Range(BB.MinRadius, BB.MaxRadius);
                Vector2 offset = Random.insideUnitCircle * radius;
                BB.GoalPoint = center + new Vector3(offset.x, 0, offset.y);
            }
            else
            {
                BB.GoalPoint = center;
            }

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
        if (!Agent.enabled || !Agent.isOnNavMesh || BB.Mode == MovementMode.Stop)
            return;

        Vector3 desiredGoal = BB.GoalPoint;

        switch (BB.TargetType)
        {
            case MovementTargetType.Point:
                if (BB.Target != null)
                    desiredGoal = BB.Target.position;
                break;

            case MovementTargetType.Area:
                if (!BB.HasGoal)
                {
                    Vector3 center = BB.Target != null ? BB.Target.position : transform.position;
                    float radius = Random.Range(BB.MinRadius, BB.MaxRadius);
                    Vector2 offset = Random.insideUnitCircle * radius;
                    desiredGoal = center + new Vector3(offset.x, 0, offset.y);
                    BB.HasGoal = true;
                }
                break;

            case MovementTargetType.Direction:
                if (BB.Direction != Vector3.zero)
                    desiredGoal = transform.position + BB.Direction;
                break;
        }
        
        if (!BB.HasDestination || Vector3.Distance(Agent.destination, desiredGoal) > 0.1f)
        {
            Agent.SetDestination(desiredGoal);
            BB.HasDestination = true;
        }

        BB.GoalPoint = desiredGoal;
        
        if (!Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance)
            BB.TimeAtDestination += Time.deltaTime;
        else
            BB.TimeAtDestination = 0f;
    }

    private bool NeedsRepath(Vector3 newDestination)
    {
        if (!Agent.hasPath)
            return true;

        float dist = Vector3.Distance(Agent.destination, newDestination);
        return dist > BB.RepathDistanceThreshold;
    }

    private void Stop()
    {
        if (Agent.hasPath)
            Agent.ResetPath();

        BB.HasDestination = false;
        BB.TimeAtDestination = 0f;
    }
}

