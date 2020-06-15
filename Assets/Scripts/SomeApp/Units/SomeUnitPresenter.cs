using UnityEngine;

public class SomeUnitPresenter
{
    private SomeUnitView _view;

    public void Initialize(SomeUnitView view)
    {
        _view = view;
        _view.UnitAI.OnDestinationComplete += OnDestinationComplete;
        _view.OnDestinationChanged += OnDestinationChanged;
        _view.OnUnitSelect += OnUnitSelect;
    }

    public void UnInitialize()
    {
        _view.UnitAI.OnDestinationComplete -= OnDestinationComplete;
        _view.OnDestinationChanged -= OnDestinationChanged;
        _view.OnUnitSelect -= OnUnitSelect;
        _view.Destroy();
    }

    private void OnUnitSelect(object sender, bool select)
    {
        if (select && _view.UnitAI.IsStopped)
        {
            _view.SetAnimation(SomeUnitView.AnimationState.Selected);
        }
    }

    private void OnDestinationChanged(object sender, Vector3 destination)
    {
        _view.SetAnimation(SomeUnitView.AnimationState.Move);
        _view.UnitAI.SetDestination(destination);
    }

    private void OnDestinationComplete(object sender, System.EventArgs e)
    {
        _view.SetAnimation(SomeUnitView.AnimationState.Idle);
    }
}
