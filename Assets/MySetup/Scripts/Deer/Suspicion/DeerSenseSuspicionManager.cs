using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerSenseSuspicionManager : MonoBehaviour
{
    [Header("Hearing")]
    public float HearingSuspicion = 5f;
    public float CooldownAfterHearing = 1f;
    
    [Header("Sight")]
    public float SightSuspicion = 1f;
    public float CooldownAfterSighted = 1f;

    public void OnSoundHeard(Vector3 sourcePosition)
    {
        Debug.Log("Deer heard sound at: " + sourcePosition);
        SuspicionManager.Instance.AddSuspicion(HearingSuspicion, CooldownAfterHearing);
    }

    public void OnPlayerSighted()
    {
        Debug.Log("Player initially sighted");
        SuspicionManager.Instance.AddSuspicion(SightSuspicion);
    }

    public void OnPlayerSightedContinuous()
    {
        SuspicionManager.Instance.AddSuspicion(SightSuspicion * Time.deltaTime);
    }
    
    public float GetTotalSuspicion()
    {
        if (SuspicionManager.Instance != null)
            return SuspicionManager.Instance.GetSuspicionNormalized();
        return 0f;
    }
}
