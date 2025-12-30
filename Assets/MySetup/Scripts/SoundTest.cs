using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    [Header("Test Settings")]
    public Transform testPosition; // where the bubble will spawn
    public SO_SurfaceProfile testSurface; // pick a surface in the inspector
    public float testHearingSuspicion = 5f;
    public float testCooldown = 1f;

    private void Start()
    {
        // Step 1: Trigger a bubble directly
        SoundBubbleManager.InstanceSoundBubbleManager.TriggerBubble(
            testPosition.position,
            surfaceRadiusMult: 1f,
            movementMult: 1f,
            durationMult: 1f,
            surface: testSurface
        );

        // Step 2: Simulate deer hearing it
        float suspicionMultiplier = testSurface != null ? testSurface.SuspicionMultiplier : 1f;
        float decayMultiplier = testSurface != null ? testSurface.DecayMultiplier : 1f;

        float finalSuspicion = testHearingSuspicion * suspicionMultiplier;

        Debug.Log($"[TEST] Bubble on surface: {testSurface?.name ?? "None"}");
        Debug.Log($"[TEST] Suspicion multiplier: {suspicionMultiplier}");
        Debug.Log($"[TEST] Decay multiplier: {decayMultiplier}");
        Debug.Log($"[TEST] Final suspicion added: {finalSuspicion}");
    }
}
