using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AttachmentBehaviour : MonoBehaviour {
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;

    public Building associatedBuilding;
    
    private void Awake() {
        meshFilter ??= GetComponent<MeshFilter>();
        meshRenderer ??= GetComponent<MeshRenderer>();
    }

    public void Setup(AttachmentData attachmentData, GridObject gridObject, Building building, int configIndex, int slotId) {
        meshFilter.sharedMesh = attachmentData.mesh;
        meshRenderer.sharedMaterials = attachmentData.materials;

        associatedBuilding = building;
        
        Transform t = transform;
        BuildData buildData = gridObject.moduleData.buildData;
        AttachmentTransform attachmentTransform = buildData.attachmentConfigs[configIndex].attachmentTransforms[slotId];
        
        t.SetParent(gridObject.gridBehaviour.transform);
        attachmentTransform.ApplyTransform(t);
    }

    public void TearDown() {
        meshFilter.sharedMesh = null;
        transform.parent = null;

        associatedBuilding.BuildingRemoved();
    }
}
