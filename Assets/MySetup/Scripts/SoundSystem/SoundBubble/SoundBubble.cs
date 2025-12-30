using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBubble : MonoBehaviour
{
    private SoundBubbleRuntimeData _runtime;

    public SO_SurfaceProfile SurfaceProfile { get; private set; }
    public bool IsActive { get; private set; }
    public float Timer { get; set; }

    public void Initialize(SO_SoundBubble baseData)
    {
        _runtime.BaseData = baseData;
        IsActive = false;
    }

    public void ApplyRuntimeData(SoundBubbleRuntimeData data)
    {
        _runtime = data;
    }

    public void SetSurfaceProfile(SO_SurfaceProfile profile)
    {
        SurfaceProfile = profile;
    }

    public void Activate(Vector3 position)
    {
        transform.position = position;

        Timer = 0f;
        IsActive = true;

        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        IsActive = false;
        Timer = 0f;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void UpdateBubble(float deltaTime)
    {
        Timer += deltaTime;

        float t = Mathf.Clamp01(Timer / _runtime.Duration);
        float curveValue = _runtime.Curve.Evaluate(t);
        float radius = curveValue * _runtime.FinalRadius;

        transform.localScale = new Vector3(radius, radius, radius);

        if (Timer >= _runtime.Duration)
            Deactivate();
    }
}

public struct SoundBubbleRuntimeData
{
    public SO_SoundBubble BaseData;

    public float FinalRadius;
    public float Duration;
    public float MovementMult;
    public AnimationCurve Curve;
}