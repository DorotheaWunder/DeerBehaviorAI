using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceMultiplier : MonoBehaviour
{
   [SerializeField] private SurfaceType _currentSurfaceIntensity;
   public SurfaceChecker SurfaceChecker;

   [SerializeField] private float _quietMultiplier = 0.5f;
   [SerializeField] private float _mediumMultiplier = 1f;
   [SerializeField] private float _loudMultiplier = 2f;
   
   [SerializeField] private float _baseMultiplier = 1f;

   public void Start()
   {
      SurfaceChecker.OnSurfaceChanged.AddListener(SetSurfaceIntensity);
   }
   
   public float CalculateSurfaceMultiplier()
   {
      switch (_currentSurfaceIntensity)
      {
         case SurfaceType.Quiet: return _quietMultiplier;
         case SurfaceType.Medium: return _mediumMultiplier;
         case SurfaceType.Loud: return _loudMultiplier;
         default: return 1f;
      }
   }

   public void SetSurfaceIntensity(SurfaceType intensity)
   {
      _currentSurfaceIntensity = intensity;
   }
}

public enum SurfaceType
{
   Quiet,
   Medium,
   Loud
}