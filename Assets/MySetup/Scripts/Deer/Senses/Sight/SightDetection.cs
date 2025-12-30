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