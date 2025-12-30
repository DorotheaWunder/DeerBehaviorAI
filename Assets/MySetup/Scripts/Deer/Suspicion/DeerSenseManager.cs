using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerSenseManager : MonoBehaviour
{
    [Header("Hearing")]
    public float HearingSuspicion = 5f;
    public float CooldownAfterHearing = 1f;

    [Header("Sight")]
    public float SightSuspicion = 1f;
    public float CooldownAfterSighted = 1f;

    public void OnSoundHeard(Vector3 sourcePosition, SO_SurfaceProfile surface)
    {
        float suspicionMultiplier = surface != null ? surface.SuspicionMultiplier : 1f;
        float decayMultiplier = surface != null ? surface.DecayMultiplier : 1f;
        float alertPause = surface != null ? surface.AlertPause : CooldownAfterHearing;

        SuspicionManager.Instance.AddSuspicion(
            HearingSuspicion * suspicionMultiplier,
            alertPause,
            decayMultiplier
        );
    }

    public void OnPlayerSighted()
    {
        SuspicionManager.Instance.AddSuspicion(SightSuspicion);
    }

    public void OnPlayerSightedContinuous(Transform player, DeerEye eye)
    {
        if (eye.Profile == null) return;

        Vector3 toPlayer = player.position - eye.transform.position;
        float distance = toPlayer.magnitude;
        
        float normalizedDistance = Mathf.InverseLerp(eye.Profile.MinRange, eye.Profile.MaxRange, distance);
        float distanceMultiplier = eye.Profile.ProximitySuspicionCurve.Evaluate(normalizedDistance);
        
        SuspicionManager.Instance.AddSuspicion(SightSuspicion * distanceMultiplier * Time.deltaTime);
    }
    
    public float GetTotalSuspicion()
    {
        if (SuspicionManager.Instance != null)
            return SuspicionManager.Instance.GetSuspicionNormalized();
        return 0f;
    }
}
