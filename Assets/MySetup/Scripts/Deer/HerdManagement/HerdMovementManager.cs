using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class HerdMovementManager : MonoBehaviour
{
    [Header("References")]
    public HerdStateManager StateManager;
    public HerdBlackboard HerdBB;

    [Header("Flee Target")]
    public Transform Player;
    public float FleeDistance = 20f;
    public Transform HerdCenter;  
    
    [Header("Migration POIs")]
    public Transform[] MeadowPOI;
    public Transform[] StreamPOI;
    public Transform[] ShelterPOI;

    private void OnEnable()
    {
        if (StateManager != null)
            StateManager.OnHerdStateChanged += OnHerdStateChanged;
    }

    private void OnDisable()
    {
        if (StateManager != null)
            StateManager.OnHerdStateChanged -= OnHerdStateChanged;
    }

    private void OnHerdStateChanged(HerdState state)
    {
        HerdBB.Clear();

        switch (state)
        {
            case HerdState.Fleeing:
                SetFleeTarget(Player);
                break;

            case HerdState.MigrateMeadow:
                PickMigrationTarget(MeadowPOI);
                break;

            case HerdState.MigrateStream:
                PickMigrationTarget(StreamPOI);
                break;

            case HerdState.MigrateShelter:
                PickMigrationTarget(ShelterPOI);
                break;

            case HerdState.Normal:
            default:
                HerdBB.MovementIntent = HerdMovementIntent.None;
                break;
        }
        
        HerdBB.Version++;
    }
    
    private void SetFleeTarget(Transform threat)
    {
        if (threat == null || HerdCenter == null) return;

        HerdBB.MovementIntent = HerdMovementIntent.Flee;
        HerdBB.PlayerPosition = threat.position;
        
        Vector3 fleeDir = (HerdCenter.position - threat.position).normalized;
        HerdBB.GoalPosition = HerdCenter.position + fleeDir * FleeDistance;
        HerdBB.HasGoal = true;

        HerdBB.AnchorPosition = HerdBB.GoalPosition;
    }

    private void PickMigrationTarget(Transform[] possiblePOIs)
    {
        if (possiblePOIs == null || possiblePOIs.Length == 0) return;
        
        Transform chosen = possiblePOIs[Random.Range(0, possiblePOIs.Length)];
        HerdBB.MovementIntent = HerdMovementIntent.Migrate;
        HerdBB.GoalPosition = chosen.position;

        HerdBB.AnchorPosition = chosen.position;
        HerdBB.HasGoal = true;
    }
}

public enum HerdMovementIntent
{
    None,
    Flee,
    Migrate
}