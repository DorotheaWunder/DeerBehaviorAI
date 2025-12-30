using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingDetector : MonoBehaviour, IFreezable
{
    [SerializeField] private DeerSenseManager deerSenseManager;

    private void OnTriggerEnter(Collider other)
    {
        SoundBubble bubble = other.GetComponent<SoundBubble>();
        if (bubble == null || !bubble.IsActive) return;
        
        SO_SurfaceProfile surfaceProfile = bubble.SurfaceProfile;

        Debug.Log($"[HearingDetector] Heard bubble on surface: {surfaceProfile?.name ?? "None"}");

        deerSenseManager.OnSoundHeard(bubble.transform.position, surfaceProfile);
    }
    
    //----------------------------------------- Connection to DeerFreezer
    public void OnFreeze()
    {
        enabled = false;
    }

    public void OnThaw()
    {
        enabled = true;
    }
}
