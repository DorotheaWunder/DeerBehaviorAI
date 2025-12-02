using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightDetection : MonoBehaviour
{
    [Header("Eyes")]
    public DeerEye LeftEye;
    public DeerEye RightEye;

    [Header("AI Reference")]
    public DeerSenseSuspicionManager deerSenseSuspicionManager;

    private bool playerSeen = false;

    private void Awake()
    {
        LeftEye.OnFOVEnter += OnPlayerDetected;
        LeftEye.OnFOVExit += OnPlayerLost;

        RightEye.OnFOVEnter += OnPlayerDetected;
        RightEye.OnFOVExit += OnPlayerLost;
    }

    private void Update()
    {
        LeftEye.CheckFOV();
        RightEye.CheckFOV();

        if (playerSeen)
        {
            deerSenseSuspicionManager.OnPlayerSightedContinuous();
        }
    }

    private void OnPlayerDetected()
    {
        if (!playerSeen)
        {
            playerSeen = true;
            deerSenseSuspicionManager.OnPlayerSighted();
        }
    }

    private void OnPlayerLost()
    {
        playerSeen = false;
    }


    // ------------------------------------------ Gizmos
    private void OnDrawGizmos()
    {
        if (LeftEye == null || LeftEye.Profile == null)
            return;

        DrawCone(LeftEye);
        DrawCone(RightEye);
        
        if (Application.isPlaying)
        {
            Transform target = LeftEye.Target;
            if (target != null)
            {
                Color c = playerSeen ? Color.white : Color.grey;
                Debug.DrawLine(transform.position, target.position, c);
            }
        }
    }

    private void DrawCone(DeerEye eye)
    {
        if (eye == null || eye.Profile == null)
            return;

        Gizmos.color = Color.yellow;
        Vector3 origin = eye.transform.position;

        float halfFOV = eye.Profile.FOV * 0.5f;

        Vector3 leftDir = Quaternion.Euler(0, -halfFOV, 0) * eye.transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, halfFOV, 0) * eye.transform.forward;

        Gizmos.DrawLine(origin, origin + leftDir * eye.Profile.MaxRange);
        Gizmos.DrawLine(origin, origin + rightDir * eye.Profile.MaxRange);
        Gizmos.DrawLine(origin, origin + eye.transform.forward * eye.Profile.MaxRange);
    }
}