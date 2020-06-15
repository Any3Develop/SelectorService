using System;
using UnityEngine;

public class InputBinderKeyBoard : IInputBinder<EventArgs>
{
    public bool IsEnabled { get; set; } = false;
    public event EventHandler<EventArgs> Begin;
    public event EventHandler<EventArgs> Pressed;
    public event EventHandler<EventArgs> Moved;
    public event EventHandler<EventArgs> Ended;
    private readonly KeyCode _key;

    public InputBinderKeyBoard(KeyCode bind)
    {
        _key = bind;
    }

    public void Update()
    {
        if (!IsEnabled) return;

        if (Input.GetKeyDown(_key))
        {
            Begin?.Invoke(this, EventArgs.Empty);
        }
        if (Input.GetKeyUp(_key))
        {
            Ended?.Invoke(this, EventArgs.Empty);
        }
        else if(Input.GetKey(_key))
        { 
            Pressed?.Invoke(this, EventArgs.Empty);
        }      
    }
}
