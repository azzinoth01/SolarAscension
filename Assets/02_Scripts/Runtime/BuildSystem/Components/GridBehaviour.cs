using UnityEngine;

public class GridBehaviour : MonoBehaviour {
    [SerializeField] protected GridMeshObject meshObject;
    [SerializeField] protected BoxCollider boxCollider;
    [SerializeField] private DroneModul _droneModul;
    public GridObject GridObject {
        get; set;
    }

    private bool _moduleIsSleeping;
    private static MaterialPropertyBlock _mpb;

    private void Awake() {
        _mpb ??= new MaterialPropertyBlock();
    }

    private static readonly int ActiveProperty = Shader.PropertyToID("_Active");

    public Transform RenderTransform => meshObject.transform;

    public void ConfigureObject() {
        if (GridObject != null) {
            BuildData buildData = GridObject.moduleData.buildData;

            meshObject.ConfigureMeshObject(buildData);

            Vector3 size = (Vector3)buildData.gridSize * BuildSystem.VolumeScale;
            Vector3 center = size / 2f - Vector3.one * BuildSystem.HalfScale;
            boxCollider.center = center;
            boxCollider.size = size + Vector3.one * BuildSystem.EighthScale;

            transform.position = GridObject.origin.ToWorldPositionCentered();
            if (GridObject.objectType != GridObjectType.Tube) {
                transform.localRotation = GridObject.orientation;
            }
        }
        else {
            meshObject.ResetMeshObject();

            boxCollider.center = Vector3.one * BuildSystem.HalfScale;
            boxCollider.size = Vector3.one * BuildSystem.VolumeScale;

            transform.localRotation = Quaternion.identity;
        }


        if (GridObject.moduleData.objectType == GridObjectType.ScrapGatherer) {
            _droneModul.SetupModul(GridObject.moduleData, transform, GridObject, TargetType.scrapField);
            _droneModul.CalcPath();
        }
        else if (GridObject.moduleData.objectType == GridObjectType.IceMiner) {
            _droneModul.SetupModul(GridObject.moduleData, transform, GridObject, TargetType.iceField);
            _droneModul.CalcPath();
        }
    }

    public void HandleRelocation() {
        _droneModul.RelocateModul(GridObject);
    }

    private void Update() {
        if ( GridObject.economyBuilding.IsActive && _moduleIsSleeping ) {
            _mpb.SetFloat(ActiveProperty, 1f);
            _moduleIsSleeping = false;
            meshObject.SetPropertyBlock(_mpb);
        } else if ( !GridObject.economyBuilding.IsActive && !_moduleIsSleeping ) {
            _mpb.SetFloat(ActiveProperty, 0.1f);
            _moduleIsSleeping = true;
            meshObject.SetPropertyBlock(_mpb);
        }

        GridObject.SubmitRenderingCommands();
    }

    public void UpdateTubeMeshAndRotation(LinkDirection linkDirection) {
        meshObject.UpdateTubeMeshAndRotation(linkDirection);
    }

    public void HideMesh() {
        meshObject.HideMesh();
        boxCollider.enabled = false;
    }

    public void ShowMesh() {
        meshObject.ShowMesh();
        boxCollider.enabled = true;
    }

    public Matrix4x4 GetMeshLocalToWorld() {
        return meshObject.GetLocalToWorld();
    }

    public GridCoordinate GetLinkCenter() {
        return GridObject.origin + GridObject.moduleData.buildData.linkCenter.Rotate(GridObject.orientation);
    }

    public Matrix4x4 GetBlockedOverlayMatrix() {
        GridCoordinate gridSize = GridObject.moduleData.buildData.gridSize;
        GridCoordinate blockedSize = gridSize + new GridCoordinate(2);
        Vector3 centerPosition = meshObject.transform.position;

        return Matrix4x4.TRS(centerPosition, GridObject.orientation, blockedSize.ToWorldPositionCentered());
    }


    private void OnDestroy() {
        if (_droneModul != null) {
            _droneModul.RemoveModul();
        }

    }
}