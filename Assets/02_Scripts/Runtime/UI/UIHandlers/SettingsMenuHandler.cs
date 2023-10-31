using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class SettingsMenuHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        static VisualElement[] _settingsPanels = new VisualElement[4];
        static Button[] _panelSelectors = new Button[4];
        Button _backButton, _restoreAudioButton;
        static Slider _masterSlider, _musicSlider, _sfxSlider, _uiSlider, _voiceSlider;
        static Slider _horSensSlider, _verSensSlider, _mouseHorSensSlider, _mouseVerSensSlider, _rotSensSlider;
        DropdownField _languageSelectionDropdown, _windowModeDropdown, _resolutionDropdown;
        Toggle _vSyncToggle, _cameraAutoFollowToggle, _showRotationGizmosToggle, _showBlockedVolumeToggle, _chromaticAberrationToggle;

        public static VisualElement Root { get { return _root; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void OnDisable() => UnregisterCallbacks();

        public void Init() {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            _settingsPanels = new VisualElement[4];
            _settingsPanels[0] = _root.Q<VisualElement>("panel_videoSettings");
            _settingsPanels[1] = _root.Q<VisualElement>("panel_audioSettings");
            _settingsPanels[2] = _root.Q<VisualElement>("panel_keybindSettings");
            _settingsPanels[3] = _root.Q<VisualElement>("panel_gameplaySettings");

            _panelSelectors = new Button[4];
            _panelSelectors[0] = _root.Q<Button>("button_videoSettings");
            _panelSelectors[1] = _root.Q<Button>("button_audioSettings");
            _panelSelectors[2] = _root.Q<Button>("button_keybindSettings");
            _panelSelectors[3] = _root.Q<Button>("button_gameplaySettings");
            _restoreAudioButton = _root.Q<Button>("button_restoreDefaultAudioSettings");
            _backButton = _root.Q<Button>("button_back");

            _masterSlider = _root.Q<Slider>("slider_masterVolume");
            _musicSlider = _root.Q<Slider>("slider_musicVolume");
            _sfxSlider = _root.Q<Slider>("slider_sfxVolume");
            _uiSlider = _root.Q<Slider>("slider_uiVolume");
            _voiceSlider = _root.Q<Slider>("slider_voiceVolume");

            _horSensSlider = _root.Q<Slider>("slider_horSens");
            _verSensSlider = _root.Q<Slider>("slider_verSens");   
            _mouseHorSensSlider = _root.Q<Slider>("slider_mouseHorSens");   
            _mouseVerSensSlider = _root.Q<Slider>("slider_mouseVerSens");
            _rotSensSlider = _root.Q<Slider>("slider_rotSens");

            _languageSelectionDropdown = _root.Q<DropdownField>("dropdown_languageSelection");
            _languageSelectionDropdown.choices = GameplaySettings.GetAvailableLocales();
            _windowModeDropdown = _root.Q<DropdownField>("dropdown_windowMode");
            _windowModeDropdown.choices = VideoSettings.GetAvailableWindowModes();
            _resolutionDropdown = _root.Q<DropdownField>("dropdown_resolution");
            _resolutionDropdown.choices = VideoSettings.GetAvailableResolutions();

            _vSyncToggle = _root.Q<Toggle>("toggle_enableVSync");
            _cameraAutoFollowToggle = _root.Q<Toggle>("toggle_cameraAutoFollow");
            _cameraAutoFollowToggle.SetActive(sceneIndex == 1);
            _showRotationGizmosToggle = _root.Q<Toggle>("toggle_showRotationGizmos");
            _showRotationGizmosToggle.SetActive(sceneIndex == 1);
            _showBlockedVolumeToggle = _root.Q<Toggle>("toggle_showBlockedVolumeGizmos");
            _showBlockedVolumeToggle.SetActive(sceneIndex == 1);
            _chromaticAberrationToggle = _root.Q<Toggle>("toggle_chromaticAberration");
            _chromaticAberrationToggle.SetActive(sceneIndex == 1);
            
            UnregisterCallbacks();
            RegisterCallbacks();

            LoadAudioSettings();
            if(sceneIndex == 1) {
                LoadGameplaySettings();
                LoadVideoSettings();
            }

            _root.SetVisible(false);
            SetSettingsPanel(3);
        }

        public static void SetSliders() {
            _masterSlider.value = AudioSettings.MasterVolume;
            _musicSlider.value = AudioSettings.MusicVolume;
            _sfxSlider.value = AudioSettings.SFXVolume;
            _uiSlider.value = AudioSettings.UIVolume;
            _voiceSlider.value = AudioSettings.VoiceVolume;

            _horSensSlider.value = GameplaySettings.HorizontalSensitivity;
            _verSensSlider.value = GameplaySettings.VerticalSensitivity;
            _mouseHorSensSlider.value = GameplaySettings.MouseHorizontalSensitivity;
            _mouseVerSensSlider.value = GameplaySettings.MouseVerticalSensitivity;
            _rotSensSlider.value = GameplaySettings.RotationSensitivity;
        }

        void HideSettings() {
            _root.SetVisible(false);
            for (int i = 0; i < _settingsPanels.Length; i++) {
                _settingsPanels[i].SetVisible(false);
            }
        }

         public static void SetSettingsPanel(int index) {
            if (!_root.visible) {
                return;
            }
            for (int i = 0; i < _settingsPanels.Length; i++) {   
                if(i == index) {
                    _settingsPanels[i].SetVisible(true);
                    _panelSelectors[i].AddToClassList("settingsMenu-tabSelector-selected");
                }
                else {
                    _settingsPanels[i].SetVisible(false);
                    _panelSelectors[i].RemoveFromClassList("settingsMenu-tabSelector-selected");
                }
            }
        }

        void SetMasterVolume(ChangeEvent<float> callback) => AudioSettings.MasterVolume = callback.newValue;
        void SetMusicVolume(ChangeEvent<float> callback) => AudioSettings.MusicVolume = callback.newValue;
        void SetSFXVolume(ChangeEvent<float> callback) => AudioSettings.SFXVolume = callback.newValue;
        void SetUIVolume(ChangeEvent<float> callback) => AudioSettings.UIVolume = callback.newValue;
        void SetVoiceVolume(ChangeEvent<float> callback) => AudioSettings.VoiceVolume = callback.newValue;
        void LoadAudioSettings() {
            AudioSettings.LoadAudioSettings();
            _masterSlider.value = AudioSettings.MasterVolume;
            _musicSlider.value = AudioSettings.MusicVolume;
            _sfxSlider.value = AudioSettings.SFXVolume;
            _uiSlider.value = AudioSettings.UIVolume;
            _voiceSlider.value = AudioSettings.VoiceVolume;
        }

        void SelectLanguage(ChangeEvent<string> callback) => GameplaySettings.SelectedLocale = _languageSelectionDropdown.index;
        void SetHorSens(ChangeEvent<float> callback) => GameplaySettings.HorizontalSensitivity = callback.newValue;
        void SetVerSens(ChangeEvent<float> callback) => GameplaySettings.VerticalSensitivity = callback.newValue;
        void SetMouseHorSens(ChangeEvent<float> callback) => GameplaySettings.MouseHorizontalSensitivity = callback.newValue;
        void SetMouseVerSens(ChangeEvent<float> callback) => GameplaySettings.MouseVerticalSensitivity = callback.newValue;
        void SetRotSens(ChangeEvent<float> callback) => GameplaySettings.RotationSensitivity = callback.newValue;
        void SetCameraAutoFollow(ChangeEvent<bool> callback) => GameplaySettings.CameraAutoFollow = callback.newValue;
        void SetShowRotationGizmos(ChangeEvent<bool> callback) => GameplaySettings.ShowRotationGizmos = callback.newValue;
        void SetShowBlockedVolumes(ChangeEvent<bool> callback) => GameplaySettings.ShowBlockedVolumes = callback.newValue;
        void LoadGameplaySettings() {
            GameplaySettings.LoadGameplaySettings();
            _languageSelectionDropdown.index = GameplaySettings.SelectedLocale;
            _horSensSlider.value = GameplaySettings.HorizontalSensitivity;
            _verSensSlider.value = GameplaySettings.VerticalSensitivity;
            _mouseHorSensSlider.value = GameplaySettings.MouseHorizontalSensitivity;
            _mouseVerSensSlider.value = GameplaySettings.MouseVerticalSensitivity;
            _rotSensSlider.value = GameplaySettings.RotationSensitivity;
            _cameraAutoFollowToggle.value = GameplaySettings.CameraAutoFollow;
            _showRotationGizmosToggle.value = GameplaySettings.ShowRotationGizmos;
            _showBlockedVolumeToggle.value = GameplaySettings.ShowBlockedVolumes;
        }

        void SelectWindowMode(ChangeEvent<string> callback) => VideoSettings.CurrentFullscreenMode = (FullScreenMode)_windowModeDropdown.index;
        void SelectResolution(ChangeEvent<string> callback) => VideoSettings.CurrentResolution = Screen.resolutions[_resolutionDropdown.index + VideoSettings.GetDiscardedResolutionsCount()];
        void SetVSyncEnabled(ChangeEvent<bool> callback) => VideoSettings.VSync = callback.newValue;
        void SetChromaticAberration(ChangeEvent<bool> callback) => VideoSettings.ChromaticAberration = callback.newValue;
        void LoadVideoSettings() {
            VideoSettings.LoadVideoSettings();
            _windowModeDropdown.index = (int)VideoSettings.CurrentFullscreenMode;
            _resolutionDropdown.index = VideoSettings.GetResolutionIndex(VideoSettings.CurrentResolution);
            _vSyncToggle.value = VideoSettings.VSync;
            _chromaticAberrationToggle.value = VideoSettings.ChromaticAberration;
        }

        void RegisterCallbacks() {
            _backButton.clicked += HideSettings;
            _panelSelectors[0].clicked += delegate { SetSettingsPanel(0); };
            _panelSelectors[1].clicked += delegate { SetSettingsPanel(1); };
            _panelSelectors[2].clicked += delegate { SetSettingsPanel(2); };
            _panelSelectors[3].clicked += delegate { SetSettingsPanel(3); };
            _restoreAudioButton.clicked += delegate { ModalDisplayHandler.SetModalDisplay("restoreDefaults", AudioSettings.RestoreDefaultAudioSettings); };

            _masterSlider.RegisterValueChangedCallback(SetMasterVolume);
            _musicSlider.RegisterValueChangedCallback(SetMusicVolume);
            _sfxSlider.RegisterValueChangedCallback(SetSFXVolume);
            _uiSlider.RegisterValueChangedCallback(SetUIVolume);
            _voiceSlider.RegisterValueChangedCallback(SetVoiceVolume);

            _horSensSlider.RegisterValueChangedCallback(SetHorSens);
            _verSensSlider.RegisterValueChangedCallback(SetVerSens);
            _mouseHorSensSlider.RegisterValueChangedCallback(SetMouseHorSens);
            _mouseVerSensSlider.RegisterValueChangedCallback(SetMouseVerSens);
            _rotSensSlider.RegisterValueChangedCallback(SetRotSens);

            _languageSelectionDropdown.RegisterValueChangedCallback(SelectLanguage);
            _windowModeDropdown.RegisterValueChangedCallback(SelectWindowMode);
            _resolutionDropdown.RegisterValueChangedCallback(SelectResolution);

            _vSyncToggle.RegisterValueChangedCallback(SetVSyncEnabled);
            _cameraAutoFollowToggle.RegisterValueChangedCallback(SetCameraAutoFollow);
            _showRotationGizmosToggle.RegisterValueChangedCallback(SetShowRotationGizmos);
            _showBlockedVolumeToggle.RegisterValueChangedCallback(SetShowBlockedVolumes);
            _chromaticAberrationToggle.RegisterValueChangedCallback(SetChromaticAberration);
        }

        void UnregisterCallbacks() {
            _backButton.clicked -= HideSettings;
            _panelSelectors[0].clicked -= delegate { SetSettingsPanel(0); };
            _panelSelectors[1].clicked -= delegate { SetSettingsPanel(1); };
            _panelSelectors[2].clicked -= delegate { SetSettingsPanel(2); };
            _panelSelectors[3].clicked -= delegate { SetSettingsPanel(3); };
            _restoreAudioButton.clicked -= delegate { ModalDisplayHandler.SetModalDisplay("restoreDefaults", AudioSettings.RestoreDefaultAudioSettings); };

            _masterSlider.UnregisterValueChangedCallback(SetMasterVolume);
            _musicSlider.UnregisterValueChangedCallback(SetMusicVolume);
            _sfxSlider.UnregisterValueChangedCallback(SetSFXVolume);
            _uiSlider.UnregisterValueChangedCallback(SetUIVolume);
            _voiceSlider.UnregisterValueChangedCallback(SetVoiceVolume);

            _horSensSlider.UnregisterValueChangedCallback(SetHorSens);
            _verSensSlider.UnregisterValueChangedCallback(SetVerSens);
            _mouseHorSensSlider.UnregisterValueChangedCallback(SetMouseHorSens);
            _mouseVerSensSlider.UnregisterValueChangedCallback(SetMouseVerSens);
            _rotSensSlider.UnregisterValueChangedCallback(SetRotSens);

            _languageSelectionDropdown.UnregisterValueChangedCallback(SelectLanguage);
            _windowModeDropdown.UnregisterValueChangedCallback(SelectWindowMode);
            _resolutionDropdown.UnregisterValueChangedCallback(SelectResolution);

            _vSyncToggle.UnregisterValueChangedCallback(SetVSyncEnabled);
            _cameraAutoFollowToggle.UnregisterValueChangedCallback(SetCameraAutoFollow);
            _showRotationGizmosToggle.UnregisterValueChangedCallback(SetShowRotationGizmos);
            _showBlockedVolumeToggle.UnregisterValueChangedCallback(SetShowBlockedVolumes);
            _chromaticAberrationToggle.UnregisterValueChangedCallback(SetChromaticAberration);
        }
    }
}