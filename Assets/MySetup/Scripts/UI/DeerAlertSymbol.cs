using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAlertSymbol : MonoBehaviour
{
    [SerializeField] private GameObject _symbol;

    private void OnEnable()
    {
        if (SuspicionManager.Instance != null)
        {
            SuspicionManager.Instance.OnSuspicionFull.AddListener(ActivateSymbol);
            Debug.Log("DeerAlertSymbol subscribed");
        }
        else
        {
            Debug.LogWarning("DeerAlertSymbol could not subscribe — SuspicionManager.Instance is NULL");
        }
    }

    private void OnDisable()
    {
        if (SuspicionManager.Instance != null)
            SuspicionManager.Instance.OnSuspicionFull.RemoveListener(ActivateSymbol);
    }

    private void ActivateSymbol()
    {
        _symbol.SetActive(true);
    }
}
