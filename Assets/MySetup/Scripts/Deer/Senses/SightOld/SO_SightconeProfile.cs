using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SightconeProfile", menuName = "DeerSystem/Sight/NewSightconeProfile")]
public class SO_SightconeProfile : ScriptableObject
{
    [Header("Geometry")]
    public float MinRange = 2f;
    public float MaxRange = 30f;
    public float FOV = 200f;
    [HideInInspector] public float CosHalfFOV;
    
    [Header("Falloff")]
    public AnimationCurve DistanceFalloff = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public AnimationCurve ProximitySuspicionCurve = AnimationCurve.Linear(0, 1, 1, 0); 
    
    [Header("Occlusion")]
    public LayerMask VisionMask;
    public float BlockPenalty = 0.25f;//maybe have that in its own script?

    [Header("Sampling Settings")]
    public int RaysPerEye = 1;
    public float EyeSphereRadius = 0.05f;
    
    private void OnEnable()
    {
        CosHalfFOV = Mathf.Cos(FOV * 0.5f * Mathf.Deg2Rad);
    }
}

// [Header("Geometry")]
// public float MinRange = 2f;
// public float MaxRange = 30f;
// public float FOV = 120f;
// [HideInInspector] public float CosHalfFOV;
//     
// [Header("Falloff")]
// public AnimationCurve DistanceFalloff = 
// AnimationCurve.EaseInOut(0, 1, 1, 0);
//
// [Header("Occlusion")]
// public LayerMask VisionMask;
// public float BlockPenalty = 0.25f;//maybe have that in its own script?
//
// [Header("Sampling Settings")]
// public int RaysPerEye = 1;
// public float EyeSphereRadius = 0.05f;
//     
// private void OnEnable()
// {
//     CosHalfFOV = Mathf.Cos(FOV * 0.5f * Mathf.Deg2Rad);
// }