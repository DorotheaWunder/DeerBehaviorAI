using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdCohesionManager : MonoBehaviour
{
    [Header("Herd Center")]
    public Transform HerdCenter;
    public float MinToCenter = 5f;
    public float MaxToCenter = 50f;

    [Header("Individual Cohesion")]
    public float MinToOtherDeer = 3f;
    public float MaxToOtherDeer = 10f;

    private HerdManager _herdManager;

    public void InitializeCohesion(HerdManager herdManager)
    {
        _herdManager = herdManager;
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
}
