using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAI : MonoBehaviour
{
    [Header("References")]
    public GameObject Player;
    public DeerFSM FSM;
    public DeerSenseSuspicionManager Senses;
    public DeerNeedController Needs;
    public HerdManager Herd;
    
    public float TotalSuspicion => Senses.GetTotalSuspicion();

    private void Awake()
    {
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
