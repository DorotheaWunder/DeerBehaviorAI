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

    private Vector3 _lastDestination;
    private bool _hasSetDestination;

    private void Awake()
    {
        if (!FSM) FSM = GetComponent<DeerFSM>();
        if (!Agent) Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        Agent.avoidancePriority = Random.Range(30, 70);

        Agent.speed *= Random.Range(0.9f, 1.1f);

        _hasSetDestination = false;
    }

    private void Update()
    {
        switch (BB.MovementIntent)
        {
            case MovementIntent.MoveToPosition:
                HandleMoveToPosition();
                break;

            case MovementIntent.MoveTowards:
                HandleMoveTowards();
                break;

            case MovementIntent.MoveAway:
                HandleMoveAway();
                break;

            case MovementIntent.Stop:
                HandleStop();
                break;

            case MovementIntent.None:
            default:
                break;
        }
    }

    private void HandleMoveToPosition()
    {
        if (!BB.HasGoal)
            return;

        if (!_hasSetDestination || NeedsRepath(BB.GoalPosition))
        {
            Agent.SetDestination(BB.GoalPosition);
            _lastDestination = BB.GoalPosition;
            _hasSetDestination = true;
            BB.HasDestination = true;
        }
        
        if (!Agent.pathPending &&
            Agent.remainingDistance <= Agent.stoppingDistance)
        {
            BB.HasArrived = true;
            BB.HasDestination = false;
            BB.MovementIntent = MovementIntent.None;

            _hasSetDestination = false;
        }
        else
        {
            BB.HasArrived = false;
        }
    }

    private void HandleMoveTowards()
    {
        if (BB.GoalDirection == Vector3.zero)
            return;

        Vector3 target = transform.position + BB.GoalDirection.normalized * 5f;

        if (!_hasSetDestination || NeedsRepath(target))
        {
            Agent.SetDestination(target);
            _lastDestination = target;
            _hasSetDestination = true;
        }
    }

    private void HandleMoveAway()
    {
        if (BB.GoalDirection == Vector3.zero)
            return;

        Vector3 target = transform.position - BB.GoalDirection.normalized * 5f;

        if (!_hasSetDestination || NeedsRepath(target))
        {
            Agent.SetDestination(target);
            _lastDestination = target;
            _hasSetDestination = true;
        }
    }

    private void HandleStop()
    {
        if (Agent.hasPath)
            Agent.ResetPath();

        BB.HasDestination = false;
        BB.HasArrived = false;

        _hasSetDestination = false;
    }

    private bool NeedsRepath(Vector3 newDestination)
    {
        if (!Agent.hasPath)
            return true;

        if (Agent.pathStatus != NavMeshPathStatus.PathComplete)
            return true;

        float dist = Vector3.Distance(_lastDestination, newDestination);
        return dist > BB.RepathDistanceThreshold;
    }
}
