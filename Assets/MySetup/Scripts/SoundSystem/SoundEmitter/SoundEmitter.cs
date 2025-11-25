using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
   [Header("Sources")] 
   [SerializeField] private SurfaceMultiplier _surfaceMultiplier;
   [SerializeField] private MovementMultiplier _movementMultiplier;
   [SerializeField] private Transform _footPivot;

   [Header("Settings")] 
   [SerializeField] private float _minTriggerInterval = 0.2f;

   private float _timer = 0f;

   private void Update()
   {
      _timer += Time.deltaTime;

      float movementMult = _movementMultiplier?.CalculateMovementMultiplier() ?? 0f;
      if (movementMult <= 0f) return;

      float speed = Mathf.Max(0.1f, _movementMultiplier.CurrentSpeed);
      float interval = Mathf.Max(_minTriggerInterval, 1f / speed);

      if (_timer >= interval)
      {
         float surfaceRadiusMult = _surfaceMultiplier.GetRadiusMultiplier();
         float surfaceDurationMult = _surfaceMultiplier.GetDurationMultiplier();

         float finalMovementMult = movementMult * surfaceRadiusMult;

         SoundBubbleManager.InstanceSoundBubbleManager.TriggerBubble(
            _footPivot.position,
            surfaceRadiusMult: surfaceRadiusMult,
            movementMult: finalMovementMult,
            durationMult: surfaceDurationMult,
            overrideCurve: _surfaceMultiplier.GetCurveOrDefault(null)
         );

         _timer = 0f;
      }
   }
}
