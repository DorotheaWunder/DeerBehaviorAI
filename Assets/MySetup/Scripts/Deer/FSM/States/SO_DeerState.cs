using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_DeerState : ScriptableObject
{
    public List<SO_StateAction> Actions = new List<SO_StateAction>();
    public List<StateTransition> Transitions = new List<StateTransition>();
    public virtual NeedbasedState HerdNeed => NeedbasedState.None;
    
    public virtual void OnNeedEvent(DeerFSM deerFSM, NeedEvent needEvent) {}
    
    public abstract void EnterState(DeerFSM deerFSM);
    public abstract void UpdateState(DeerFSM deerFSM);
    public abstract void ExitState(DeerFSM deerFSM);
    
    
    public void ExecuteActions(DeerFSM deerFSM)
    {
        foreach (var action in Actions)
        {
            action.ExecuteAction(deerFSM);
        }
    }
    
    public SO_DeerState CheckTransitions(DeerFSM deerFSM)
    {
        foreach (var transition in Transitions)
        {
            if (transition.ShouldTransition(deerFSM))
                return transition.TargetState;
        }
        return null;
    }
}
