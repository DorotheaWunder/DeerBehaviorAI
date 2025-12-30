using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovTrigger : MonoBehaviour
{
   [Header("References")]
    public DeerAI Deer;
    public DeerEye Eye;

    private DeerBlackboard Blackboard => Deer.FSM.DeerBlackboard;

    [Header("Sight Settings")]
    public LayerMask VisionMask;
    public float EyeHeight = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Blackboard.Player = other.transform;
        Blackboard.TimePlayerVisible = 0f;
    }

    private void OnTriggerStay(Collider other)//split up into helper methods, only add suspicion
    {
        if (!other.CompareTag("Player") || Eye == null || Eye.Profile == null)
            return;

        Blackboard.Player = other.transform;

        Vector3 origin = transform.position + Vector3.up * EyeHeight;
        Vector3 targetPos = other.transform.position + Vector3.up * EyeHeight;
        Vector3 toTarget = targetPos - origin;
        float distance = toTarget.magnitude;
        
        if (distance < Eye.Profile.MinRange || distance > Eye.Profile.MaxRange)
        {
            Blackboard.HasLineOfSight = false;
            Blackboard.TimePlayerVisible = 0f;
            return;
        }
        
        Vector3 dir = toTarget.normalized;
        float dot = Vector3.Dot(transform.forward, dir);
        float cosHalfFOV = Mathf.Cos(Eye.Profile.FOV * 0.5f * Mathf.Deg2Rad);
        if (dot < cosHalfFOV)
        {
            Blackboard.HasLineOfSight = false;
            Blackboard.TimePlayerVisible = 0f;
            return;
        }

        if (!Physics.Raycast(origin, dir, out RaycastHit hit, distance, VisionMask))
        {
            Blackboard.HasLineOfSight = true;
            Blackboard.TimePlayerVisible += Time.deltaTime;
            
            Deer.Senses.OnPlayerSightedContinuous(Blackboard.Player, Eye);
        }
        else
        {
            Blackboard.HasLineOfSight = false;
            Blackboard.TimePlayerVisible = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Blackboard.Player = null;
        Blackboard.TimePlayerVisible = 0f;
        Blackboard.HasLineOfSight = false;
    }
}
