using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
   [Header("Sources")] 
   [SerializeField] private SurfaceMultiplier _surfaceMultiplier;
   [SerializeField] protected MovementMultiplier _movementMultiplier;
   [SerializeField] private Transform _footPivot;

   [Header("Settings")] 
   [SerializeField] private float _minTriggerInterval = 0.2f;
   [SerializeField] private float speedToIntervalFactor = 0.1f; 

   private float _timer = 0f;

   private void Update()
   {
      _timer += Time.deltaTime;
      
      float movementMultiplier = _movementMultiplier?.CalculateMovementMultiplier() ?? 0f;
      float surfaceMultiplier = _surfaceMultiplier?.CalculateSurfaceMultiplier() ?? 1f;
      
      if (movementMultiplier <= 0f) return;
      
      float interval = Mathf.Max(_minTriggerInterval, 1f / (_movementMultiplier.CurrentSpeed + 0.1f));

      if (_timer >= interval)
      {
         float finalMultiplier = movementMultiplier * surfaceMultiplier;

         SoundBubbleManager.InstanceSoundBubbleManager.TriggerBubble(
            _footPivot.position,
            surfaceMultiplier: 1f,
            movementMultiplier: finalMultiplier
         );

         _timer = 0f;
      }
   }
}
