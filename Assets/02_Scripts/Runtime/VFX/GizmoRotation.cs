using UnityEngine;

public class GizmoRotation : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform root;
    [SerializeField] private Transform keyTransformR;
    [SerializeField] private Transform keyTransformF;
    [SerializeField] private Transform keyTransformG;

    public void LateUpdate() {
        Quaternion parentRotation = root.localRotation;
        transform.localRotation = Quaternion.Inverse(parentRotation);
        keyTransformF.localRotation = keyTransformR.localRotation = keyTransformG.localRotation = parentRotation;
    }
}
