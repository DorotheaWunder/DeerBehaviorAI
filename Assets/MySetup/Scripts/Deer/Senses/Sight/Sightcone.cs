using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sightcone
{
    private readonly Transform _fovOwner;
    private readonly SO_SightconeProfile _sightProfile;

    public Sightcone(Transform fovOwner ,SO_SightconeProfile sightProfile)
    {
        _fovOwner = fovOwner;
        _sightProfile = sightProfile;
    }

    public bool PlayerIsInFOV(Transform target)
    {
        Vector3 toTarget = target.position - _fovOwner.position;
        if (toTarget.magnitude > _sightProfile.MaxRange) return false;

        float angle = Vector3.Angle(_fovOwner.forward, toTarget);
        return angle <= _sightProfile.FOV * 0.5f;
    }
}