using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HerdCohesionManager : MonoBehaviour
{
    public HerdManager _herdManager;
    
    [Header("Boid Movement")]
    [Range(0f,1f)]
    public float BoidFactor = 0.5f;
    
    [Header("Herd Center")]
    public Transform HerdCenter;
    
    [Header("Base Herd Center")]
    public float BaseMinToCenter = 5f;
    public float BaseMaxToCenter = 50f;

    [Header("Base Individual Cohesion")]
    public float BaseMinToOtherDeer = 3f;
    public float BaseMaxToOtherDeer = 10f;

    [Header("Runtime Modifiers")]
    [Range(0.1f, 2f)] public float CenterRangeMultiplier = 1f;
    [Range(0.1f, 2f)] public float IndividualRangeMultiplier = 1f;
    
    public float MinToCenter => BaseMinToCenter * CenterRangeMultiplier;
    public float MaxToCenter => BaseMaxToCenter * CenterRangeMultiplier;

    public float MinToOtherDeer => BaseMinToOtherDeer * IndividualRangeMultiplier;
    public float MaxToOtherDeer => BaseMaxToOtherDeer * IndividualRangeMultiplier;
    
    void Start()
    {
        _herdManager = GetComponent<HerdManager>();
    }
    
    public float DistanceToCenter(Transform deer) =>
        Vector3.Distance(deer.position, HerdCenter.position);

    public float DistanceToNearestDeer(Transform deer)
    {
        float minDistance = float.MaxValue;
        foreach (var other in _herdManager.DeerList)
        {
            if (other.transform == deer) continue;
            float dist = Vector3.Distance(deer.position, other.transform.position);
            if (dist < minDistance) minDistance = dist;
        }
        return minDistance;
    }

    public bool IsOutsideMaxCenterRange(Transform deer) => DistanceToCenter(deer) > MaxToCenter;
    public bool IsTooFarFromNearestDeer(Transform deer) => DistanceToNearestDeer(deer) > MaxToOtherDeer; 
    
    public bool IsTooClose(DeerAI deer)
    {
        float distance = DistanceToNearestDeer(deer.transform);
        return distance < MinToOtherDeer;
    }
    
    public float PersonalSpacePressure(DeerAI deer)
    {
        float distance = DistanceToNearestDeer(deer.transform);
        if (distance >= MaxToOtherDeer) return 0f;
        if (distance <= MinToOtherDeer) return 1f;
        return 1f - (distance - MinToOtherDeer) / (MaxToOtherDeer - MinToOtherDeer);
    }
    
    
    public bool IsApproaching(Transform self, NavMeshAgent otherAgent)
    {
        Vector3 toSelf = self.position - otherAgent.transform.position;
        return Vector3.Dot(otherAgent.velocity.normalized, toSelf.normalized) > 0.5f;
    }
    
    //------------------------------------------ Boid Section
    public Vector3 GetBoidForce(DeerAI deer)
    {
        Vector3 cohesionForce = Vector3.zero;
        Vector3 separationForce = Vector3.zero;
        Vector3 alignmentForce = Vector3.zero;
    
        int neighborCount = 0;

        foreach (var other in _herdManager.DeerList)
        {
            if (other == deer) continue;
            float distance = Vector3.Distance(deer.transform.position, other.transform.position);

            if (distance < MaxToOtherDeer)
            {
                cohesionForce += other.transform.position;
                
                if (distance < MinToOtherDeer)
                    separationForce += (deer.transform.position - other.transform.position) / distance;
                
                alignmentForce += Vector3.zero; 
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            cohesionForce = (cohesionForce / neighborCount - deer.transform.position).normalized;
            alignmentForce = (alignmentForce / neighborCount).normalized;
            separationForce = separationForce.normalized;
        }
        
        Vector3 centerForce = (HerdCenter.position - deer.transform.position).normalized;
        
        Vector3 boidForce = (
            cohesionForce * 0.5f +
            separationForce * 1f +
            alignmentForce * 0.3f +
            centerForce * 0.2f
        ) * BoidFactor;

        return boidForce;
    }
    
    public Vector3 GetHerdFleeDirection(Vector3 threatPosition)
    {
        Vector3 dir = HerdCenter.position - threatPosition;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return Vector3.zero;

        return dir.normalized;
    }
    
    public Vector3 GetHerdTarget(DeerAI deer)
    {
        if (_herdManager.HerdBB == null || !_herdManager.HerdBB.HasGoal)
            return deer.transform.position;

        Vector3 herdGoal = _herdManager.HerdBB.GoalPosition;
        Vector3 goalDir = (herdGoal - deer.transform.position).normalized;
        Vector3 boidForce = GetBoidForce(deer);
        Vector3 finalDir = (goalDir + boidForce).normalized;

        return deer.transform.position + finalDir * 5f;
    }
    
    //----------------------------------- Gizmos
    private void OnDrawGizmos()
    {
        if (HerdCenter != null)
        {

            Gizmos.color = new Color(0.6f, 0.3f, 0f, 1f);
            Gizmos.DrawWireSphere(HerdCenter.position, MaxToCenter);
        }

        List<Transform> deerTransforms = new List<Transform>();
        if (_herdManager != null && _herdManager.DeerList != null)
        {
            foreach (var deer in _herdManager.DeerList)
                deerTransforms.Add(deer.transform);
        }
        else
        {
            var allDeer = FindObjectsOfType<DeerAI>();
            foreach (var deer in allDeer)
                deerTransforms.Add(deer.transform);
        }

        for (int i = 0; i < deerTransforms.Count; i++)
        {
            Transform deerA = deerTransforms[i];

            Transform closest = null;
            float closestDistance = float.MaxValue;

            for (int j = 0; j < deerTransforms.Count; j++)
            {
                if (i == j) continue;
                float dist = Vector3.Distance(deerA.position, deerTransforms[j].position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closest = deerTransforms[j];
                }
            }
            
            for (int j = 0; j < deerTransforms.Count; j++)
            {
                if (i == j) continue;

                Transform deerB = deerTransforms[j];
                float distance = Vector3.Distance(deerA.position, deerB.position);
                
                Color lineColor;
                if (distance <= MaxToOtherDeer)
                {
                    float alpha = (deerB == closest) ? 1f : 0.1f;
                    lineColor = new Color(0f, 1f, 0f, alpha); 
                }
                else
                {
                    lineColor = new Color(1f, 0f, 0f, 0.05f); 
                }

                Gizmos.color = lineColor;
                Gizmos.DrawLine(deerA.position, deerB.position);
            }
        }
    }
}
