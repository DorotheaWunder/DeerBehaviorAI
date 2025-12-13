using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerFSM : MonoBehaviour, ITickable, IFreezable
{
    public DeerAI DeerAI;
    public SO_DeerState CurrentState;
    [SerializeField] private float _stateTimer = 0f;
    
    [Header("Overwrite State")]
    [SerializeField] private bool _isOverridden;
    [SerializeField] private SO_DeerState _overwriteState;
    [SerializeField] private SO_DeerState _initialState;
    
    public event Action<SO_DeerState, SO_DeerState> OnStateChanged; 
    
    private void Start()
    {
        if (CurrentState != null)
        {
            CurrentState.EnterState(this);
            OnStateChanged?.Invoke(CurrentState, null);
        }
    }
    
    private void OnEnable()
    {
        if (!DeerAI) DeerAI = GetComponent<DeerAI>();

        if (DeerAI.Herd != null && DeerAI.Herd.StateManager != null)
            DeerAI.Herd.StateManager.OnHerdStateChanged += OnHerdStateChanged;
    }
    
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
    
    //------------------------------------ need events
    public void OnNeedEvent(NeedEvent needEvent)
    {
        CurrentState?.OnNeedEvent(this, needEvent);
        //trigger need state
    }
    
    //------------------------------------ Herdwide States
    public void OverwriteState(SO_DeerState state)
    {
        if (_isOverridden) return;

        _initialState = CurrentState;
        _overwriteState = state;
        _isOverridden = true;

        TransitionToState(state);
    }

    public void ClearOverride()
    {
        if (!_isOverridden) return;

        _isOverridden = false;
        TransitionToState(_initialState);
    }
    
    [SerializeField] private SO_DeerState FleeState;
    // [SerializeField] private SO_DeerState MigrateMeadowState;
    // [SerializeField] private SO_DeerState MigrateStreamState;
    
    private void OnHerdStateChanged(HerdState state)
    {
        switch (state)
        {
            case HerdState.Fleeing:
                Debug.Log($"{name} OVERWRITING STATE → FleeState");
                OverwriteState(FleeState);
                break;

            // case HerdState.MigrateMeadow:
            //     OverwriteState(MigrateMeadowState);
            //     break;
            //
            // case HerdState.MigrateStream:
            //     OverwriteState(MigrateStreamState);
            //     break;

            case HerdState.Normal:
                Debug.Log($"{name} CLEAR OVERRIDE → { _initialState }");
                ClearOverride();
                break;
        }
    }
    
    private void OnDisable()
    {
        if (DeerAI && DeerAI.Herd && DeerAI.Herd.StateManager != null)
            DeerAI.Herd.StateManager.OnHerdStateChanged -= OnHerdStateChanged;
    }
    
    
    //-------------------------------------- connection to DeerFreezer
    public void OnFreeze()
    {
        enabled = false;
    }

    public void OnThaw()
    {
        enabled = true;
    }
}
