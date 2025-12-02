using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/States/State")]
public class State : SO_DeerState
{
    public string StateName;
    
    public override void EnterState(DeerFSM deerFSM)
    {
        Debug.Log("Entering state" + StateName);
    }

    public override void UpdateState(DeerFSM deerFSM)
    {
        Debug.Log("Deer is:" + StateName);
    }

    public override void ExitState(DeerFSM deerFSM)
    {
        Debug.Log("Exiting State: " + StateName);
    }
}
