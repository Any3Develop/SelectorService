using System;
using UnityEngine;

namespace Services.Selector
{
    public interface ISelectorInput
    {
        event EventHandler Begin;
        event EventHandler Moved;
        event EventHandler Ended;

        Vector3 CursorPosition { get; }
        bool IsEnabled { get; set; }
    }
}
