using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuspicionUI : MonoBehaviour
{
    [SerializeField] private Slider suspicionSlider;

    private void OnEnable()
    {
        if (SuspicionManager.Instance == null)
        {
            Debug.LogWarning("SuspicionUI could not subscribe — SuspicionManager.Instance is NULL");
            return;
        }

        SuspicionManager.Instance.OnSuspicionChanged.AddListener(UpdateBar);
    }

    private void OnDisable()
    {
        if (SuspicionManager.Instance != null)
            SuspicionManager.Instance.OnSuspicionChanged.RemoveListener(UpdateBar);
    }

    private void UpdateBar(float normalizedValue)
    {
        suspicionSlider.value = normalizedValue;
    }
}
