using System;
using System.Collections;
using UnityEngine;

namespace SolarAscension {
    [Serializable]
    public class AudioSourceController {
        [SerializeField] private AudioSource source;
        [Space, Tooltip("Duration of Fade In/Out in seconds")]
        [SerializeField] private float fadeDuration = 5;
        [Tooltip("Min and Max time between Fades in minutes")]
        [SerializeField] private float minPlayTimeBeforeFade, maxPlayTimeBeforeFade;
        [Tooltip("Min and Max time the Layer remains Faded out in minutes")]
        [SerializeField] private float minFadedOutDuration, maxFadedOutDuration;

        [Space, SerializeField] private bool fade = true;

        float _initialVolume;

        public void Play() {
            _initialVolume = source.volume;
            source.Play();
            if (fade) {
                AudioManager.Instance.StartCoroutine(FadeIn());
            }
        }

        float SetTimeUntilFadeOut() => UnityEngine.Random.Range(minPlayTimeBeforeFade * 60, maxPlayTimeBeforeFade * 60);
        float SetTimeUntilFadeIn() => UnityEngine.Random.Range(minFadedOutDuration * 60, maxFadedOutDuration * 60);

        IEnumerator FadeIn() {
            float currentTime = 0;
            while (currentTime < fadeDuration) {
                source.volume = Mathf.Lerp(0, _initialVolume, (currentTime / fadeDuration));
                currentTime += Time.deltaTime;
                yield return null;
            }
            source.volume = _initialVolume;

            AudioManager.Instance.StartCoroutine(AwaitNextFade(SetTimeUntilFadeOut(), FadeOut()));
        }

        IEnumerator FadeOut() {
            float currentTime = 0;
            while (currentTime < fadeDuration) {
                source.volume = Mathf.Lerp(_initialVolume, 0, (currentTime / fadeDuration));
                currentTime += Time.deltaTime;
                yield return null;
            }
            source.volume = 0;

            AudioManager.Instance.StartCoroutine(AwaitNextFade(SetTimeUntilFadeIn(), FadeIn()));
        }

        IEnumerator AwaitNextFade(float waitTime, IEnumerator nextFade) {
            yield return new WaitForSecondsRealtime(waitTime);
            AudioManager.Instance.StartCoroutine(nextFade);
        }
    }
}