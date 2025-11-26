using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAI : MonoBehaviour
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
        SuspicionManager.Instance.AddSuspicion(amount: HearingSuspicion, miniCooldown: CooldownAfterHearing);
    }

    public void OnPlayerSighted()
    {
        Debug.Log("Player initially sighted");
    }
    
    public void OnPlayerSightedContinuous()
    {
        SuspicionManager.Instance.AddSuspicion(
            amount: SightSuspicion * Time.deltaTime,
            miniCooldown: CooldownAfterSighted
        );
    }
}
