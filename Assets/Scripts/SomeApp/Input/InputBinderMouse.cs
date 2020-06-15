using System;
using UnityEngine;

public class InputBinderMouse : IInputBinder<Vector3>
{
    public bool IsEnabled { get; set; } = false;
    public event EventHandler<Vector3> Begin;
    public event EventHandler<Vector3> Pressed;
    public event EventHandler<Vector3> Moved;
    public event EventHandler<Vector3> Ended;
    private Vector3 _lastPosition = Vector3.zero;
    private readonly KeyCode _key;

    public InputBinderMouse(KeyCode key)
    {
        _key = key;
    }

    public void Update()
    {
        if (!IsEnabled) return;
        if (Input.GetKeyDown(_key))
        {
            Begin?.Invoke(this, _lastPosition = Input.mousePosition);
        }
        else if (Input.GetKey(_key))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            if (_lastPosition != currentMousePosition)
            {
                Moved?.Invoke(this, currentMousePosition);
                _lastPosition = currentMousePosition;
            }

            Pressed?.Invoke(this, currentMousePosition);
        }
        else if (Input.GetKeyUp(_key))
        {
            Ended?.Invoke(this, Input.mousePosition);
            _lastPosition = Vector3.zero;
        }
    }
}
