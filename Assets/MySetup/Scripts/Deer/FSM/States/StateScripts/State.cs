using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/States/State")]
public class State : SO_DeerState
{
    public string StateName;
    public SO_AnimationSet AnimationSet;
    
    [Header("Changes Need?")]
    public NeedbasedState NeedbasedState = NeedbasedState.None; 
    public override NeedbasedState HerdNeed => NeedbasedState;

    
    public override void EnterState(DeerFSM deerFSM)
    {
        foreach (var action in Actions)
        {
            action.ResetExecution();
        }
        
        var bb = deerFSM.DeerBlackboard;
        
        bb.TargetType = MovementTargetType.None;
        bb.Mode = MovementMode.Stop;
        bb.FollowTarget = null;

        bb.HasGoal = false;
        bb.HasDestination = false;
        bb.TimeAtDestination = 0f;
    }

    public override void UpdateState(DeerFSM deerFSM)
    {
        //Debug.LogWarning("Deer is: " + StateName);
    }

    public override void ExitState(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;
        
        bb.TargetType = MovementTargetType.None;
        bb.Mode = MovementMode.Stop;
        bb.FollowTarget = null;

        bb.HasGoal = false;
        bb.HasDestination = false;
        bb.TimeAtDestination = 0f;
    }
}
