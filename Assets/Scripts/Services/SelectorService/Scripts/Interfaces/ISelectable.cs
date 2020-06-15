using UnityEngine;
namespace Services.Selector
{
    public interface ISelectable
    {
        void Select();
        void DeSelect();
        Vector3 GetCenter();
    }
}