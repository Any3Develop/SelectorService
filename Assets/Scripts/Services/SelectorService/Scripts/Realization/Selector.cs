using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Services.Selector
{
    public class Selector : ISelector
    {
        private readonly ISelectorInput _selectorInput;
        private readonly List<ISelectable> _poolSelectables;
        private readonly List<ISelectable> _selected;

        private Vector3 _startPosition, _currentPosition;
        private ISelectorView _view;

        public event EventHandler OnSelectionStart;
        public event EventHandler OnSelectionEnd;
        public event EventHandler<ISelectable[]> OnSelected;

        [Inject]
        public Selector(ISelectorInput input)
        {
            _selectorInput = input;
            _selectorInput.IsEnabled = true;
            _selected = new List<ISelectable>();
            _poolSelectables = new List<ISelectable>();
        }

        public void Initialize(ISelectorView view)
        {
            _view = view;
            _selectorInput.Begin += Input_Begin;
            _selectorInput.Moved += Input_Moved;
            _selectorInput.Ended += Input_Ended;
        }

        public void UnInitialize()
        {
            _selectorInput.Begin -= Input_Begin;
            _selectorInput.Moved -= Input_Moved;
            _selectorInput.Ended -= Input_Ended;
            ClearSelections();
            _poolSelectables.Clear();
        }

        public ISelectable[] GetSelections()
        {
            if (_selected.Count == 0)
            {
                Debug.LogError(this + " : [GetSelection] Сurrently no selected objects.");
                return null;
            }
            return _selected.ToArray();
        }

        public void SetSelection(ISelectable[] selectable)
        {
            ClearSelections();

            _selected.RemoveAll(x =>
            {
                bool inRange = selectable.Contains(x);
                if (!inRange)
                {
                    DeselectLeft(x);
                }
                return inRange;
            });

            _selected.Clear();

            foreach (var item in selectable)
            {
                item.Select();
                _selected.Add(item);
            }

            OnSelected?.Invoke(this, _selected.ToArray());
        }

        public void UpdateSelectables(IEnumerable units)
        {
            // remove null objects
            _poolSelectables.RemoveAll(x => x.Equals(null));
            foreach (var item in units)
            {
                if (item is ISelectable)
                {
                    var finded = item as ISelectable;
                    if (!_poolSelectables.Contains(finded))
                    {
                        _poolSelectables.Add(finded);
                    }
                }
            }
        }

        public void ClearSelections()
        {
            if (_selected.Count == 0) return;

            foreach (var item in _selected)
            {
                DeselectLeft(item);
            }
            _selected.Clear();
        }

        private void DeselectLeft(ISelectable selectables)
        {
            if (selectables != null)
            {
                selectables.DeSelect();
            }
        }

        private ISelectable[] FindUnderRect()
        {
            List<ISelectable> finded = new List<ISelectable>();
            foreach (var item in _poolSelectables)
            {
                if (IsWithinSelectionBounds(item.GetCenter()))
                {
                    finded.Add(item);
                }
            }
            return finded.ToArray();
        }

        private bool IsWithinSelectionBounds(Vector3 position)
        {
            return GetViewportBounds().Contains(_view.Camera.WorldToViewportPoint(position));
        }

        private Bounds GetViewportBounds()
        {
            if (_view == null)
            {
                Debug.LogError(this + " : ISelectorView is null");
                return default;
            }
            var camera = _view.Camera;
            var v1 = camera.ScreenToViewportPoint(_startPosition);
            var v2 = camera.ScreenToViewportPoint(_currentPosition);
            var min = Vector3.Min(v1, v2);
            var max = Vector3.Max(v1, v2);
            min.z = camera.nearClipPlane;
            max.z = camera.farClipPlane;

            var bounds = new Bounds();
            bounds.SetMinMax(min, max);
            return bounds;
        }

        private void Input_Begin(object sender, EventArgs e)
        {
            _startPosition = _selectorInput.CursorPosition;
            _currentPosition = _selectorInput.CursorPosition;
            _view.Draw(_startPosition, _currentPosition);
            _view.EnableDraw = true;
            OnSelectionStart?.Invoke(this, EventArgs.Empty);
        }
        private void Input_Moved(object sender, EventArgs e)
        {
            _currentPosition = _selectorInput.CursorPosition;
            _view.Draw(_startPosition, _currentPosition);
        }
        private void Input_Ended(object sender, EventArgs e)
        {
            SetSelection(FindUnderRect());
            _currentPosition = _selectorInput.CursorPosition;
            _view.Draw(_startPosition, _currentPosition);
            _view.EnableDraw = false;
            OnSelectionEnd?.Invoke(this, EventArgs.Empty);
        }

    }
}
