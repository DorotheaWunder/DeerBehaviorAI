using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBubbleManager : MonoBehaviour
{
    public static SoundBubbleManager InstanceSoundBubbleManager;

    [Header("Base Bubble Settings")]
    [SerializeField] private SO_SoundBubble _baseData;

    [Header("Pool")]
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private int _bubblePoolSize = 20;

    private readonly List<SoundBubble> _all = new();
    private readonly List<SoundBubble> _active = new();

    private void Awake()
    {
        if (InstanceSoundBubbleManager != null && InstanceSoundBubbleManager != this)
        {
            Destroy(gameObject);
            return;
        }

        InstanceSoundBubbleManager = this;
        CreatePool();
    }

    void Update()
    {
        float dt = Time.deltaTime;

        for (int i = _active.Count - 1; i >= 0; i--)
        {
            var bubble = _active[i];
            bubble.UpdateBubble(dt);

            if (!bubble.IsActive)
                _active.RemoveAt(i);
        }
    }

    private void CreatePool()
    {
        for (int i = 0; i < _bubblePoolSize; i++)
        {
            var obj = Instantiate(_bubblePrefab, transform);
            var b = obj.GetComponent<SoundBubble>();
            b.Initialize(_baseData);
            obj.SetActive(false);
            _all.Add(b);
        }
    }

    private SoundBubble GetFree()
    {
        foreach (var b in _all)
            if (!b.IsActive) return b;

        Debug.LogWarning("Bubble pool exhausted!");
        return null;
    }

    public void TriggerBubble(
        Vector3 position,
        float surfaceRadiusMult,
        float movementMult,
        float durationMult,
        AnimationCurve overrideCurve = null
    )
    {
        var bubble = GetFree();
        if (bubble == null) return;
        
        var data = new SoundBubbleRuntimeData
        {
            BaseData = _baseData,
            FinalRadius = _baseData.BaseRadius * surfaceRadiusMult * movementMult,
            Duration = _baseData.Duration * durationMult,
            Curve = overrideCurve != null ? overrideCurve : _baseData.RadiusCurve
        };

        bubble.ApplyRuntimeData(data);
        bubble.Activate(position);

        _active.Add(bubble);
    }
}
