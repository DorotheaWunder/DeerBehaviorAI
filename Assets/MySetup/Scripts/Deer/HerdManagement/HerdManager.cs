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
    
    [Header("Herd Movement")]
    public Transform CurrentPOI;
    
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
