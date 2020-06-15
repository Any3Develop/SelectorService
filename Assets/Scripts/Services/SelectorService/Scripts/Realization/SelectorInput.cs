using System;
using UnityEngine;
namespace Services.Selector
{
    public class SelectorInput : MonoBehaviour, ISelectorInput
    {
        [SerializeField] float _delayBeforInput = 0.05f;
        public Vector3 CursorPosition { get; private set; }

        public bool IsEnabled { get; set; }

        public event EventHandler Begin;
        public event EventHandler Moved;
        public event EventHandler Ended;

        private bool _begin;
        private float _startInput;
        private KeyCode _key = KeyCode.Mouse0;

        public void Update()
        {
            if (!IsEnabled) return;

            // отложеный Begin
            if (Input.GetKeyDown(_key))
            {
                _startInput = Time.time + _delayBeforInput;
                _begin = true;
            }


            if (Time.time > _startInput)
            {
                if (_begin)
                {
                    _begin = false;
                    CursorPosition = Input.mousePosition;
                    Begin?.Invoke(this, EventArgs.Empty);
                }
                else if (Input.GetKey(_key))
                {
                    Vector3 currentCursorPosition = Input.mousePosition;
                    if (CursorPosition != currentCursorPosition)
                    {
                        CursorPosition = currentCursorPosition;
                        Moved?.Invoke(this, EventArgs.Empty);
                    }
                }
                else if (Input.GetKeyUp(_key))
                {
                    CursorPosition = Input.mousePosition;
                    Ended?.Invoke(this, EventArgs.Empty);
                    CursorPosition = Vector3.zero;
                }
            }
        }
    }
}
