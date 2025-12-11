using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdUpdateManager : MonoBehaviour
{
    [Header("References")]
    public Transform PlayerPos;
    public HerdManager Herd;
    
    [Header("Distance LODs")]
    [SerializeField] private SO_DeerUpdateProfile _currentProfile;
    
    [Header("Close")]
    public SO_DeerUpdateProfile Close;
    public float CloseTreshold = 20f;
    [Header("Medium")]
    public SO_DeerUpdateProfile Medium;
    public float MediumTreshold = 40f;
    [Header("Far")]
    public SO_DeerUpdateProfile Far;
    public float FarTreshold = 50f;
    [Header("Very Far")]
    public SO_DeerUpdateProfile VeryFar;
    public float VeryFarTreshold = 60f;
    
    public event Action<SO_DeerUpdateProfile> OnProfileChanged;
    
    private void Start()
    {
        StartCoroutine(DistanceCheckRoutine());
    }
    
    private IEnumerator DistanceCheckRoutine()
    {
        while (true)
        {
            float distance = GetDistanceToPlayer();
            ChangeUpdateProfile(distance);

            float baseInterval = 0.1f;
            float currentinterval = baseInterval * _currentProfile.UpdateMultiplierTotal;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(PlayerPos.position, Herd.transform.position);
    }

    private void ChangeUpdateProfile(float distance)
    {
        var newProfile = DetermineUpdateProfile(distance);

        if (newProfile != _currentProfile)
        {
            _currentProfile = newProfile;
            OnProfileChanged?.Invoke(_currentProfile);
        }
    }

    private SO_DeerUpdateProfile DetermineUpdateProfile(float distance)
    {
        if (distance < CloseTreshold) return Close;
        if (distance < MediumTreshold) return Medium;
        if (distance < FarTreshold) return Far;
        return VeryFar;
    }
    
    public SO_DeerUpdateProfile GetCurrentProfile() => _currentProfile;
    
    
    // update distance gizmos
    private void OnDrawGizmos()
    {
        if (Herd == null) return;

        Vector3 center = Herd.transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, CloseTreshold);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, MediumTreshold);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, FarTreshold);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(center, VeryFarTreshold);
    }
}
