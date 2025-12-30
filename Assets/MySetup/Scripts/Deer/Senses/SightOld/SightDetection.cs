using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightDetection : MonoBehaviour, IFreezable, ITickable
{
    public DeerEye LeftEye;
    public DeerEye RightEye;

    public DeerSenseManager DeerSenseManager;

    private bool _playerSeen;

    public void Tick(float dt, float distanceMultiplier = 1f)
    {
        bool leftValid  = LeftEye.EvaluateTarget(out float leftDist01);
        bool rightValid = RightEye.EvaluateTarget(out float rightDist01);

        if (leftValid || rightValid)
        {
            if (!_playerSeen)
            {
                _playerSeen = true;
                DeerSenseManager.OnPlayerSighted();
            }

            if (leftValid)
                DeerSenseManager.OnPlayerSightedContinuous(LeftEye.Target, LeftEye);

            if (rightValid)
                DeerSenseManager.OnPlayerSightedContinuous(RightEye.Target, RightEye);
        }
        else
        {
            _playerSeen = false;
        }
    }

    public void OnFreeze()
    {
        _playerSeen = false;
        LeftEye.ClearTarget();
        RightEye.ClearTarget();
        enabled = false;
    }

    public void OnThaw()
    {
        enabled = true;
    }
}
//
// [Header("Eyes")]
//     public DeerEye LeftEye;
//     public DeerEye RightEye;
//
//     [Header("AI Reference")]
//     public DeerSenseSuspicionManager DeerSenseSuspicionManager;
//
//     private bool playerSeen = false;
//
//     private void Awake()
//     {
//         LeftEye.OnFOVEnter += OnPlayerDetected;
//         LeftEye.OnFOVExit += OnPlayerLost;
//
//         RightEye.OnFOVEnter += OnPlayerDetected;
//         RightEye.OnFOVExit += OnPlayerLost;
//     }
//
//     public void Tick(float dt, float distanceMultiplier = 1f)
//     {
//         LeftEye.CheckFOV();
//         RightEye.CheckFOV();
//
//         if (playerSeen)
//         {
//             DeerSenseSuspicionManager.OnPlayerSightedContinuous();
//         }
//     }
//
//     private void OnPlayerDetected()
//     {
//         if (!playerSeen)
//         {
//             playerSeen = true;
//             DeerSenseSuspicionManager.OnPlayerSighted();
//         }
//     }
//
//     private void OnPlayerLost()
//     {
//         playerSeen = false;
//     }
//     
//     private void ClearTargets()
//     {
//         playerSeen = false;
//
//         if (LeftEye != null)
//             LeftEye.ClearTarget();
//
//         if (RightEye != null)
//             RightEye.ClearTarget();
//     }
//
//     // ------------------------------------------ Connection to DeerFreezer
//     public void OnFreeze()
//     {
//         ClearTargets();
//         enabled = false;
//     }
//
//     public void OnThaw()
//     {
//         enabled = true;
//     }
//     
//
//     // ------------------------------------------ Gizmos
//     private void OnDrawGizmos()
//     {
//         if (LeftEye == null || LeftEye.Profile == null)
//             return;
//
//         DrawCone(LeftEye);
//         DrawCone(RightEye);
//         
//         if (Application.isPlaying)
//         {
//             Transform target = LeftEye.Target;
//             if (target != null)
//             {
//                 Color c = playerSeen ? Color.white : Color.grey;
//                 Debug.DrawLine(transform.position, target.position, c);
//             }
//         }
//     }
//
//     private void DrawCone(DeerEye eye)
//     {
//         if (eye == null || eye.Profile == null)
//             return;
//
//         Gizmos.color = new Color(0.5f, 0.5f, 0f, 0.1f);
//         Vector3 origin = eye.transform.position;
//
//         float halfFOV = eye.Profile.FOV * 0.5f;
//
//         Vector3 leftDir = Quaternion.Euler(0, -halfFOV, 0) * eye.transform.forward;
//         Vector3 rightDir = Quaternion.Euler(0, halfFOV, 0) * eye.transform.forward;
//
//         Gizmos.DrawLine(origin, origin + leftDir * eye.Profile.MaxRange);
//         Gizmos.DrawLine(origin, origin + rightDir * eye.Profile.MaxRange);
//         Gizmos.DrawLine(origin, origin + eye.transform.forward * eye.Profile.MaxRange);
//     }