using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/States/State")]
public class State : SO_DeerState
{
    public string StateName;
    public SO_AnimationSet AnimationSet;
    //maybe have space for need draining multiplier (needs drain faster in certain states)
    
    [Header("Changes Need?")]
    public NeedbasedState NeedbasedState = NeedbasedState.None; 
    public override NeedbasedState HerdNeed => NeedbasedState;

    
    public override void EnterState(DeerFSM deerFSM)
    {
        Debug.Log("Entering state " + StateName);
    }

    public override void UpdateState(DeerFSM deerFSM)
    {
        Debug.LogWarning("Deer is: " + StateName);
    }

    public override void ExitState(DeerFSM deerFSM)
    {
        Debug.Log("Exiting State: " + StateName);
    }
}
