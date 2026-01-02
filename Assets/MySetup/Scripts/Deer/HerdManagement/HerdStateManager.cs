using System;
using System.Collections.Generic;
using UnityEngine;

public class HerdStateManager : MonoBehaviour
{
    public HerdState CurrentHerdState = HerdState.Normal;

    public event Action<HerdState> OnHerdStateChanged;
    
    private void OnEnable()
    {
        if (SuspicionManager.Instance != null)
        {
            SuspicionManager.Instance.OnSuspicionFull.AddListener(OnSuspicionFull);
            SuspicionManager.Instance.OnSuspicionEmpty.AddListener(OnSuspicionEmpty);
        }
    }

    private void OnDisable()
    {
        if (SuspicionManager.Instance != null)
        {
            SuspicionManager.Instance.OnSuspicionFull.RemoveListener(OnSuspicionFull);
            SuspicionManager.Instance.OnSuspicionEmpty.RemoveListener(OnSuspicionEmpty);
        }
    }

    private void OnSuspicionFull()
    {
        if (CurrentHerdState != HerdState.Fleeing)
            SetHerdState(HerdState.Fleeing);
    }
    
    private void OnSuspicionEmpty()
    {
        if (CurrentHerdState == HerdState.Fleeing)
            ResetToNormal();
    }
    
    public void SetHerdState(HerdState newState)
    {
        if (newState == CurrentHerdState)
            return;

        CurrentHerdState = newState;
        OnHerdStateChanged?.Invoke(newState);
    }
    
    public void TriggerMigrateMeadow(Vector3 meadowPosition)
    {
        SetHerdState(HerdState.MigrateMeadow);
    }
    
    public void TriggerMigrateStream(Vector3 streamPosition)
    {
        SetHerdState(HerdState.MigrateStream);
    }
    
    public void TriggerMigrateShelter(Vector3 meadowPosition)
    {
        SetHerdState(HerdState.MigrateShelter);
    }
    
    public void ResetToNormal()
    {
        SetHerdState(HerdState.Normal);
    }
}
