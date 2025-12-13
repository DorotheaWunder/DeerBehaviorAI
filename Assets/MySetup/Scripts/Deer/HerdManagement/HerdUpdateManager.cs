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

    [Header("Debug")]
    public float CurrentDistanceMultiplier = 1f;

    public event Action<SO_DeerUpdateProfile> OnProfileChanged;
    
    private Transform _herdTransform;
    private List<DeerFreezer> _deerFreezers;
    private List<ITickable> _deerTickables;
    
    private void Start()
    {
        if (Herd == null || PlayerPos == null)
        {
            Debug.LogError("Herd or PlayerPos not assigned!");
            enabled = false;
            return;
        }

        _currentProfile = Close;
        _herdTransform = Herd.transform;
        
        _deerFreezers = new List<DeerFreezer>();//make own method?
        foreach (var deer in Herd.DeerList)
        {
            if (deer == null) continue;
        
            var freezer = deer.GetComponent<DeerFreezer>();
            if (freezer != null)
                _deerFreezers.Add(freezer);
        }
        
        _deerTickables = new List<ITickable>();//make own method?
        foreach (var deer in Herd.DeerList)
        {
            if (deer == null) continue;

            var tickables = deer.GetComponents<ITickable>();
            foreach (var tickable in tickables)
            {
                if (tickable != null)
                    _deerTickables.Add(tickable);
            }
        }
        StartCoroutine(DistanceCheckRoutine());
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        Vector3 playerPos = PlayerPos.position;

        foreach (var tickable in _deerTickables)
        {
            if (tickable is MonoBehaviour mb && !mb.gameObject.activeInHierarchy) continue;

            Vector3 deerPos = (tickable as MonoBehaviour).transform.position;
            float sqrDist = (playerPos - deerPos).sqrMagnitude;
            var profile = DetermineUpdateProfileSqr(sqrDist);

            CurrentDistanceMultiplier = profile.UpdateMultiplierTotal;

            if (UnityEngine.Random.value > profile.UpdateChance) continue;

            tickable.Tick(dt, profile.UpdateMultiplierTotal);
        }
    }

    private IEnumerator DistanceCheckRoutine()
    {
        while (true)
        {
            float sqrDistance = (_herdTransform.position - PlayerPos.position).sqrMagnitude;
            ChangeUpdateProfileSqr(sqrDistance);

            float baseInterval = 0.1f;
            float currentInterval = baseInterval * _currentProfile.UpdateMultiplierTotal;
            yield return new WaitForSeconds(currentInterval);
        }
    }

    private void ChangeUpdateProfileSqr(float sqrDistance)
    {
        var newProfile = DetermineUpdateProfileSqr(sqrDistance);
        if (newProfile == _currentProfile)
            return;

        _currentProfile = newProfile;
        OnProfileChanged?.Invoke(_currentProfile);

        bool shouldFreeze = (_currentProfile == VeryFar);

        foreach (var freezer in _deerFreezers)
        {
            if (shouldFreeze)
                freezer.Freeze();
            else
                freezer.Thaw();
        }
    }

    private SO_DeerUpdateProfile DetermineUpdateProfileSqr(float sqrDistance)
    {
        if (sqrDistance < CloseTreshold * CloseTreshold) return Close;
        if (sqrDistance < MediumTreshold * MediumTreshold) return Medium;
        if (sqrDistance < FarTreshold * FarTreshold) return Far;
        return VeryFar;
    }

    public SO_DeerUpdateProfile GetCurrentProfile() => _currentProfile;

    
    //-------------------------------- Gizmos
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
