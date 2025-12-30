using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
public class SuspicionManager : MonoBehaviour
{
    public static SuspicionManager Instance;

    [Header("Settings")]
    [SerializeField] private float _maxSuspicion = 100f;
    [SerializeField] private float _decayPerSecond = 5f;
    [SerializeField] private float _sightCooldown = 1f;

    private float _currentSuspicion = 0f;
    private float _currentDecayMultiplier = 1f;
    private float _afterSensedTimer = 0f;

    private bool _recentlySighted = false;

    public UnityEvent<float> OnSuspicionChanged;
    public UnityEvent OnSuspicionFull;
    public UnityEvent OnSuspicionEmpty;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;
    }

    private void Update()
    {
        if (_recentlySighted)
        {
            _afterSensedTimer -= Time.deltaTime;
            if (_afterSensedTimer <= 0f)
                _recentlySighted = false;
        }

        if (_currentSuspicion > 0f && !_recentlySighted)
        {
            float decay = _decayPerSecond * _currentDecayMultiplier;
            _currentSuspicion -= decay * Time.deltaTime;
            _currentSuspicion = Mathf.Max(_currentSuspicion, 0f);
            OnSuspicionChanged?.Invoke(_currentSuspicion / _maxSuspicion);

            if (_currentSuspicion <= 0f)
                OnSuspicionEmpty?.Invoke();
        }
    }

    public void AddSuspicion(float amount, float alertPause = 1f, float decayMultiplier = 1f)
    {
        _currentSuspicion += amount;
        _currentSuspicion = Mathf.Min(_currentSuspicion, _maxSuspicion);

        _afterSensedTimer = alertPause;
        _currentDecayMultiplier = decayMultiplier;

        _recentlySighted = true;

        OnSuspicionChanged?.Invoke(_currentSuspicion / _maxSuspicion);

        if (_currentSuspicion >= _maxSuspicion)
            OnSuspicionFull?.Invoke();
    }

    public float GetSuspicionNormalized()
    {
        return _currentSuspicion / _maxSuspicion;
    }
}

// public static SuspicionManager Instance;
//
// [Header("Settings")]
// [SerializeField] private float _maxSuspicion = 100f;
// [SerializeField] private float _decayPerSecond = 5f;
// [SerializeField] private float _sightCooldown = 1f;
//
// private float _currentSuspicion = 0f;
// private float _currentDecayMultiplier = 1f;
// private float _afterSensedTimer = 0f;
//
// private bool _recentlySighted = false;
//
// public UnityEvent<float> OnSuspicionChanged;
// public UnityEvent OnSuspicionFull;
// public UnityEvent OnSuspicionEmpty;
//
// private void Awake()
// {
//     if (Instance != null && Instance != this)
//         Destroy(gameObject);
//     Instance = this;
// }
//
// private void Update()
// {
//     if (_recentlySighted)
//     {
//         _afterSensedTimer -= Time.deltaTime;
//         if (_afterSensedTimer <= 0f)
//             _recentlySighted = false;
//     }
//
//     if (_currentSuspicion > 0f && !_recentlySighted)
//     {
//         float decay = _decayPerSecond * _currentDecayMultiplier;
//         _currentSuspicion -= decay * Time.deltaTime;
//         _currentSuspicion = Mathf.Max(_currentSuspicion, 0f);
//         OnSuspicionChanged?.Invoke(_currentSuspicion / _maxSuspicion);
//
//         if (_currentSuspicion <= 0f)
//             OnSuspicionEmpty?.Invoke();
//     }
// }
//
// public void AddSuspicion(float amount, float alertPause = 1f, float decayMultiplier = 1f)
// {
//     _currentSuspicion += amount;
//     _currentSuspicion = Mathf.Min(_currentSuspicion, _maxSuspicion);
//
//     _afterSensedTimer = alertPause;
//     _currentDecayMultiplier = decayMultiplier;
//
//     _recentlySighted = true;
//
//     OnSuspicionChanged?.Invoke(_currentSuspicion / _maxSuspicion);
//
//     if (_currentSuspicion >= _maxSuspicion)
//         OnSuspicionFull?.Invoke();
// }
//
// public float GetSuspicionNormalized()
// {
//     return _currentSuspicion / _maxSuspicion;
// }