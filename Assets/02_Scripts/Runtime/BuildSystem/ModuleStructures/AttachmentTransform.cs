using UnityEngine;

[System.Serializable]
public struct AttachmentTransform {
	public Vector3 position;
	public Vector3 rotation;

	public void ApplyTransform(Transform transform) {
		transform.localPosition = position;
		transform.localRotation = Quaternion.Euler(rotation);
	}
}