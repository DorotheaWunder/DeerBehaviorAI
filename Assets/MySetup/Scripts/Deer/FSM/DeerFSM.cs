using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerFSM : MonoBehaviour, ITickable, IFreezable
{
    [Header("References")]
    public DeerAI DeerAI;

    [Header("State")]
    public SO_DeerState CurrentState;
    [SerializeField] private float _stateTimer;
    [SerializeField] private float _timeInState;
    [SerializeField] private float _currentStateDuration;
    [SerializeField] private float _timeUntilNextTransition;
    
    [Header("BlackBoard")]
    public DeerBlackboard DeerBlackboard;
    
    [Header("Override (Herd / Emergency)")]
    [SerializeField] private bool _isOverridden;
    [SerializeField] private SO_DeerState _returnState;

    public event Action<SO_DeerState, SO_DeerState> OnStateChanged;

    [Header("Herd States")]
    [SerializeField] private SO_DeerState FleeState;
    // [SerializeField] private SO_DeerState MigrateMeadowState;
    // [SerializeField] private SO_DeerState MigrateStreamState;
    // [SerializeField] private SO_DeerState MigrateShelterState;

    private void OnEnable()
    {
        if (!DeerAI) DeerAI = GetComponent<DeerAI>();
        if (DeerAI?.Herd?.StateManager != null)
            DeerAI.Herd.StateManager.OnHerdStateChanged += OnHerdStateChanged;
    }

    private void Start()
    {
        if (CurrentState == null)
            return;

        InitializeState(CurrentState, null);
    }

    private void OnDisable()
    {
        if (DeerAI?.Herd?.StateManager != null)
            DeerAI.Herd.StateManager.OnHerdStateChanged -= OnHerdStateChanged;
    }

    public void Tick(float deltaTime, float distanceMultiplier = 1f)
    {
        if (CurrentState == null)
            return;

        _stateTimer += deltaTime * distanceMultiplier;
        _timeInState += deltaTime * distanceMultiplier;

        if (_stateTimer >= CurrentState.UpdateInterval)
        {
            _stateTimer = 0f;
            CurrentState.ExecuteActions(this);
            CurrentState.UpdateState(this);
        }

        if (_isOverridden)
            return;

        if (_timeInState < _currentStateDuration)
            return;

        if (_timeUntilNextTransition > 0f)
        {
            _timeUntilNextTransition -= deltaTime * distanceMultiplier;
            return;
        }

        SO_DeerState nextState = CurrentState.CheckTransitions(this);
        if (nextState != null)
            TransitionToState(nextState);
    }

    public void TransitionToState(SO_DeerState newState)
    {
        if (newState == null || newState == CurrentState)
            return;

        SO_DeerState previousState = CurrentState;

        CurrentState?.ExitState(this);

        InitializeState(newState, previousState);
    }

    private void InitializeState(SO_DeerState state, SO_DeerState previousState)
    {
        CurrentState = state;
        _stateTimer = 0f;
        _timeInState = 0f;

        _currentStateDuration = UnityEngine.Random.Range(state.MinDuration, state.MaxDuration);
        _timeUntilNextTransition = UnityEngine.Random.Range(state.MinTransitionTime, state.MaxTransitionTime);

        state.EnterState(this);

        OnStateChanged?.Invoke(CurrentState, previousState);
    }

    //------------------------------------------ Overwriting
    public void OverwriteState(SO_DeerState state)
    {
        if (_isOverridden || state == null || CurrentState == state)
            return;

        _returnState = CurrentState;
        _isOverridden = true;

        TransitionToState(state);
    }

    public void ClearOverride()
    {
        if (!_isOverridden)
            return;

        _isOverridden = false;

        if (_returnState != null)
        {
            TransitionToState(_returnState);
            _returnState = null;
        }
    }

    //------------------------------------------ Herd States
    private void OnHerdStateChanged(HerdState state)
    {
        switch (state)
        {
            case HerdState.Fleeing:
                OverwriteState(FleeState);
                break;
            // case HerdState.MigrateMeadow:
            //     OverwriteState(MigrateMeadowState);
            //     break;
            //
            // case HerdState.MigrateStream:
            //     OverwriteState(MigrateStreamState);
            //     break;
            //
            // case HerdState.MigrateShelter:
            //     OverwriteState(MigrateShelterState);
            //     break;
            
            case HerdState.Normal:
                ClearOverride();
                break;
        }
    }

    //------------------------------------------ Need Events
    public void OnNeedEvent(NeedEvent needEvent)
    {
        CurrentState?.OnNeedEvent(this, needEvent);
    }

    //------------------------------------------ Freezing
    public void OnFreeze() => enabled = false;
    public void OnThaw() => enabled = true;
}
