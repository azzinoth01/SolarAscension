using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class MainMenuHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        VisualElement _creditsPanel;
        Button _newGameButton, _settingsButton, _creditsButton, _quitGameButton, _creditsBackButton;

        public static VisualElement Root {
            get {
                return _root;
            }
        }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void OnDisable() => UnregisterCallbacks();

        public void Init() {
            _creditsPanel = _root.Q<VisualElement>("credits_panel");

            _newGameButton = _root.Q<Button>("button_newGame");
            _settingsButton = _root.Q<Button>("button_settings");
            _creditsButton = _root.Q<Button>("button_credits");
            _quitGameButton = _root.Q<Button>("button_quitGame");
            _creditsBackButton = _root.Q<Button>("creditsBack_button");

            _creditsPanel.SetActive(false);

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        void NewGame() {
            AudioManager.Instance.MusicController.PlayIngameMusic();
            SceneLoader.Instance.LoadScene(1);
        }

        void OpenCredits() => _creditsPanel.SetActive(true);
        void CloseCredits() => _creditsPanel.SetActive(false);

        void RegisterCallbacks() {
            _newGameButton.clicked += NewGame;
            _settingsButton.clicked += delegate { SettingsMenuHandler.Root.SetVisible(true); SettingsMenuHandler.SetSettingsPanel(3); };
            _creditsButton.clicked += OpenCredits;
            _quitGameButton.clicked += Application.Quit;
            _creditsBackButton.clicked += CloseCredits;
        }

        void UnregisterCallbacks() {
            _newGameButton.clicked -= NewGame;
            _settingsButton.clicked -= delegate { SettingsMenuHandler.Root.SetVisible(true); SettingsMenuHandler.SetSettingsPanel(3); };
            _creditsButton.clicked -= OpenCredits;
            _quitGameButton.clicked -= Application.Quit;
            _creditsBackButton.clicked -= CloseCredits;
        }
    }
}
