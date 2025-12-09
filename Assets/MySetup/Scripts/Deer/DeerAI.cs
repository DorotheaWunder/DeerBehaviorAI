using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeerAI : MonoBehaviour
{
    [Header("AI Navigation")] 
    public GameObject Player;
    public NavMeshAgent Agent;
    
    [Header("Manager References")]
    public DeerFSM FSM;
    public DeerSenseSuspicionManager Senses;
    public DeerNeedController Needs;
    public HerdManager Herd;
    
    public float TotalSuspicion => Senses.GetTotalSuspicion();

    private void Awake()
    {
        if (!Agent) Agent = GetComponent<NavMeshAgent>();
        
        if (!FSM) FSM = GetComponent<DeerFSM>();
        if (!Senses) Senses = GetComponent<DeerSenseSuspicionManager>();
        if (!Needs) Needs = GetComponent<DeerNeedController>();
        if (!Herd) Herd = GetComponentInParent<HerdManager>();
    }
    
    private void OnEnable()
    {
        Needs.OnNeedEvent += HandleNeedEvent;
    }

    private void OnDisable()
    {
        Needs.OnNeedEvent -= HandleNeedEvent;
    }

    private void HandleNeedEvent(NeedEvent needEvent)
    {
        FSM?.OnNeedEvent(needEvent);
    }
}
