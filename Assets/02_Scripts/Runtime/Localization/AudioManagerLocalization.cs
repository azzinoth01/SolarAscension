using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace SolarAscension {
    public class AudioManagerLocalization : MonoBehaviour {
        [SerializeField] private LocalizedAssetTable _assetTable = null;

        private void OnEnable() => _assetTable.TableChanged += Localize;
        private void OnDisable() => _assetTable.TableChanged -= Localize;

        private void Localize(AssetTable table) {
            foreach (LocalizedSound s in AudioManager.Instance._localizedSounds) {
                bool wasPlaying = s.source.isPlaying;
                s.source.clip = s.clip.LoadAsset();
                if (wasPlaying) {
                    s.source.Play();
                }
            }
        }
    }

}
