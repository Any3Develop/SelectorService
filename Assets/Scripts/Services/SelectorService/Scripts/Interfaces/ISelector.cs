using System;
using System.Collections;

namespace Services.Selector
{
    public interface ISelector
    {
        event EventHandler OnSelectionStart;
        event EventHandler OnSelectionEnd;
        event EventHandler<ISelectable[]> OnSelected;

        ISelectable[] GetSelections();
        void ClearSelections();
        void Initialize(ISelectorView view);
        void SetSelection(ISelectable[] selectable);
        void UnInitialize();
        void UpdateSelectables(IEnumerable units);
    }
}