using SolarAscension;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteAlways, DisallowMultipleComponent]
public class ModulDataDroneConfigurator : MonoBehaviour {
    public ModuleData ModuleData;
    
#if UNITY_EDITOR
private void OnDrawGizmos() {
        if (ModuleData.buildData == null) {
            return;
        }

        Gizmos.color = GizmoUtils.VolumeFillColor;

        Vector3 size = (Vector3)ModuleData.buildData.gridSize * BuildSystem.VolumeScale;
        Vector3 center = (ModuleData.buildData.gridSize - Vector3.one) * BuildSystem.HalfScale;

        Vector3 modulCenter = center;

        Gizmos.DrawCube(center, size);

        Gizmos.color = GizmoUtils.VolumeWireColor;
        Gizmos.DrawWireCube(center, size);

        Gizmos.color = GizmoUtils.BufferFillColor;


        Gizmos.DrawCube(ModuleData.buildData.linkCenter.ToWorldPositionCentered(), Vector3.one * BuildSystem.VolumeScale);

        for (int z = 0; z < ModuleData.buildData.gridSize.z; z++) {
            Vector3 pos = new Vector3(0f, 0f, z) * BuildSystem.VolumeScale - Vector3.one * (BuildSystem.HalfScale - 1f);
            Handles.Label(pos, new GridCoordinate(0, 0, z).ToString(), GizmoUtils.LabelStyle);
        }

        for (int y = 0; y < ModuleData.buildData.gridSize.y; y++) {
            Vector3 pos = new Vector3(0f, y, 0f) * BuildSystem.VolumeScale - Vector3.one * (BuildSystem.HalfScale - 1f);
            Handles.Label(pos, new GridCoordinate(0, y, 0).ToString(), GizmoUtils.LabelStyle);
        }

        for (int x = 0; x < ModuleData.buildData.gridSize.x; x++) {
            Vector3 pos = new Vector3(x, 0f, 0f) * BuildSystem.VolumeScale - Vector3.one * (BuildSystem.HalfScale - 1f);
            Handles.Label(pos, new GridCoordinate(x, 0, 0).ToString(), GizmoUtils.LabelStyle);
        }

        Gizmos.color = GizmoUtils.LinkFillColor;
        foreach (GridLink link in ModuleData.buildData.links) {
            center = link.localCoordinate.ToWorldPositionCentered();
            size = Vector3.one * BuildSystem.VolumeScale;
            Gizmos.DrawCube(center, size);
        }

        Gizmos.color = GizmoUtils.LinkWireColor;
        foreach (GridLink link in ModuleData.buildData.links) {
            center = link.localCoordinate.ToWorldPositionCentered();
            size = Vector3.one * BuildSystem.VolumeScale;
            Gizmos.DrawWireCube(center, size);
        }

        Span<GridCoordinate> offsets = stackalloc GridCoordinate[6];

        Gizmos.color = GizmoUtils.SelectionFillColor;
        foreach (GridLink link in ModuleData.buildData.links) {
            int offsetCount = link.connectorDirection.ToGridVectorsAll(ref offsets);

            for (int i = 0; i < offsetCount; i++) {
                center = (link.localCoordinate + offsets[i]).ToWorldPositionCentered();
                Gizmos.DrawSphere(center, BuildSystem.HalfScale);
            }
        }

        Gizmos.color = GizmoUtils.SelectionWireColor;
        foreach (GridLink link in ModuleData.buildData.links) {
            int offsetCount = link.connectorDirection.ToGridVectorsAll(ref offsets);

            for (int i = 0; i < offsetCount; i++) {
                center = (link.localCoordinate + offsets[i]).ToWorldPositionCentered();
                Gizmos.DrawWireSphere(center, BuildSystem.HalfScale);
            }
        }

        foreach (GridLink link in ModuleData.buildData.links) {
            center = link.localCoordinate.ToWorldPositionCentered();
            Handles.Label(center, link.connectorDirection.ToString(), GizmoUtils.LabelStyle);
        }

        Gizmos.color = new Color(1, 1, 1, 0.5f);

        Gizmos.DrawMesh(ModuleData.buildData.mesh, -1, modulCenter);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(ModuleData.droneMoveInPosition * 5, new Vector3(5f, 5f, 5f));

        Gizmos.color = Color.green;
        Gizmos.DrawCube(ModuleData.droneMoveOutPosition * 5, new Vector3(5f, 5f, 5f));


        Vector3 lastPos = ModuleData.droneMoveInPosition * 5;
        foreach (Vector3 pos in ModuleData.droneStayPoints) {

            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(pos, new Vector3(0.5f, 0.5f, 0.5f));


            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(lastPos, pos);
            lastPos = pos;
        }

        Gizmos.DrawLine(ModuleData.droneMoveOutPosition * 5, lastPos);
    }
#endif
}
