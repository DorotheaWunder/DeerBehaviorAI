using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceMultiplier : MonoBehaviour
{
   public SurfaceTrigger SurfaceTrigger;

   private SO_SurfaceProfile _current;

   private void Start()
   {
      SurfaceTrigger.OnSurfaceChanged.AddListener(OnSurfaceChanged);
   }

   private void OnSurfaceChanged(SO_SurfaceProfile profile)
   {
      _current = profile;
   }

   public float GetRadiusMultiplier()
   {
      return _current != null ? _current.RadiusMultiplier : 1f;
   }

   public float GetDurationMultiplier()
   {
      return _current != null ? _current.DurationMultiplier : 1f;
   }

   public AnimationCurve GetCurveOrDefault(AnimationCurve defaultCurve)
   {
      if (_current != null && _current.UseOverrideCurve && _current.OverrideRadiusCurve != null)
         return _current.OverrideRadiusCurve;

      return defaultCurve;
   }

   public SO_SurfaceProfile GetCurrentSurfaceProfile()
   {
      return _current;
   }
}

public enum SurfaceType
{
   Quiet,
   Medium,
   Loud
}