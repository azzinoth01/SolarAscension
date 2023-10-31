using UnityEngine;

namespace SolarAscension {
    public static class AudioSettings {
        public static float MasterVolume { get { return PlayerPrefs.GetFloat(_masterKey, _defaultVolume); } set { PlayerPrefs.SetFloat(_masterKey, value); AudioManager.Instance.masterMixer.SetFloat(_masterKey, Mathf.Log10(value) * 20); } }
        public static float MusicVolume { get { return PlayerPrefs.GetFloat(_musicKey, _defaultVolume); } set { PlayerPrefs.SetFloat(_musicKey, value); AudioManager.Instance.musicMixer.SetFloat(_musicKey, Mathf.Log10(value) * 20); } }
        public static float SFXVolume { get { return PlayerPrefs.GetFloat(_sfxKey, _defaultVolume); } set { PlayerPrefs.SetFloat(_sfxKey, value); AudioManager.Instance.sfxMixer.SetFloat(_sfxKey, Mathf.Log10(value) * 20); } }
        public static float UIVolume { get { return PlayerPrefs.GetFloat(_uiKey, _defaultVolume); } set { PlayerPrefs.SetFloat(_uiKey, value); AudioManager.Instance.uiMixer.SetFloat(_uiKey, Mathf.Log10(value) * 20); } }
        public static float VoiceVolume { get { return PlayerPrefs.GetFloat(_voiceKey, _defaultVolume); } set { PlayerPrefs.SetFloat(_voiceKey, value); AudioManager.Instance.voiceMixer.SetFloat(_voiceKey, Mathf.Log10(value) * 20); } }

        private const string _masterKey = "MasterVolume", _musicKey = "MusicVolume", _sfxKey = "SFXVolume", _uiKey = "UIVolume", _voiceKey = "VoiceVolume";

        private static float _defaultVolume = 0.3f;

        public static void RestoreDefaultAudioSettings() {
            MasterVolume = _defaultVolume;
            MusicVolume = _defaultVolume;
            SFXVolume = _defaultVolume;
            UIVolume = _defaultVolume;
            VoiceVolume = _defaultVolume;
            SettingsMenuHandler.SetSliders();
        }

        public static void LoadAudioSettings() {
            MasterVolume = MasterVolume;
            MusicVolume = MusicVolume;
            SFXVolume= SFXVolume;
            UIVolume = UIVolume;    
            VoiceVolume = VoiceVolume;
        }
    }
}