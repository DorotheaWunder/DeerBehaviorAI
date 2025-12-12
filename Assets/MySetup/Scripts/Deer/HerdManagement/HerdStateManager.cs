using System;
using System.Collections.Generic;
using UnityEngine;

public class HerdStateManager : MonoBehaviour
{
    public HerdState CurrentHerdState = HerdState.Normal;

    public event Action<HerdState> OnHerdStateChanged;
    
    public void SetHerdState(HerdState newState)
    {
        if (newState == CurrentHerdState)
            return;

        CurrentHerdState = newState;
        OnHerdStateChanged?.Invoke(newState);
    }
    
    public void TriggerFlee(Vector3 threatPosition)
    {
        SetHerdState(HerdState.Fleeing);
    }
    
    public void TriggerMigrateMeadow(Vector3 meadowPosition)
    {
        SetHerdState(HerdState.MigrateMeadow);
    }
    
    public void TriggerMigrateStream(Vector3 streamPosition)
    {
        SetHerdState(HerdState.MigrateStream);
    }
    
    public void ResetToNormal()
    {
        SetHerdState(HerdState.Normal);
    }
}
