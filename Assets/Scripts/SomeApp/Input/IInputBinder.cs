using System;

public interface IInputBinder<TValue>
{
    event EventHandler<TValue> Begin;
    event EventHandler<TValue> Pressed;
    event EventHandler<TValue> Moved;
    event EventHandler<TValue> Ended;

    bool IsEnabled { get; set; }
    void Update();
}
