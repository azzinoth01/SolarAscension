using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SolarAscension {
    [Serializable]
    public class Sound {
        public string key;
        [Space]
        public AudioClip clip;
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
