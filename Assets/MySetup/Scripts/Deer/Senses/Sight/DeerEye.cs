using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeerEye : MonoBehaviour
{
    public SO_SightconeProfile Profile;
    public Transform Target;

    public event Action OnFOVEnter;
    public event Action OnFOVExit;

    private bool playerInside = false;

    public void CheckFOV()
    {
        if (Target == null || Profile == null)
            return;

        Vector3 toTarget = Target.position - transform.position;
        float sqrDist = toTarget.sqrMagnitude;
        
        if (sqrDist > Profile.MaxRange * Profile.MaxRange)
        {
            ExitIfNeeded();
            return;
        }
        
        Vector3 dir = toTarget.normalized;
        float dot = Vector3.Dot(transform.forward, dir);

        if (dot < Profile.CosHalfFOV)
        {
            ExitIfNeeded();
            return;
        }
        
        if (Physics.Raycast(
                transform.position,
                dir,
                out RaycastHit hit,
                Profile.MaxRange,
                Profile.VisionMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                EnterIfNeeded();
            }
            else
            {
                ExitIfNeeded();
            }
        }
        else
        {
            ExitIfNeeded();
        }
    }
    
    private void EnterIfNeeded()
    {
        if (playerInside) return;
        playerInside = true;
        OnFOVEnter?.Invoke();
    }

    private void ExitIfNeeded()
    {
        if (!playerInside) return;
        playerInside = false;
        OnFOVExit?.Invoke();
    }
    
    public void ClearTarget()
    {
        if (playerInside)
        {
            playerInside = false;
            OnFOVExit?.Invoke();
        }

        Target = null;
    }

    public bool IsPlayerInside => playerInside;
}
