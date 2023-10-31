using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class PauseMenuHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        Button _continueButton, _saveButton, _loadButton, _settingsButton, _quitGameButton;

        Input _input;

        public static VisualElement Root { get { return _root; } }

        void Start() {
            _input = new Input();
            _input.Enable();
        }

        private void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void OnDisable() => UnregisterCallbacks();

        void Update() {
            if (_input.UI.TogglePause.WasPerformedThisFrame()) {
                TogglePause();
            }
        }

        public void Init() {
            _continueButton = _root.Q<Button>("button_continue");
            _saveButton = _root.Q<Button>("button_save");
            _loadButton = _root.Q<Button>("button_load");
            _settingsButton = _root.Q<Button>("button_settings");
            _quitGameButton = _root.Q<Button>("button_quitGame");

            _root.SetActive(false);

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        void TogglePause() {
            Time.timeScale = MenuManager.Instance.paused ? 1f : 0f;
            _root.SetActive(!MenuManager.Instance.paused);
            AudioManager.Instance.Play(MenuManager.Instance.paused ? "" : "ui_pause");
            MenuManager.Instance.paused = !MenuManager.Instance.paused;
        }

        void Save() { }

        void Load() { }

        void RegisterCallbacks() {
            _continueButton.clicked += TogglePause;
            _saveButton.clicked += Save;
            _loadButton.clicked += Load;
            _settingsButton.clicked += delegate { SettingsMenuHandler.Root.SetVisible(true); SettingsMenuHandler.SetSettingsPanel(3); };
            _quitGameButton.clicked += Application.Quit;
        }

        void UnregisterCallbacks() {
            _continueButton.clicked -= TogglePause;
            _saveButton.clicked -= Save;
            _loadButton.clicked -= Load;
            _settingsButton.clicked -= delegate { SettingsMenuHandler.Root.SetVisible(true); SettingsMenuHandler.SetSettingsPanel(3); };
            _quitGameButton.clicked -= Application.Quit;
        }
    }
}