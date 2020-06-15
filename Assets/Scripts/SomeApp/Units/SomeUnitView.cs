using System;
using System.Collections;
using UnityEngine;
using Services.Selector;
using Services.Markable;

public class SomeUnitView : BaseUnit, ISelectable, IControllable, IMarkable
{
    [Header("Unit")]
    [SerializeField] private GameObject _selfContainer = null;
    [SerializeField] private SomeUnitAI     _unitAI = null;
    [SerializeField] private Animator   _animator = null;
    [SerializeField] private Renderer   _renderer = null;
    [Header("Marker")]
    [SerializeField] private GameObject _markerContainer = null;
    [SerializeField] private Animator   _markerAnimator = null;

    public event EventHandler<Vector3> OnDestinationChanged;
    public event EventHandler<bool> OnUnitSelect;
    public SomeUnitAI UnitAI => _unitAI;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(Wait()); 
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f,1f));
        SetAnimation(AnimationState.Active);
    }

    public void SetDestination(Vector3 destination)
    { 
        OnDestinationChanged?.Invoke(this, destination);
    }

    public void Select()
    {
        ShowMarker();
        OnUnitSelect?.Invoke(this, true);
    }

    public void DeSelect()
    {
        HideMarker();
        OnUnitSelect?.Invoke(this, false);
    }

    public void Destroy()
    {
        if(!_selfContainer)
        {
            Debug.LogError(this + " : SelfContainer is null");
            return;
        }
        Destroy(_selfContainer);
    }

    public Vector3 GetCenter()
    {
       if(!_renderer)
       {
            Debug.LogError(this + " : [GetCenter] Renderer is null");
            return default;
       }
        return _renderer.bounds.center;
    }

    public void ShowMarker()
    {
        if(!_markerContainer)
        {
            Debug.LogError(this + " : Marker Container is null");
            return;
        }
        if(!_markerAnimator)
        {
            Debug.LogError(this + " : Marker Animator is null");
            return;
        }
        _markerContainer.SetActive(true);
        _markerAnimator.SetTrigger("Active");
    }

    public void HideMarker()
    {
        if (!_markerContainer)
        {
            Debug.LogError(this + " : Marker Container is null");
            return;
        }
        if (!_markerAnimator)
        {
            Debug.LogError(this + " : Marker Animator is null");
            return;
        }
        _markerAnimator.SetTrigger("DeActive");
        _markerContainer.SetActive(false);
    }

    internal enum AnimationState { Active, Move, Idle, Selected }

    internal void SetAnimation(AnimationState animationState)
    {
        switch (animationState)
        {
            case AnimationState.Move: _animator.SetTrigger("Move");
                break;
            case AnimationState.Idle: _animator.SetTrigger("Idle");
                break;
            case AnimationState.Selected: _animator.SetTrigger("Select");
                break;
            case AnimationState.Active: _animator.SetTrigger("Active");
                break;
            default: Debug.LogError(this + " : [Animate] Animation not found");
                break;
        }
    }
}
