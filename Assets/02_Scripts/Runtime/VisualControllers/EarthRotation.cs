using UnityEngine;

public class EarthRotation : MonoBehaviour {
    [SerializeField] private Transform surfaceTransform;

    [SerializeField] private float earthSpeed;

    private void FixedUpdate() {
        surfaceTransform.localRotation *= Quaternion.Euler(0f, earthSpeed * Time.deltaTime, 0f);
    }
}
