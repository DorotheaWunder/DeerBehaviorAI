using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
public class SuspicionManager : MonoBehaviour
{
    public static SuspicionManager Instance;
    
    [Header("Suspicion Settings")]
    [SerializeField] private float _maxSuspicion = 100f;
    [SerializeField] private float _decayPerSecond = 5f;
    [SerializeField] private float _cooldownAfterSensed = 1f;

    [SerializeField] private float _currentSuspicion = 0f;
    [SerializeField] private float _afterSensedTimer = 0f;

    public UnityEvent<float> OnSuspicionChanged;
    public UnityEvent OnSuspicionFull;
    
    private void Awake()
    {
        Debug.Log("SuspicionManager Awake — EXECUTION ORDER TEST");
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }
    
    private void Update()
    {
        
        if (_afterSensedTimer > 0f)
        {
            _afterSensedTimer -= Time.deltaTime;
            return;
        }
        
        if (_currentSuspicion > 0f)
        {
            _currentSuspicion -= _decayPerSecond * Time.deltaTime;
            _currentSuspicion = Mathf.Max(_currentSuspicion, 0f);
            OnSuspicionChanged?.Invoke(_currentSuspicion / _maxSuspicion);
        }
    }
    
    public void AddSuspicion(float amount, float miniCooldown = 1f)
    {
        _currentSuspicion += amount;
        _currentSuspicion = Mathf.Min(_currentSuspicion, _maxSuspicion);

        _afterSensedTimer = _cooldownAfterSensed;

        OnSuspicionChanged?.Invoke(_currentSuspicion / _maxSuspicion);

        if (_currentSuspicion >= _maxSuspicion)
            OnSuspicionFull?.Invoke();
    }
    
    public float GetSuspicionNormalized()
    {
        return _currentSuspicion / _maxSuspicion;
    }
}
