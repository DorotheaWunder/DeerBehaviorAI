using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeedProfile", menuName = "DeerNeeds/NewProfile")]
public class SO_NeedProfile : ScriptableObject
{
    public NeedType NeedType;
        
    public float DrainPerSecond = 0.1f;
    public float RecoveryPerSecond = 0.2f; 
    
    [Range(0f, 1f)] public float LowThreshold = 0.3f;
    [Range(0f, 1f)] public float HighThreshold = 0.9f;
}
