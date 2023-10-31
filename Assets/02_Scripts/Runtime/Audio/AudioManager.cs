using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SolarAscension {
    public class AudioManager : MonoBehaviour {
        public AudioMixer masterMixer, musicMixer, sfxMixer, uiMixer, voiceMixer;
        [Space]
        [SerializeField] private Sound[] _sounds;
        public LocalizedSound[] _localizedSounds;

        private Dictionary<string, AudioSource> _sources = new Dictionary<string, AudioSource>();
        
        private MusicController _musicController;
        public MusicController MusicController { get { return _musicController; } }

        private static AudioManager _instance;
        public static AudioManager Instance { get { return _instance; } }
        
        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
                DontDestroyOnLoad(this);
            }

            _musicController = GetComponent<MusicController>();

            foreach (Sound s in _sounds) {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.looping;
                s.source.outputAudioMixerGroup = s.mixer;
                _sources.Add(s.key, s.source);
            }

            foreach (LocalizedSound s in _localizedSounds) {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip.LoadAsset();
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.looping;
                s.source.outputAudioMixerGroup = s.mixer;
                _sources.Add(s.key, s.source);
            }

            AudioSettings.LoadAudioSettings();
        }

        public void Play(string key) {
            if (_sources.ContainsKey(key)) { 
                _sources[key].Play();
            }
        }

        public void Stop(string key) {
            if (_sources.ContainsKey(key)) {
                _sources[key].Stop();
            }
        }

        public void Pause(string key) {
            if (_sources.ContainsKey(key)) {
                _sources[key].Pause();
            }
        }

        public void StopAll() {
            foreach (KeyValuePair<string, AudioSource> source in _sources) {
                source.Value.Stop();
            }
        }
    }
}