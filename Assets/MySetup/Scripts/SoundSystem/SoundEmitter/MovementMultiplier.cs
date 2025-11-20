using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementMultiplier : MonoBehaviour
{
    [SerializeField] private float _baseMultiplier = 1f;
    [SerializeField] private float _minSpeed = 0.1f;
    [SerializeField] private float _maxSpeed = 5f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public float CalculateMovementMultiplier()
    {
        float speed = _rb.velocity.magnitude;
        if (speed < _minSpeed) return 0f;

        return _baseMultiplier * Mathf.Clamp01(speed/_maxSpeed);
    }
    
    public float CurrentSpeed => _rb.velocity.magnitude;
}
