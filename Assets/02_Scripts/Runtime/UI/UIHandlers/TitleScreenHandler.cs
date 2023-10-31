using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class TitleScreenHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        Button _startButton;

        public static VisualElement Root { get { return _root; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void OnDisable() => UnregisterCallbacks();

        void Init() {
            _startButton = _root.Q<Button>("button_startGame");

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        void RegisterCallbacks() {
            _startButton.clicked += delegate { _root.SetActive(false); };
        }

        void UnregisterCallbacks() {
            _startButton.clicked -= delegate { _root.SetActive(false); };
        }
    }
}