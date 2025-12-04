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

    private float _drainMultiplier = 1f;
    private float _recoverMultiplier = 1f;
    
    public NeedType NeedType => NeedProfile.NeedType;
    public bool BelowMarker => _needValue < NeedProfile.LowThreshold;
    public bool AboveMarker => _needValue > NeedProfile.HighThreshold;
    
    
    public void InitializeRandomizers(float drainMin, float drainMax, float recoverMin, float recoverMax)
    {
        _drainMultiplier = Random.Range(drainMin, drainMax);
        _recoverMultiplier = Random.Range(recoverMin, recoverMax);
    }
    
    public void Drain(float deltaTime)
    {
        _needValue -= NeedProfile.DrainPerSecond * _drainMultiplier * deltaTime;
        _needValue = Mathf.Clamp01(_needValue);
    }
    
    public void Recover(float deltaTime)
    {
        _needValue += NeedProfile.RecoveryPerSecond * _recoverMultiplier * deltaTime;
        _needValue = Mathf.Clamp01(_needValue);
    }
}
