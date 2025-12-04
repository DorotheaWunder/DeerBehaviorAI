using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Conditions/AtPlace")]
public class AtPlace : SO_StateCondition
{
    public string PlaceTag;
    
    public override bool EvaluateCondition(DeerFSM deerFSM)
    {
        Debug.LogWarning("Deer is at " + PlaceTag);
        return false; //for now
    }
}
