using Services.Selector;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class SelectorTest : MonoBehaviour
{
    [Inject] private readonly ISelector _selector = null;
    private List<IControllable> _unitsControllable = new List<IControllable>();
    private IInputBinder<Vector3> _inputControllable = new InputBinderMouse(KeyCode.Mouse1);
    private ISelectorView _view;
    [SerializeField] SomeUnitView _someUnitPrefab;
    [SerializeField] Transform _someUnitsContainer;
    [SerializeField] int _unitsCount = 5;
    private SomeUnitPresenter[] someUnits;

    private void Start()
    {
        CreateUnits();
        _view = Camera.main.GetComponent<ISelectorView>();
        _selector.Initialize(_view);
        _selector.UpdateSelectables(BaseUnit.FirstCreated);
        _selector.OnSelected += OnSelectorSelected;
        _inputControllable.Begin += Input_Begin;
        _inputControllable.IsEnabled = true;
    }

    private void CreateUnits()
    {
        if (_unitsCount < 1)
        {
            _unitsCount = 1;
        }
        else if (_unitsCount > 10)
        {
            _unitsCount = 10;
        }
        someUnits = new SomeUnitPresenter[_unitsCount];

        for (int i = 0; i < _unitsCount; i++)
        {
            someUnits[i] = new SomeUnitPresenter();
            SomeUnitView view = Instantiate(_someUnitPrefab, _someUnitsContainer);
            view.transform.position -= Vector3.right * i * 3;

            someUnits[i].Initialize(view);
        }
    }

    private void Update()
    {
        _inputControllable.Update();
    }

    private void Input_Begin(object sender, Vector3 position)
    {
        if (_unitsControllable.Count > 0)
        {
            Ray ray =_view.Camera.ScreenPointToRay(position);
            Vector3 destenation;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                destenation = hit.point;
            }
            else
            {
                return;
            }

            foreach (var item in _unitsControllable)
            {
                item.SetDestination(destenation);
            }
        }
        
    }

    private void OnSelectorSelected(object sender, ISelectable[] selection)
    {
        _unitsControllable.Clear();
        foreach (var item in selection)
        {
            if(item is IControllable)
            {
                _unitsControllable.Add(item as IControllable);
            }
        }
    } 
}
