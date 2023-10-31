using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Module Data", menuName = "Nebra/ModuleData", order = 0)]
public class ModuleData : ScriptableObject {
    public GridObjectType objectType;
    public bool isDistributor;
    [FormerlySerializedAs("defaultBuildData")] public BuildData buildData;
    [FormerlySerializedAs("defaultDistributionData")] public DistributionData distributionData;
    [FormerlySerializedAs("defaultDescriptionData")] public DescriptionData descriptionData;
    [FormerlySerializedAs("startingProduction")] public int buildingId;
    [FormerlySerializedAs("droneMoveInPosition")] public Vector3Int droneMoveInPosition;
    [FormerlySerializedAs("droneMoveOutPosition")] public Vector3Int droneMoveOutPosition;
    [FormerlySerializedAs("droneMoveOutPosition")] public List<Vector3> droneStayPoints;


    public GridCoordinate PlanePositionToOrigin(GridCoordinate planeCoordinate, GridOrientation orientation) {
        return planeCoordinate - buildData.linkCenter.Rotate(orientation);
    }
}
