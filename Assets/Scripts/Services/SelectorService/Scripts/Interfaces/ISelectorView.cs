using UnityEngine;

namespace Services.Selector
{
    public interface ISelectorView
    {
        Camera Camera { get; }
        bool EnableDraw { get; set; }
        void Draw(Vector3 screenPosition1, Vector3 screenPosition2);
    }
}