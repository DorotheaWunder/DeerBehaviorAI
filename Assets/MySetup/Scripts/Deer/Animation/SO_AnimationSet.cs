using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeerAnimations/AnimationSet")]
public class SO_AnimationSet : ScriptableObject
{
    public AnimationClip EntryAnimation;
    public List<AnimationClip> LoopAnimations;
    public AnimationClip ExitAnimation;
}
