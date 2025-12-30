using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeerAI : MonoBehaviour, ITickable
{
    [Header("AI Navigation")] 
    public GameObject Player;
    public NavMeshAgent Agent;
    
    [Header("Manager References")]
    public DeerFSM FSM;
    public DeerSenseManager Senses;
    public DeerNeedController Needs;
    public HerdManager Herd;
    
    public float TotalSuspicion => Senses.GetTotalSuspicion();

    private void Awake()
    {
        if (!Agent) Agent = GetComponent<NavMeshAgent>();
        if (!FSM) FSM = GetComponent<DeerFSM>();
        if (!Senses) Senses = GetComponent<DeerSenseManager>();
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

    public void Tick(float dt, float distanceMultiplier = 1f)
    {
        FSM?.Tick(dt);
        Needs?.Tick(dt);
        //Senses?.Tick(dt);
        //and any other updates as well
    }
    
    //------------------------------------------------------- Boid Movement
    public Vector3 ApplyHerdDirection(Vector3 baseDirection)
    {
        if (Herd == null || Herd.CohesionManager == null)
            return baseDirection;

        Vector3 boidDir = Herd.CohesionManager.GetBoidForce(this);
        return (baseDirection + boidDir).normalized;
    }
}
