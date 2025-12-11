using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeerUpdateProfile", menuName = "DeerUpdateProfile")]
public class SO_DeerUpdateProfile : ScriptableObject
{
    public float UpdateMultiplierTotal = 1f;
    
    public float FSMUpdateInterval = 0.2f;
    public float NeedsUpdateInterval = 0.5f;
    public float SightUpdateInterval = 0.25f;
}
