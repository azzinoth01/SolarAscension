using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;

namespace SolarAscension {
    [Serializable]
    public class LocalizedSound {
        public string key;
        [Space]
        public LocalizedAudioClip clip;
        public AudioMixerGroup mixer;
        [Space, Range(0f, 1f)]
        public float volume;
        public float pitch;
        [Space]
        public bool looping;

        [HideInInspector]
        public AudioSource source;
    }
}

