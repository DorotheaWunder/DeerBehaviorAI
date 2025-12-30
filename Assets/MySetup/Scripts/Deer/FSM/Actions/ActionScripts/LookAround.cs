using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerFSM/Actions/LookAround")]
public class LookAround : SO_StateAction
{
    [Header("Rotation")]
    public float MaxYaw = 45f;
    public float Speed = 1f;

    [Header("Curve")]
    public AnimationCurve LookCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Transform _sight;
    private float _time;
    private Quaternion _baseRotation;
    
    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (_sight == null || !_sight)
        {
            _sight = deerFSM.DeerBlackboard.Sight;
            if (_sight == null) return;

            _baseRotation = _sight.localRotation;
            _time = 0f;
        }

        _time += Time.deltaTime * Speed;
        float t = _time % 1f;
        
        float yaw = Mathf.Lerp(
            -MaxYaw,
            MaxYaw,
            LookCurve.Evaluate(t)
        );

        _sight.localRotation =
            _baseRotation * Quaternion.Euler(0f, yaw, 0f);
    }
}
