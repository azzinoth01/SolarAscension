using UnityEngine;

[CreateAssetMenu(menuName = "Nebra/BuildData", order = 0)]
public class BuildData : ScriptableObject {
    [Header("Grid Configuration")]
    public GridCoordinate gridSize;
    public GridCoordinate linkCenter;
    public GridLink[] links;

    [Header("Attachment Configuration")]
    public AttachmentConfig[] attachmentConfigs;
    public AttachmentData[] attachmentData;

    [Header("Mesh Configuration")]
    public Mesh mesh;
    public Material[] materials;
    public Material[] previewMaterials;

    [Header("Audio Configuration")]
    public string buildSfxName;
    public string selectSfxName;

    public Vector3 GetCenterPoint(Quaternion rotation) {
        return rotation * (gridSize - Vector3.one) * BuildSystem.HalfScale;
    }
        
    #if UNITY_EDITOR
    public void SetAttachmentTransforms(AttachmentTransform[] transforms, int index) {
        if ( Application.isPlaying ) {
            return;
        }

        if ( attachmentConfigs.Length <= index ) {
            attachmentConfigs = new AttachmentConfig[index + 1];
        }

        attachmentConfigs[index] = new AttachmentConfig {
            attachmentTransforms = transforms
        };
    }
    #endif
}