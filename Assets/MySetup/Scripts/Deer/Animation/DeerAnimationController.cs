using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAnimationController : MonoBehaviour, IFreezable
{
 [Header("Animator Reference")]
    public Animator Animator;

    [Header("Individual Speed Variation")]
    [SerializeField] private float _individualSpeedMin = 0.8f;
    [SerializeField] private float _individualSpeedMax = 1.2f;
    [SerializeField] private float _individualSpeedModifier;

    private State _currentState;
    private Coroutine _stateRoutine;
    private Coroutine _transitionRoutine;

    private void OnEnable()
    {
        _individualSpeedModifier = Random.Range(_individualSpeedMin, _individualSpeedMax);

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
        if (_transitionRoutine != null)
            StopCoroutine(_transitionRoutine);

        _transitionRoutine = StartCoroutine(
            TransitionRoutine(previousState as State, newState as State)
        );
    }

    private IEnumerator TransitionRoutine(State previous, State next)
    {
        if (previous != null &&
            previous.AnimationSet != null &&
            previous.AnimationSet.ExitAnimation != null)
        {
            yield return PlayAndWait(previous.AnimationSet.ExitAnimation, previous);
        }

        if (_stateRoutine != null)
        {
            StopCoroutine(_stateRoutine);
            _stateRoutine = null;
        }

        if (next == null || next.AnimationSet == null)
            yield break;

        _currentState = next;
        _stateRoutine = StartCoroutine(StateRoutine(next));
    }

    private IEnumerator StateRoutine(State state)
    {
        var set = state.AnimationSet;
        
        if (set.EntryAnimation != null)
        {
            yield return PlayAndWait(set.EntryAnimation, state);
        }
        
        while (_currentState == state)
        {
            var loops = set.LoopAnimations;
            if (loops == null || loops.Count == 0)
                yield break;

            var clip = loops[Random.Range(0, loops.Count)];
            yield return PlayAndWait(clip, state);
            
            float pause = 0f;
            if (state.AllowLoopPause)
            {
                pause = Random.Range(state.MinLoopPause, state.MaxLoopPause);
            }

            if (pause > 0f)
                yield return new WaitForSeconds(pause);
        }
    }

    private IEnumerator PlayAndWait(AnimationClip clip, State state)
    {
        if (clip == null || Animator == null)
            yield break;

        float speed =
            Random.Range(state.MinAnimSpeed, state.MaxAnimSpeed) *
            _individualSpeedModifier;

        Animator.speed = speed;
        Animator.CrossFade(clip.name, 0f, 0, 0f);

        yield return new WaitForSeconds(clip.length / speed);
    }

    // ------------------------------------------ Connection to DeerFreezer
    public void OnFreeze()
    {
        enabled = false;
    }

    public void OnThaw()
    {
        enabled = true;
    }
}
