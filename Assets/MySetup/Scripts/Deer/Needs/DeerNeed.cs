using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeerNeed 
{
    public SO_NeedProfile NeedProfile;
    
    [Range(0f, 1f), SerializeField] 
    private float _needValue = 1f; 
    
    public float Value => _needValue;
    public NeedType NeedType => NeedProfile.NeedType;

    public bool BelowMarker => _needValue < NeedProfile.LowThreshold;
    public bool AboveMarker => _needValue > NeedProfile.HighThreshold;
    
    public void Drain(float deltaTime)
    {
        _needValue -= NeedProfile.DrainPerSecond * deltaTime;
        _needValue = Mathf.Clamp01(_needValue);
    }
    
    public void Recover(float deltaTime)
    {
        _needValue += NeedProfile.RecoveryPerSecond * deltaTime;
        _needValue = Mathf.Clamp01(_needValue);
    }
}
