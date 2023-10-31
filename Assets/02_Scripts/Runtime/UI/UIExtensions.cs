using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public static class UIExtensions {
        public static void SetActive(this VisualElement element, bool active) => element.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        public static void SetVisible(this VisualElement element, bool visible) => element.visible = visible;
        public static void SetBackgroundSprite(this VisualElement element, Sprite sprite) => element.style.backgroundImage = new StyleBackground(sprite);
        public static void SetHeight(this VisualElement element, int height, LengthType type) => element.style.height = type == LengthType.Percent ? Length.Percent(height) : height;
        public static void SetMinHeight(this VisualElement element, int height, LengthType type) => element.style.minHeight = type == LengthType.Percent ? Length.Percent(height) : height;
        public static void SetMaxHeight(this VisualElement element, int height, LengthType type) => element.style.maxHeight = type == LengthType.Percent ? Length.Percent(height) : height;
        public static void SetMinAndMaxHeight(this VisualElement element, int height, LengthType type) {
            element.style.minHeight = type == LengthType.Percent ? Length.Percent(height) : height;
            element.style.maxHeight = type == LengthType.Percent ? Length.Percent(height) : height;
        }
        public static void SetWidth(this VisualElement element, int width, LengthType type) => element.style.width = type == LengthType.Percent ? Length.Percent(width) : width;
        public static void SetPosition(this VisualElement element, Position position, int amount, LengthType type) {
            switch (position) {
                case Position.Left:
                    element.style.left = type == LengthType.Percent ? Length.Percent(amount) : amount;
                    break;
                case Position.Top:
                    element.style.top = type == LengthType.Percent ? Length.Percent(amount) : amount;
                    break;
                case Position.Right:
                    element.style.right = type == LengthType.Percent ? Length.Percent(amount) : amount;
                    break;
                case Position.Bottom:
                    element.style.bottom = type == LengthType.Percent ? Length.Percent(amount) : amount;
                    break;
                default:
                    break;
            }
        }
        public static void SetPosition(this VisualElement element, Vector2 position, LengthType type) {
            element.style.left = type == LengthType.Percent ? Length.Percent(position.x) : position.x;
            element.style.top = type == LengthType.Percent ? Length.Percent(Screen.height - position.y) : Screen.height - position.y;
        }
        public static bool IsActive(this VisualElement element) => element.style.display == DisplayStyle.Flex;
        public static void ToggleActive(this VisualElement element) => element.style.display = element.style.display == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex;
    }

    public enum LengthType {
        Percent,
        Pixel,
    }

    public enum Position {
        Left,
        Top,
        Right,
        Bottom,
    }
}