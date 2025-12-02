using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAnimationController : MonoBehaviour
{
    [Header("Animator Reference")]
    public Animator Animator;

    private SO_AnimationSet _currentAnimSet;
    [SerializeField] private AnimationClip _currentLoop;

    private void OnEnable()
    {
        var fsm = GetComponent<DeerFSM>();
        if (fsm != null)
            fsm.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        var fsm = GetComponent<DeerFSM>();
        if (fsm != null)
            fsm.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(SO_DeerState newState, SO_DeerState previousState)
    {
        if (newState is State state)
        {
            _currentAnimSet = state.AnimationSet;

            if (_currentAnimSet?.EntryAnimation != null)
                PlayAnimation(_currentAnimSet.EntryAnimation);

            PlayRandomLoop(_currentAnimSet?.LoopAnimations);
        }
    }

    private void Update()
    {
        if (_currentAnimSet != null && _currentLoop != null && !IsAnimationPlaying(_currentLoop))
        {
            PlayRandomLoop(_currentAnimSet.LoopAnimations);
        }
    }

    public void PlayAnimation(AnimationClip clip)
    {
        if (clip == null || Animator == null) return;
        Animator.Play(clip.name);
    }

    public bool IsAnimationPlaying(AnimationClip clip)
    {
        if (Animator == null || clip == null) return false;
        var state = Animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName(clip.name) && state.normalizedTime < 1f;
    }

    public void PlayRandomLoop(List<AnimationClip> loops)
    {
        if (loops == null || loops.Count == 0) return;
        int index = Random.Range(0, loops.Count);
        _currentLoop = loops[index];
        PlayAnimation(_currentLoop);
    }

    public AnimationClip CurrentLoop => _currentLoop;
}
