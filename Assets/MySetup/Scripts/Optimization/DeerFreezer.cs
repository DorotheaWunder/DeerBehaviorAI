using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeerFreezer : MonoBehaviour
{
    [SerializeField] private bool AutoFindComponents = true;
    
    [SerializeField] private IFreezable[] _freezables;
    
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private Collider[] _colliders;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    
    public bool IsFrozen { get; private set; }
    
    private void Awake()
    {
        if (AutoFindComponents)
            CacheComponents();
    }
    
    private void CacheComponents()
    {
        _freezables = GetComponentsInChildren<IFreezable>(true);
        
        
        _renderers = GetComponentsInChildren<Renderer>(true);
        _colliders = GetComponentsInChildren<Collider>(true);
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }
    
    public void Freeze()
    {
        if (IsFrozen) return;

        foreach (var freezable in _freezables)
            freezable.OnFreeze();
        
        if (_renderers != null)
            foreach (var r in _renderers)
                if (r) r.enabled = false;

        if (_colliders != null)
            foreach (var c in _colliders)
                if (c) c.enabled = false;
        
        if (_agent) _agent.enabled = false;
        if (_animator) _animator.enabled = false; 
        
        IsFrozen = true;
    }
    
    public void Thaw()
    {
        if (!IsFrozen) return;

        foreach (var freezable in _freezables)
            freezable.OnThaw();
        
        if (_renderers != null)
            foreach (var r in _renderers)
                if (r) r.enabled = true;

        if (_colliders != null)
            foreach (var c in _colliders)
                if (c) c.enabled = true;

        if (_agent) _agent.enabled = true;
        if (_animator) _animator.enabled = true;
        
        IsFrozen = false;
    }
}
