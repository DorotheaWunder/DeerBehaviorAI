using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeerEye : MonoBehaviour
{
    public SO_SightconeProfile Profile;
    public Transform Target;

    public bool IsTargetValid { get; private set; }

    public bool EvaluateTarget(out float distance01)
    {
        distance01 = 0f;

        if (Target == null || Profile == null)
        {
            IsTargetValid = false;
            return false;
        }

        Vector3 toTarget = Target.position - transform.position;
        float distance = toTarget.magnitude;
        
        if (distance < Profile.MinRange || distance > Profile.MaxRange)
        {
            IsTargetValid = false;
            return false;
        }

        Vector3 dir = toTarget / distance;
        float dot = Vector3.Dot(transform.forward, dir);
        if (dot < Profile.CosHalfFOV)
        {
            IsTargetValid = false;
            return false;
        }

        distance01 = Mathf.InverseLerp(Profile.MinRange, Profile.MaxRange, distance);
        IsTargetValid = true;
        return true;
    }

    public bool HasLineOfSight()
    {
        if (Target == null || Profile == null)
            return false;

        Vector3 origin = transform.position;
        Vector3 dir = (Target.position - origin).normalized;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, Profile.MaxRange, Profile.VisionMask))
        {
            return hit.transform == Target;
        }

        return false;
    }

    public void ClearTarget()
    {
        Target = null;
        IsTargetValid = false;
    }
}
