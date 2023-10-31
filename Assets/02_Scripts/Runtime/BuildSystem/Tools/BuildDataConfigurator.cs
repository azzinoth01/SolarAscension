using System;
using SolarAscension;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor only component to let Game Designers or Artists author module links with minimal overhead and direct feedback.
/// </summary>
[ExecuteAlways, DisallowMultipleComponent]
public class BuildDataConfigurator : MonoBehaviour {
	public BuildData buildDataToEdit;

	public AttachmentTransform[] attachmentTransforms;
	public Mesh attachmentTransformsMesh;

	public int attachmentConfigIndex;
	public int radialAttachmentCounts;
	public Vector3 radialAttachmentEulerAngles;
	public Vector3 radialOriginPoint;
	public Vector3 radialAxisPosition;
	public Vector3 rotationAxis;
	public float angleOffset;
		
	#if UNITY_EDITOR
	public void ApplyAttachmentTransforms() {
		buildDataToEdit.SetAttachmentTransforms(attachmentTransforms, attachmentConfigIndex);
		EditorUtility.SetDirty(buildDataToEdit);
		Undo.RecordObject(buildDataToEdit, "Set Attachment Transforms on Build Data");
	}

	public void GenerateRadialAttachmentTransforms() {
		attachmentTransforms = new AttachmentTransform[radialAttachmentCounts];

		Vector3 deltaOriginToAxis = radialOriginPoint - radialAxisPosition;

		for (int i = 0; i < radialAttachmentCounts; i++) {
			float angle = 360f / (radialAttachmentCounts - 1) * i + angleOffset;
			Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis);
			Vector3 rotatedOffset = rotation * deltaOriginToAxis;

			attachmentTransforms[i] = new AttachmentTransform {
				position = radialOriginPoint + rotatedOffset,
				rotation = (Quaternion.Euler(radialAttachmentEulerAngles) * rotation).eulerAngles
			};
		}
	}
	
	private void OnDrawGizmos() {
		if ( buildDataToEdit == null ) {
			return;
		}

		Gizmos.color = GizmoUtils.VolumeFillColor;

		Vector3 size = (Vector3)buildDataToEdit.gridSize * BuildSystem.VolumeScale;
		Vector3 center = (buildDataToEdit.gridSize - Vector3.one) * BuildSystem.HalfScale;
		Gizmos.DrawCube(center, size);

		Gizmos.color = GizmoUtils.VolumeWireColor;
		Gizmos.DrawWireCube(center, size);

		Gizmos.color = GizmoUtils.BufferFillColor;
		Gizmos.DrawCube(buildDataToEdit.linkCenter.ToWorldPositionCentered(), Vector3.one * BuildSystem.VolumeScale);

		for (int z = 0; z < buildDataToEdit.gridSize.z; z++) {
			Vector3 pos = new Vector3(0f, 0f, z) * BuildSystem.VolumeScale - Vector3.one * (BuildSystem.HalfScale - 1f);
			Handles.Label(pos, new GridCoordinate(0, 0, z).ToString(), GizmoUtils.LabelStyle);
		}

		for (int y = 0; y < buildDataToEdit.gridSize.y; y++) {
			Vector3 pos = new Vector3(0f, y, 0f) * BuildSystem.VolumeScale - Vector3.one * (BuildSystem.HalfScale - 1f);
			Handles.Label(pos, new GridCoordinate(0, y, 0).ToString(), GizmoUtils.LabelStyle);
		}

		for (int x = 0; x < buildDataToEdit.gridSize.x; x++) {
			Vector3 pos = new Vector3(x, 0f, 0f) * BuildSystem.VolumeScale - Vector3.one * (BuildSystem.HalfScale - 1f);
			Handles.Label(pos, new GridCoordinate(x, 0, 0).ToString(), GizmoUtils.LabelStyle);
		}

		Gizmos.color = GizmoUtils.LinkFillColor;
		foreach (GridLink link in buildDataToEdit.links) {
			center = link.localCoordinate.ToWorldPositionCentered();
			size = Vector3.one * BuildSystem.VolumeScale;
			Gizmos.DrawCube(center, size);
		}
			
		Gizmos.color = GizmoUtils.LinkWireColor;
		foreach (GridLink link in buildDataToEdit.links) {
			center = link.localCoordinate.ToWorldPositionCentered();
			size = Vector3.one * BuildSystem.VolumeScale;
			Gizmos.DrawWireCube(center, size);
		}

		Span<GridCoordinate> offsets = stackalloc GridCoordinate[6];

		Gizmos.color = GizmoUtils.SelectionFillColor;
		foreach (GridLink link in buildDataToEdit.links) {
			int offsetCount = link.connectorDirection.ToGridVectorsAll(ref offsets);

			for (int i = 0; i < offsetCount; i++) {
				center = (link.localCoordinate + offsets[i]).ToWorldPositionCentered();
				Gizmos.DrawSphere(center, BuildSystem.HalfScale);
			}
		}
			
		Gizmos.color = GizmoUtils.SelectionWireColor;
		foreach (GridLink link in buildDataToEdit.links) {
			int offsetCount = link.connectorDirection.ToGridVectorsAll(ref offsets);

			for (int i = 0; i < offsetCount; i++) {
				center = (link.localCoordinate + offsets[i]).ToWorldPositionCentered();
				Gizmos.DrawWireSphere(center, BuildSystem.HalfScale);
			}
		}

		foreach (GridLink link in buildDataToEdit.links) {
			center = link.localCoordinate.ToWorldPositionCentered();
			Handles.Label(center, link.connectorDirection.ToString(), GizmoUtils.LabelStyle);
		}

		Gizmos.color = GizmoUtils.VolumeFillColor;
		Gizmos.DrawMesh(buildDataToEdit.mesh, -1, buildDataToEdit.GetCenterPoint(Quaternion.identity));

		if ( !attachmentTransformsMesh ) {
			return;
		}

		if ( radialAttachmentCounts > 0 ) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(radialOriginPoint, 1f);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(radialAxisPosition, 1f);
		}

		Gizmos.color = GizmoUtils.BufferFillColor;
		if ( radialAttachmentCounts == 0 ) {
			for (int i = 0; i < attachmentTransforms.Length; i++) {
				Gizmos.DrawMesh(attachmentTransformsMesh, -1, attachmentTransforms[i].position, Quaternion.Euler(attachmentTransforms[i].rotation));
			}
		} else {
			for (int i = 0; i < radialAttachmentCounts; i++) {
				Vector3 deltaOriginToAxis = radialOriginPoint - radialAxisPosition;

				for (int j = 0; j < radialAttachmentCounts; j++) {
					float angle = 360f / (radialAttachmentCounts - 1) * j + angleOffset;
					Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis);
					Vector3 rotatedOffset = rotation * deltaOriginToAxis;

					Gizmos.DrawMesh(attachmentTransformsMesh, -1, radialAxisPosition + rotatedOffset, rotation * Quaternion.Euler(radialAttachmentEulerAngles));
				}
			}
		}
	}
	#endif
}