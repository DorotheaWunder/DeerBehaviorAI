using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerNeedController : MonoBehaviour, ITickable, IFreezable
{
    [Header("Needs")]
    public DeerNeed[] Needs;

    private Dictionary<NeedType, DeerNeed> _needsDictionary;
    private Dictionary<NeedType, bool> _wasLow;
    private Dictionary<NeedType, bool> _wasHigh;

    [Header("Update Interval")]
    [SerializeField] private float baseTickInterval = 0.25f;
    private float tickTimer;

    [Header("Need Randomizers")]
    public Vector2 FoodDrainRange = new Vector2(0.8f, 1.2f);
    public Vector2 FoodRecoverRange = new Vector2(0.9f, 1.1f);
    public Vector2 WaterDrainRange = new Vector2(0.9f, 1.3f);
    public Vector2 WaterRecoverRange = new Vector2(0.8f, 1.2f);
    public Vector2 StaminaDrainRange = new Vector2(0.7f, 1.4f);
    public Vector2 StaminaRecoverRange = new Vector2(1.0f, 1.3f);

    [Header("Debug")]
    public float CurrentDistanceMultiplier = 1f;

    public event Action<NeedEvent> OnNeedEvent;

    private void Awake()
    {
        _needsDictionary = new Dictionary<NeedType, DeerNeed>();
        _wasLow = new Dictionary<NeedType, bool>();
        _wasHigh = new Dictionary<NeedType, bool>();

        var ranges = new Dictionary<NeedType, (Vector2 drain, Vector2 recover)>
        {
            { NeedType.Food, (FoodDrainRange, FoodRecoverRange) },
            { NeedType.Water, (WaterDrainRange, WaterRecoverRange) },
            { NeedType.Stamina, (StaminaDrainRange, StaminaRecoverRange) }
        };

        foreach (var need in Needs)
        {
            if (need == null) continue;

            _needsDictionary[need.NeedType] = need;
            _wasLow[need.NeedType] = false;
            _wasHigh[need.NeedType] = false;

            if (ranges.TryGetValue(need.NeedType, out var r))
            {
                need.InitializeRandomizers(r.drain.x, r.drain.y, r.recover.x, r.recover.y);
            }
        }
    }
    
    public void Tick(float dt, float distanceMultiplier = 1f)
    {
        CurrentDistanceMultiplier = distanceMultiplier;

        tickTimer += dt;
        float effectiveInterval = baseTickInterval * distanceMultiplier;
        if (tickTimer < effectiveInterval) return;

        float step = tickTimer;
        tickTimer = 0f;
        
        float drainMultiplier = 1f;
        if (TryGetComponent<DeerFSM>(out var fsm) && fsm.CurrentState != null)
        {
            drainMultiplier = fsm.CurrentState.DrainMultiplier;
        }
        
        float totalMultiplier = drainMultiplier * distanceMultiplier;

        foreach (var need in Needs)
        {
            if (need == null) continue;

            need.Drain(step * totalMultiplier);
            CheckThresholds(need);
        }
    }

    public DeerNeed GetNeed(NeedType type)
    {
        _needsDictionary.TryGetValue(type, out var need);
        return need;
    }

    private void CheckThresholds(DeerNeed need)
    {
        bool isLow = need.BelowMarker;
        bool isHigh = need.AboveMarker;

        if (_wasLow.TryGetValue(need.NeedType, out bool previousLow) &&
            _wasHigh.TryGetValue(need.NeedType, out bool previousHigh))
        {
            if (isLow != previousLow || isHigh != previousHigh)
            {
                BroadcastEvent(need, isLow, isHigh);
                _wasLow[need.NeedType] = isLow;
                _wasHigh[need.NeedType] = isHigh;
            }
        }
    }

    private void BroadcastEvent(DeerNeed need, bool isLow, bool isHigh)
    {
        OnNeedEvent?.Invoke(new NeedEvent
        {
            NeedType = need.NeedType,
            NormalizedValue = need.Value,
            IsLow = isLow,
            IsHigh = isHigh
        });
    }
    
    // ------------------------------------------ Connection to DeerFreezer
    public void OnFreeze()
    {
        enabled = false;
    }

    public void OnThaw()
    {
        enabled = true;
    }
}
