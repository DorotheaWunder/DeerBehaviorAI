using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdManager : MonoBehaviour
{
    [Header("References")]
    public HerdStateManager StateManager;
    public HerdCohesionManager CohesionManager;
    //need manager later
    
    [Header("Deer List")]
    public List<DeerAI> DeerList = new List<DeerAI>();
    [SerializeField] private int _herdSize;

    public int HungryDeer;
    public int ThirstyDeer;
    public int TiredDeer;
    
    
    void Start()
    {
        //need manager later
        CohesionManager = GetComponent<HerdCohesionManager>();
        StateManager = GetComponent<HerdStateManager>();
        
        DeerList.Clear();
        GetComponentsInChildren<DeerAI>(includeInactive: false, result: DeerList);
        _herdSize = DeerList.Count;
        
        foreach (var deer in DeerList)
        {
            deer.FSM.OnStateChanged += (newState, oldState) =>
                CheckDeerForNeedState(deer, oldState, newState);
        }
    }

    public void CheckDeerForNeedState(DeerAI deer, SO_DeerState oldState, SO_DeerState newState)
    {
        if (oldState != null && oldState.HerdNeed != NeedbasedState.None)
            AdjustDeerCount(oldState.HerdNeed, -1);
        
        if (newState != null && newState.HerdNeed != NeedbasedState.None)
            AdjustDeerCount(newState.HerdNeed, +1);

        CheckMajority();
    }

    private void AdjustDeerCount(NeedbasedState needState, int delta)
    {
        switch (needState)
        {
            case NeedbasedState.Hungry:
                HungryDeer = Mathf.Clamp(HungryDeer + delta, 0, _herdSize);
                break;
            case NeedbasedState.Thirsty:
                ThirstyDeer = Mathf.Clamp(ThirstyDeer + delta, 0, _herdSize);
                break;
            case NeedbasedState.Tired:
                TiredDeer = Mathf.Clamp(TiredDeer + delta, 0, _herdSize);
                break;
        }
    }

    private void CheckMajority()
    {
        if (HungryDeer > _herdSize / 2)
            Debug.Log("HERD: Majority is hungry.");

        if (ThirstyDeer > _herdSize / 2)
            Debug.Log("HERD: Majority is thirsty.");

        if (TiredDeer > _herdSize / 2)
            Debug.Log("HERD: Majority is tired.");
    }
}
