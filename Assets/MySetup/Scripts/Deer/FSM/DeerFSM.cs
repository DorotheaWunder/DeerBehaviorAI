using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerFSM : MonoBehaviour, ITickable
{
    public DeerAI DeerAI;
    public SO_DeerState CurrentState;
    [SerializeField] private float _stateTimer = 0f;
    public event Action<SO_DeerState, SO_DeerState> OnStateChanged; //unity event instead?
    
    private void Start()
    {
        if (!DeerAI) DeerAI = GetComponent<DeerAI>();
        if (CurrentState != null)
        {
            CurrentState.EnterState(this);
            OnStateChanged?.Invoke(CurrentState, null);
        }
    }

    // private void Update()
    // {
    //     Tick(Time.deltaTime);
    //     // if (CurrentState == null) return;
    //     //
    //     // CurrentState.ExecuteActions(this);
    //     //
    //     // CurrentState.UpdateState(this);
    //     //
    //     // SO_DeerState nextState = CurrentState.CheckTransitions(this);
    //     // if (nextState != null)
    //     //     TransitionToState(nextState);
    // }

    
    public void Tick(float deltaTime, float distanceMultiplier = 1f)
    {
        if (CurrentState == null) return;

        _stateTimer += deltaTime * distanceMultiplier;

        if (_stateTimer >= CurrentState.UpdateInterval)
        {
            _stateTimer = 0f;

            CurrentState.ExecuteActions(this);
            CurrentState.UpdateState(this);

            var nextState = CurrentState.CheckTransitions(this);
            if (nextState != null)
                TransitionToState(nextState);
        }
    }
    
    public void TransitionToState(SO_DeerState newState)
    {
        if (CurrentState != null)
            CurrentState.ExitState(this);

        var previousState = CurrentState;
        CurrentState = newState;

        if (CurrentState != null)
        {
            CurrentState.EnterState(this);
            OnStateChanged?.Invoke(CurrentState, previousState);
        }
    }
    
    public void OnNeedEvent(NeedEvent needEvent)
    {
        CurrentState?.OnNeedEvent(this, needEvent);
        //trigger need state
    }
}
