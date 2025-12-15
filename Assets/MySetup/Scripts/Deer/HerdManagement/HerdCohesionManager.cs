using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdCohesionManager : MonoBehaviour
{
    public HerdManager _herdManager;
    
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
