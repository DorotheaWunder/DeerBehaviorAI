using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerFSM : MonoBehaviour
{
    public DeerAI DeerAI;
    public SO_DeerState CurrentState;

    private void Start()
    {
        if (!DeerAI) DeerAI = GetComponent<DeerAI>();
        if (CurrentState != null)
            CurrentState.EnterState(this);
    }

    private void Update()
    {
        if (CurrentState == null) return;

        CurrentState.ExecuteActions(this);

        CurrentState.UpdateState(this);

        SO_DeerState nextState = CurrentState.CheckTransitions(this);
        if (nextState != null)
            TransitionToState(nextState);
    }

    public void TransitionToState(SO_DeerState newState)
    {
        if (CurrentState != null)
            CurrentState.ExitState(this);

        CurrentState = newState;

        if (CurrentState != null)
            CurrentState.EnterState(this);
    }
}
