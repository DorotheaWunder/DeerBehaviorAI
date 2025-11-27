using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //use unity events instead?

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
        float distance = toTarget.magnitude;

        if (distance > Profile.MaxRange)
        {
            if (playerInside)
            {
                playerInside = false;
                OnFOVExit?.Invoke();
            }
            return;
        }

        float angle = Vector3.Angle(transform.forward, toTarget);
        if (angle > Profile.FOV * 0.5f)
        {
            if (playerInside)
            {
                playerInside = false;
                OnFOVExit?.Invoke();
            }
            return;
        }
        
        if (Physics.Raycast(transform.position, 
                toTarget.normalized, 
                out RaycastHit hit, 
                Profile.MaxRange, 
                Profile.VisionMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!playerInside)
                {
                    playerInside = true;
                    OnFOVEnter?.Invoke();
                }
            }
            else
            {
                if (playerInside)
                {
                    playerInside = false;
                    OnFOVExit?.Invoke();
                }
            }
        }
        else
        {
            if (playerInside)
            {
                playerInside = false;
                OnFOVExit?.Invoke();
            }
        }
    }

    public bool IsPlayerInside => playerInside;
}
