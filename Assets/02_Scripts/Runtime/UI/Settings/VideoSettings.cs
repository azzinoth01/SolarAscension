using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SolarAscension {
    public static class VideoSettings {
        public static FullScreenMode CurrentFullscreenMode { get { return (FullScreenMode)PlayerPrefs.GetInt(_fullscreenModeKey, 0); } set { PlayerPrefs.SetInt(_fullscreenModeKey, (int)value); Screen.fullScreenMode = value; } }
        public static Resolution CurrentResolution { get { return Screen.resolutions[PlayerPrefs.GetInt(_resolutionKey, Screen.resolutions.Length - 1)]; } set { PlayerPrefs.SetInt(_resolutionKey, GetResolutionIndex(value)); Screen.SetResolution(value.width, value.height, CurrentFullscreenMode); } }
        public static bool VSync { get { return PlayerPrefs.GetInt(_vSyncKey, 1) == 1; } set { PlayerPrefs.SetInt(_vSyncKey, value ? 1 : 0); QualitySettings.vSyncCount = value ? 1 : 0; } }
        public static bool ChromaticAberration { get { return PlayerPrefs.GetInt(_chromaticAberrationKey, 1) == 1; } set { PlayerPrefs.SetInt(_chromaticAberrationKey, value ? 1 : 0); MenuManager.Instance.postProcessController.SetChromaticAberration(value); } }

        private const string _vSyncKey = "VSync", _resolutionKey = "Resolution", _fullscreenModeKey = "FullscreenMode", _chromaticAberrationKey = "chromaticAberration";

        public static List<string> GetAvailableResolutions() {
            List<string> resolutions = new List<string>();
            foreach (Resolution resolution in Screen.resolutions) {
                if (resolution.width >= 1440) {
                    resolutions.Add($"{resolution.width} x {resolution.height}");
                }
            }
            return resolutions;
        }

        public static int GetDiscardedResolutionsCount() {
            for (int i = 0; i < Screen.resolutions.Length; i++) {
                if (Screen.resolutions[i].width >= 1440) {
                    return i - 1;
                }
            }
            return 0;
        }

        public static List<string> GetAvailableWindowModes() => new List<string>() { "Fullscreen", "Windowed Borderless", "Windowed Maximized", "Windowed" };

        public static int GetResolutionIndex(Resolution resolution) {
            for (int i = 0; i < Screen.resolutions.Length; i++) {
                if (resolution.width == Screen.resolutions[i].width) {
                    return i;
                }
            }
            return -1;
        }

        public static void LoadVideoSettings() {
            CurrentFullscreenMode = CurrentFullscreenMode;
            CurrentResolution = CurrentResolution;
            VSync = VSync;
            ChromaticAberration = ChromaticAberration;
        }
    }
}

