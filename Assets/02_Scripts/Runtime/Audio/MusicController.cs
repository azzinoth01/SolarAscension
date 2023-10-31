using UnityEngine;
using System.Collections.Generic;

namespace SolarAscension {
    public class MusicController : MonoBehaviour {
        [Range(0f, 1f)]
        [SerializeField] private float musicVolume;
        [Space]
        [SerializeField] private List<AudioSourceController> ingameSources = new List<AudioSourceController>();
        [SerializeField] private AudioSource menuSource;

        private void Start() {
            menuSource.Play();
        }

        public void PlayIngameMusic() {
            menuSource.Stop();
            foreach(AudioSourceController sourceController in ingameSources) {
                sourceController.Play();
            }
        }
    }
}