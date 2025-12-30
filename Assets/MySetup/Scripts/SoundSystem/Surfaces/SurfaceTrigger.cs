using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SurfaceTrigger : MonoBehaviour
{
    [System.Serializable]
    public class SurfaceChangedEvent : UnityEvent<SO_SurfaceProfile> {}

    public SurfaceChangedEvent OnSurfaceChanged;
    
    [SerializeField]private SO_SurfaceProfile _currentProfile;
    private int _currentOverlaps = 0;

    private void OnTriggerEnter(Collider other)
    {
        var surfaceData = other.GetComponent<SurfaceData>();
        if (surfaceData == null) return;

        _currentOverlaps++;
        SetSurface(surfaceData.Profile);
    }

    private void OnTriggerExit(Collider other)
    {
        var surfaceData = other.GetComponent<SurfaceData>();
        if (surfaceData == null) return;

        _currentOverlaps--;

        if (_currentOverlaps <= 0)
        {
            _currentOverlaps = 0;
            SetSurface(null);
        }
    }

    private void SetSurface(SO_SurfaceProfile newSurface)
    {
        if (_currentProfile == newSurface) return;

        _currentProfile = newSurface;
        OnSurfaceChanged.Invoke(newSurface);
    }

    public SO_SurfaceProfile GetCurrentProfile()
    {
        return _currentProfile;
    }
}
