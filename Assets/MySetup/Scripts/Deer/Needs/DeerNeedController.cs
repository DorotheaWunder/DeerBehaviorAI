using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerNeedController : MonoBehaviour
{
    public DeerNeed[] Needs;

    public event Action<NeedEvent> OnNeedEvent;

    private void Update()
    {
        float dt = Time.deltaTime;

        foreach (var need in Needs)
        {
            need.Drain(dt);
            CheckThresholds(need);
        }
    }

    public DeerNeed GetNeed(NeedType type)
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
