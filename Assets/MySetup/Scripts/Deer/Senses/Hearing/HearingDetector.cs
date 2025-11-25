using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingDetector : MonoBehaviour
{
    [SerializeField] private DeerAI _deerAI;

    private void OnTriggerEnter(Collider other)
    {
        SoundBubble bubble = other.GetComponent<SoundBubble>();
        
        if (bubble ==null) return;
        if(!bubble.IsActive) return;
        
        _deerAI.OnSoundHeard(bubble.transform.position);
    }
}
