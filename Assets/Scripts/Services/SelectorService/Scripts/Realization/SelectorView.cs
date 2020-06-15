using UnityEngine;
namespace Services.Selector
{
    public class SelectorView : MonoBehaviour, ISelectorView
    {
        public Camera Camera => _camera;

        [SerializeField] Camera _camera = null;
        [SerializeField] Color _insideColor = new Color(0.8f, 0.8f, 0.95f, 0.25f);
        [SerializeField] Color _borderColor = new Color(0.8f, 0.8f, 0.95f);
        [SerializeField] float _borderThickness = 2f;

        public bool EnableDraw { get; set; }
        private Vector3 _screenPosition1;
        private Vector3 _screenPosition2;
        private Texture2D _whiteTexture;

        public void Draw(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            _screenPosition1 = screenPosition1;
            _screenPosition2 = screenPosition2;
        }

        private void Start()
        {
            if (!_camera)
            {
                Debug.LogError(this + " : camera is null");
                return;
            }
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }
        }

        private void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, _whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawScreenRectBorder(Rect rect, Color color)
        {
            // верх
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, _borderThickness), color);
            // лево
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, _borderThickness, rect.height), color);
            // право
            DrawScreenRect(new Rect(rect.xMax - _borderThickness, rect.yMin, _borderThickness, rect.height), color);
            // низ
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - _borderThickness, rect.width, _borderThickness), color);
        }

        private Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            // Перемещение координат из левого нижнего в левый верхний угол
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;
            // Рассчитать углы
            var topLeft = Vector3.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
            // Создать прямоугольник
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }

        private void OnGUI()
        {
            if (EnableDraw)
            {
                var rect = GetScreenRect(_screenPosition1, _screenPosition2);
                DrawScreenRect(rect, _insideColor);
                DrawScreenRectBorder(rect, _borderColor);
            }
        }
    }
}
