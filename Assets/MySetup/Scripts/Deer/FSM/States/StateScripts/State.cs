using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/States/State")]
public class State : SO_DeerState
{
    [Header("Animation")]
    public string StateName;
    public SO_AnimationSet AnimationSet;
    public float MinAnimSpeed = 0.9f;
    public float MaxAnimSpeed = 1.1f;
    public float MinLoopPause = 0f;
    public float MaxLoopPause = 0.01f;
    public bool AllowLoopPause = true;
    
    [Header("Changes Need?")]
    public NeedbasedState NeedbasedState = NeedbasedState.None; 
    public override NeedbasedState HerdNeed => NeedbasedState;

    
    public override void EnterState(DeerFSM deerFSM)
    {
        //Debug.Log("Entering state " + StateName);
    }

    public override void UpdateState(DeerFSM deerFSM)
    {
        //Debug.LogWarning("Deer is: " + StateName);
    }

    public override void ExitState(DeerFSM deerFSM)
    {
        //Debug.Log("Exiting State: " + StateName);
    }
}
