using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class CameraMenuHandler : MonoBehaviour{
        [SerializeField] private OrbitalCamera _camera;

        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        Button _cameraResetButton, _cameraFocusButton;

        public static VisualElement Root { get { return _root; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("camera_settings");

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void OnDisable() => UnregisterCallbacks();

        public void Init() {
            _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("camera_settings");

            _cameraResetButton = _root.Q<Button>("button_cameraReset");
            _cameraFocusButton = _root.Q<Button>("button_cameraFocus");

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        void RegisterCallbacks() {
            _cameraResetButton.clicked += _camera.ResetCamera;
            _cameraFocusButton.clicked += _camera.SetFocusPointSelected;
        }

        void UnregisterCallbacks() {
            _cameraResetButton.clicked -= _camera.ResetCamera;
            _cameraFocusButton.clicked -= _camera.SetFocusPointSelected;
        }
    }
}