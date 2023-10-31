using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PostProcessController : MonoBehaviour {
	private Volume _volume;

	private void Start() {
		_volume = GetComponent<Volume>();
	}

	public void SetChromaticAberration(bool state) {
		VolumeProfile profile = _volume.sharedProfile;
		if (profile.TryGet<ChromaticAberration>(out var chromaticAberration)) {
			chromaticAberration.active = state;
		}
	}
}
