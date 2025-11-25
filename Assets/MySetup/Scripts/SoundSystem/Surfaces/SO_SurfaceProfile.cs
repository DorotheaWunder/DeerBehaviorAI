using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SurfaceProfile", menuName = "DeerSystem/Sound/NewSurfaceProfile")]
public class SO_SurfaceProfile : ScriptableObject
{
    public SurfaceType SurfaceType;

    [Header("Bubble Overrides")]
    public float RadiusMultiplier = 1f;
    public float DurationMultiplier = 1f;

    public AnimationCurve OverrideRadiusCurve;
    public bool UseOverrideCurve = false;
    
    //multipliers for suspicion/ suspicion decay
    //array of sfx
}
