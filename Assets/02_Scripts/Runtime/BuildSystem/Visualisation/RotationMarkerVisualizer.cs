using UnityEngine;
using UnityEngine.Rendering;

public class RotationMarkerVisualizer : MonoBehaviour {
	[SerializeField] private Transform cameraTransform;
	[SerializeField] private Mesh markerMesh;
	[SerializeField] private Mesh planeMesh;
	[SerializeField] private Mesh keyMesh;
	[SerializeField] private Material markerMaterial, planeMaterial, keyXMaterial, keyYMaterial, keyZMaterial;

	private readonly Vector3 _arrowScale = new(15f, 25f, 25f);
	private readonly Vector3 _planeScale = new(17.5f, 17.5f, 1f);
	
	private readonly Matrix4x4[] _markerMatrices = new Matrix4x4[3];
	private readonly Matrix4x4[] _keyMatrices = new Matrix4x4[3];
	private readonly Matrix4x4[] _planeMatrices = new Matrix4x4[3];

	private GridCoordinate _lastRenderedGridSize;
	private GridCoordinate _lastRenderedBuildPosition;
	private bool _arrowsRenderedLast;
	
	public void RenderArrowMarkers(GridCoordinate gridSize, GridCoordinate buildPosition) { 
		if ( gridSize != _lastRenderedGridSize || buildPosition != _lastRenderedBuildPosition || !_arrowsRenderedLast ) {
			UpdateMarkerMatrices(buildPosition);
			UpdateKeyMatrices(buildPosition);
		}

		Graphics.DrawMeshInstanced(markerMesh, 0, markerMaterial, _markerMatrices, _markerMatrices.Length, null, ShadowCastingMode.Off, false);
		RenderKeyMarkers();

		_lastRenderedGridSize = gridSize;
		_lastRenderedBuildPosition = buildPosition;
		_arrowsRenderedLast = true;
	}
	
	public void RenderPlaneMarkers(GridCoordinate gridSize, GridCoordinate buildPosition) { 
		if ( gridSize != _lastRenderedGridSize || buildPosition != _lastRenderedBuildPosition || _arrowsRenderedLast ) {
			UpdatePlaneMatrices(buildPosition);
			UpdateKeyMatrices(buildPosition);
		}

		Graphics.DrawMeshInstanced(planeMesh, 0, planeMaterial, _planeMatrices, _planeMatrices.Length, null, ShadowCastingMode.Off, false);
		RenderKeyMarkers();

		_lastRenderedGridSize = gridSize;
		_lastRenderedBuildPosition = buildPosition;
		_arrowsRenderedLast = false;
	}

	private void UpdatePlaneMatrices(GridCoordinate buildPosition) {
		Vector3 xOffset = Vector3.up * (6f * BuildSystem.HalfScale);
		Vector3 yOffset = new Vector3(0f, 0f, -1f) * (5f * BuildSystem.HalfScale);
		Vector3 zOffset = new Vector3(-1f, 0.5f, 0f) * (8f * BuildSystem.HalfScale);

		_planeMatrices[0] = Matrix4x4.TRS(buildPosition.ToWorldPositionCentered() + xOffset, Quaternion.Euler(90f, 0f, 0f), _planeScale);
		_planeMatrices[1] = Matrix4x4.TRS(buildPosition.ToWorldPositionCentered() + yOffset, Quaternion.Euler(0f, 90f, 0f), _planeScale);
		_planeMatrices[2] = Matrix4x4.TRS(buildPosition.ToWorldPositionCentered() + zOffset, Quaternion.identity, _planeScale);
	}

	private void UpdateMarkerMatrices(GridCoordinate buildPosition) {
		Vector3 xOffset = Quaternion.AngleAxis(45f, Vector3.right) * Vector3.up * (5f * BuildSystem.HalfScale);
		Vector3 yOffset = Vector3.back * (5f * BuildSystem.HalfScale);
		Vector3 zOffset = Vector3.up * (5f * BuildSystem.HalfScale);

		_markerMatrices[0] = Matrix4x4.TRS(buildPosition.ToWorldPositionCentered() + xOffset, Quaternion.AngleAxis(45f, Vector3.right), _arrowScale);
		_markerMatrices[1] = Matrix4x4.TRS(buildPosition.ToWorldPositionCentered() + yOffset, Quaternion.Euler(0, -90, 90), _arrowScale);
		_markerMatrices[2] = Matrix4x4.TRS(buildPosition.ToWorldPositionCentered() + zOffset, Quaternion.AngleAxis(90f, Vector3.up), _arrowScale);
	}

	private void RenderKeyMarkers() {
		Graphics.DrawMesh(keyMesh, _keyMatrices[0], keyXMaterial, 0, null, 0, null, ShadowCastingMode.Off, false);
		Graphics.DrawMesh(keyMesh, _keyMatrices[1], keyYMaterial, 0, null, 0, null, ShadowCastingMode.Off, false);
		Graphics.DrawMesh(keyMesh, _keyMatrices[2], keyZMaterial, 0, null, 0, null, ShadowCastingMode.Off, false);
	}

	private void UpdateKeyMatrices(GridCoordinate buildPosition) {
		Quaternion keyRotation = Quaternion.LookRotation(cameraTransform.forward, cameraTransform.up);

		Vector3 xKeyOffset = Vector3.up * (8f * BuildSystem.HalfScale) + Vector3.forward * 5f;
		Vector3 yKeyOffset = Vector3.back * (5f * BuildSystem.HalfScale) + Vector3.right * 15f;
		Vector3 zKeyOffset = Vector3.up * (5f * BuildSystem.HalfScale) + Vector3.left * 12.5f;

		_keyMatrices[0] = Matrix4x4.TRS(buildPosition.ToWorldPositionCentered() + xKeyOffset, keyRotation, Vector3.one * 5f);
		_keyMatrices[1] = Matrix4x4.TRS(buildPosition.ToWorldPositionCentered() + yKeyOffset, keyRotation, Vector3.one * 5f);
		_keyMatrices[2] = Matrix4x4.TRS(buildPosition.ToWorldPositionCentered() + zKeyOffset, keyRotation, Vector3.one * 5f);
	}
}
