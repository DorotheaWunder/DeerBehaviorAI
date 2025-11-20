using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBubble : MonoBehaviour
{
    [SerializeField] private SO_SoundBubble _soundBubbleData;

    public bool IsActive { get; set; }
    public float Timer { get; set; }
    public float TargetRadius { get; set; }

    private SphereCollider _collider;

    public void Initialize(SO_SoundBubble bubbleData)
    {
        _soundBubbleData = bubbleData;
        _collider = GetComponent<SphereCollider>();
    }

    public void Activate(Vector3 position, float surfaceMultiplier, float movementMultiplier)
    {
        transform.position = position;

        Timer = 0f;
        IsActive = true;

        TargetRadius = _soundBubbleData.BaseRadius * surfaceMultiplier * movementMultiplier;

        if (_collider != null)
        {
            _collider.radius = 0f;
        }
        
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        IsActive = false;
        gameObject.SetActive(false);
    }
    
    public SO_SoundBubble Data => _soundBubbleData;
}
