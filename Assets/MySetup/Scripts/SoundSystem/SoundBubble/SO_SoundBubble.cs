using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundBubbleProfile", menuName = "DeerSystem/Sound/NewSoundBubbleProfile")]
public class SO_SoundBubble : ScriptableObject
{
    public float BaseRadius = 2f;
    public float ExpansionSpeed = 5f;
    public float Duration = 1f;
    public AnimationCurve RadiusCurve;
}
