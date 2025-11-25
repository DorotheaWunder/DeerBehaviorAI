using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAI : MonoBehaviour
{
    public float HearingSuspicion = 5f;
    public float CooldownAfterHearing = 1f;
    
    public void OnSoundHeard(Vector3 sourcePosition)
    {
        Debug.Log("Deer heard sound at: " + sourcePosition);
        SuspicionManager.Instance.AddSuspicion(amount: HearingSuspicion, miniCooldown: CooldownAfterHearing);
    }
}
