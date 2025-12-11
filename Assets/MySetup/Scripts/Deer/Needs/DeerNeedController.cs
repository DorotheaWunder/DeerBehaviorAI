using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerNeedController : MonoBehaviour
{
    public DeerNeed[] Needs;
    
    [Header("Random Randomizers")]//profile as SO in the future?
    public Vector2 FoodDrainRange = new Vector2(0.8f, 1.2f);
    public Vector2 FoodRecoverRange = new Vector2(0.9f, 1.1f);

    public Vector2 WaterDrainRange = new Vector2(0.9f, 1.3f);
    public Vector2 WaterRecoverRange = new Vector2(0.8f, 1.2f);

    public Vector2 StaminaDrainRange = new Vector2(0.7f, 1.4f);
    public Vector2 StaminaRecoverRange = new Vector2(1.0f, 1.3f);
    
    public event Action<NeedEvent> OnNeedEvent;

    private void Awake()
    {
        foreach (var need in Needs)
        {
            switch (need.NeedType)
            {
                case NeedType.Food:
                    need.InitializeRandomizers(
                        FoodDrainRange.x, FoodDrainRange.y,
                        FoodRecoverRange.x, FoodRecoverRange.y);
                    break;

                case NeedType.Water:
                    need.InitializeRandomizers(
                        WaterDrainRange.x, WaterDrainRange.y,
                        WaterRecoverRange.x, WaterRecoverRange.y);
                    break;

                case NeedType.Stamina:
                    need.InitializeRandomizers(
                        StaminaDrainRange.x, StaminaDrainRange.y,
                        StaminaRecoverRange.x, StaminaRecoverRange.y);
                    break;
            }
        }
    }
    
    private void Update()//use staggered update instead
    {
        float dt = Time.deltaTime;

        foreach (var need in Needs)
        {
            need.Drain(dt);
            CheckThresholds(need);
        }
    }

    public DeerNeed GetNeed(NeedType type)//use dictionary instead
    {
        foreach (var n in Needs)
            if (n.NeedType == type)
                return n;

        return null;
    }

    private void CheckThresholds(DeerNeed need)
    {
        if (need.BelowMarker)
        {
            BroadcastEvent(need, isLow: true, isHigh: false);
        }
        else if (need.AboveMarker)
        {
            BroadcastEvent(need, isLow: false, isHigh: true);
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
}
