using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System.Collections.Generic;

namespace SolarAscension {
    public static class GameplaySettings {
        public static int SelectedLocale { get { return PlayerPrefs.GetInt(_selectedLocaleKey, 0); } set { PlayerPrefs.SetInt(_selectedLocaleKey, value); LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value]; } }
        public static float HorizontalSensitivity { get { return PlayerPrefs.GetFloat(_horSensKey, 1f); } set { PlayerPrefs.SetFloat(_horSensKey, value); OnCameraSettingsUpdate?.Invoke(); } }
        public static float VerticalSensitivity { get { return PlayerPrefs.GetFloat(_verSensKey, 1f); } set { PlayerPrefs.SetFloat(_verSensKey, value); OnCameraSettingsUpdate?.Invoke(); } }
        public static float MouseHorizontalSensitivity { get { return PlayerPrefs.GetFloat(_mouseHorSensKey, 1f); } set { PlayerPrefs.SetFloat(_mouseHorSensKey, value); OnCameraSettingsUpdate?.Invoke(); } }
        public static float MouseVerticalSensitivity { get { return PlayerPrefs.GetFloat(_mouseVerSensKey, 1f); } set { PlayerPrefs.SetFloat(_mouseVerSensKey, value); OnCameraSettingsUpdate?.Invoke(); } }
        public static float RotationSensitivity { get { return PlayerPrefs.GetFloat(_rotSensKey, 1f); } set { PlayerPrefs.SetFloat(_rotSensKey, value); OnCameraSettingsUpdate?.Invoke(); } }
        public static bool CameraAutoFollow { get { return PlayerPrefs.GetInt(_cameraAutoFollowKey, 1) == 1; } set { PlayerPrefs.SetFloat(_cameraAutoFollowKey, value ? 1 : 0); PlayerInput.cameraAutoAdjustEnabled = value; } }
        public static bool ShowRotationGizmos { get { return PlayerPrefs.GetInt(_rotGizmoKey, 1) == 1; } set { PlayerPrefs.SetInt(_rotGizmoKey, value ? 1 : 0); MenuManager.Instance.player.State.StateData.showGizmos = value; } }
        public static bool ShowBlockedVolumes { get { return PlayerPrefs.GetInt(_blockedVolumeKey, 1) == 1; } set { PlayerPrefs.SetInt(_blockedVolumeKey, value ? 1 : 0); MenuManager.Instance.player.State.StateData.showBlockedVolume = value; } }

        private const string _selectedLocaleKey = "SelectedLocale", _horSensKey = "horSens", _verSensKey = "verSens", _mouseHorSensKey = "mouseHorSens", _mouseVerSensKey = "mouseVerSens", _rotSensKey = "rotSens", _cameraAutoFollowKey = "cameraAutoFollow", _rotGizmoKey = "showRotationGizmos", _blockedVolumeKey = "showBlockedVolumes";

        public delegate void OnCameraSettingsUpdateHandler();
        public static event OnCameraSettingsUpdateHandler OnCameraSettingsUpdate;

        public static List<string> GetAvailableLocales() {
            List<string> locales = new List<string>();
            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales) {
                locales.Add(locale.LocaleName);
            }
            return locales;
        }

        public static void LoadGameplaySettings() {
            SelectedLocale = SelectedLocale;
            HorizontalSensitivity = HorizontalSensitivity;
            VerticalSensitivity = VerticalSensitivity;
            MouseHorizontalSensitivity = MouseHorizontalSensitivity;
            MouseVerticalSensitivity = MouseVerticalSensitivity;
            RotationSensitivity = RotationSensitivity;
            CameraAutoFollow = CameraAutoFollow;
            ShowRotationGizmos = ShowRotationGizmos;
            ShowBlockedVolumes = ShowBlockedVolumes;
        }
    }
}
