using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SurfaceChecker : MonoBehaviour
{
    [System.Serializable]
    public class SurfaceChangedEvent : UnityEvent<SurfaceType> {}

    [Header("Detection Settings")] 
    public float RaycastDistance = 1.5f;
    public LayerMask SurfaceMask;

    [Header("Events")] 
    public SurfaceChangedEvent OnSurfaceChanged;
    private SurfaceType _currentSurface;

    void Start()
    {
        _currentSurface = SurfaceType.Medium;
    }

    public void Update()
    {
        CheckSurface();
    }

    public void CheckSurface()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RaycastDistance,SurfaceMask))
        {
            SurfaceType newSurface;

            SurfaceData data = hit.collider.GetComponent<SurfaceData>();

            if (data != null)
            {
                newSurface = data.SurfaceType;
            }
            else
            {
                newSurface = SurfaceType.Medium;
            }

            if (newSurface != _currentSurface)
            {
                _currentSurface = newSurface;
                OnSurfaceChanged.Invoke(newSurface);
            }
        }
    }
    
    public SurfaceType GetCurrentSurface()
    {
        return _currentSurface;
    }
}
