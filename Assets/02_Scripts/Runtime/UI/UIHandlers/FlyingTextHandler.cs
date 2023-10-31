using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class FlyingTextHandler : MonoBehaviour {
        [SerializeField] Camera orbitalCamera;

        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        VisualElement _icon;
        Label _label;

        public static VisualElement Root { get { return _root; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("flyingText");

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void Init() {
            _root = _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("flyingText");

            _icon = _root.Q<VisualElement>("flyingText_icon");
            _label = _root.Q<Label>("flyingText_label");

            _root.SetActive(false);
        }

        public void ActivateFlyingText(Vector3 targetWorldPosition, string message, Sprite icon = null, float duration = 2f) {
            _root.SetActive(true);
            _icon.SetActive(icon != null);
            _icon.SetBackgroundSprite(icon);

            _label.text = message;

            StartCoroutine(UpdateFlyingText(targetWorldPosition, duration));
        }

        void SetFlyingTextPosition(Vector3 targetWorldPosition) {
            Vector2 screenPosition = orbitalCamera.WorldToScreenPoint(targetWorldPosition);
            int left = Mathf.RoundToInt(screenPosition.x / Screen.width * 100f);
            int top = Mathf.RoundToInt(screenPosition.y / Screen.height * 100f);
            _root.SetPosition(Position.Left, left, LengthType.Percent);
            _root.SetPosition(Position.Top, top, LengthType.Percent);
        }

        IEnumerator UpdateFlyingText(Vector3 targetWorldPosition, float duration) {
            float currentTime = 0f;
            while (currentTime < duration) {
                SetFlyingTextPosition(targetWorldPosition);
                currentTime += Time.deltaTime;
                yield return null;
            }
            _root.SetActive(false);
        }
    }
}