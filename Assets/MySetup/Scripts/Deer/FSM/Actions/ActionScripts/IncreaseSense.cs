using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SenseType
{
    None,
    Hearing,
    Sight,
    Smell
}


[CreateAssetMenu(menuName = "DeerFSM/Actions/IncreaseSense")]
public class IncreaseSense : SO_StateAction
{
    [Header("Sense")]
    public SenseType Sense = SenseType.Hearing;

    [Header("Scaling")]
    public float MinScale = 1f;
    public float MaxScale = 2f;
    public float Speed = 1f;
    public AnimationCurve ScaleCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Transform _senseTransform;
    private float _time;

    public override void ExecuteAction(DeerFSM deerFSM)
    {
        if (_senseTransform == null || !_senseTransform)
        {
            _senseTransform = FindSenseParent(deerFSM);
            _time = 0f;

            if (_senseTransform == null)
                return;
        }

        _time += Time.deltaTime * Speed;
        float t = _time % 1f;

        float scale = Mathf.Lerp(
            MinScale,
            MaxScale,
            ScaleCurve.Evaluate(t)
        );

        _senseTransform.localScale = Vector3.one * scale;
    }

    private Transform FindSenseParent(DeerFSM deerFSM)
    {
        var bb = deerFSM.DeerBlackboard;
    
         return Sense switch
         {
             SenseType.Hearing => bb.Hearing,
             SenseType.Sight   => bb.Sight,
             _ => null
         };
    }
}
