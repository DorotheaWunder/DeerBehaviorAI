using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdManager : MonoBehaviour
{
    [Header("References")]
    public HerdStateManager StateManager;
    public HerdCohesionManager CohesionManager;
    public HerdNeedManager NeedManager;
    public HerdBlackboard HerdBB = new HerdBlackboard();
    
    [Header("Herd Movement")]
    public Transform CurrentPOI;
    public Vector3 AnchorPosition;
    public Vector3 GoalPosition;
    public bool HasDestination;
    public bool HasArrived;
    
    [Header("Deer List")]
    public List<DeerAI> DeerList = new List<DeerAI>();

    void Start()
    {
        CohesionManager = GetComponent<HerdCohesionManager>();
        StateManager = GetComponent<HerdStateManager>();
        NeedManager = GetComponent<HerdNeedManager>();
        
        DeerList.Clear();
        GetComponentsInChildren<DeerAI>(includeInactive: false, result: DeerList);
    }
}
