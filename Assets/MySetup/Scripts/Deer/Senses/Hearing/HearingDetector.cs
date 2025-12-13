using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingDetector : MonoBehaviour, IFreezable
{
    [SerializeField] private DeerSenseSuspicionManager deerSenseSuspicionManager;

    private void OnTriggerEnter(Collider other)
    {
        SoundBubble bubble = other.GetComponent<SoundBubble>();
        
        if (bubble ==null) return;
        if(!bubble.IsActive) return;
        
        deerSenseSuspicionManager.OnSoundHeard(bubble.transform.position);
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
