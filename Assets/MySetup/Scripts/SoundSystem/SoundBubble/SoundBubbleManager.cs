using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBubbleManager : MonoBehaviour
{
    public static SoundBubbleManager InstanceSoundBubbleManager;

    [SerializeField] private SO_SoundBubble _soundBubbleData;
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private int _bubblePoolSize = 20;

    [SerializeField]private List<SoundBubble> _bubblesTotal = new List<SoundBubble>();
    [SerializeField]private List<SoundBubble> _bubblesActive = new List<SoundBubble>();

    private void Awake()
    {
        if (InstanceSoundBubbleManager != null && InstanceSoundBubbleManager != this)
        {
            Destroy(gameObject);
            return;
        }

        InstanceSoundBubbleManager = this;

        CreateBubblePool();
    }

    public void Update()
    {
        UpdateActiveBubbles();
    }

    private void CreateBubblePool()
    {
        for (int i = 0; i < _bubblePoolSize; i++)
        {
            GameObject obj = Instantiate(_bubblePrefab,transform);
            SoundBubble bubble = obj.GetComponent<SoundBubble>();
            bubble.Initialize(_soundBubbleData);
            obj.SetActive(false);
            _bubblesTotal.Add(bubble);
        }
    }

    public void TriggerBubble(Vector3 position, float surfaceMultiplier = 1f, float movementMultiplier = 1f)
    {
        SoundBubble bubble = GetFreeBubble();
        if (bubble == null) return;
        
        bubble.Activate(position,surfaceMultiplier, movementMultiplier);
        _bubblesActive.Add(bubble);
    }

    public void ScaleBubble(SoundBubble bubble, float deltaTime)
    {
        bubble.Timer += deltaTime;

        float t = Mathf.Clamp01(bubble.Timer/ bubble.Data.Duration);
        float curveValue = bubble.Data.RadiusCurve.Evaluate(t);
        float finalRadius = curveValue * bubble.TargetRadius;

        bubble.transform.localScale = Vector3.one * finalRadius;
    }

    private SoundBubble GetFreeBubble()
    {
        foreach (var bubble in _bubblesTotal)
        {
            if (!bubble.IsActive) return bubble;
        }
        Debug.LogWarning("SoundBubble pool exhausted!");
        return null;
    }

    public void UpdateActiveBubbles()
    {
        float dt = Time.deltaTime;

        for (int  i = _bubblesActive.Count - 1; i >= 0; i--)
        {
            SoundBubble bubble = _bubblesActive[i];
            ScaleBubble(bubble, dt);

            if (bubble.Timer >= bubble.Data.Duration)
            {
                bubble.Deactivate();
                bubble.transform.localScale = Vector3.zero;
                _bubblesActive.Remove(bubble);
            }
        }
    }
}
