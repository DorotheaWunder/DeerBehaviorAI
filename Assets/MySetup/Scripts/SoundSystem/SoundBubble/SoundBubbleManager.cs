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
}
