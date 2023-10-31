using SolarAscension;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Input = SolarAscension.Input;

public class PlayerInput {
	private readonly Camera _camera;
	private readonly OrbitalCamera _orbitCamera;
	private readonly Input.GameActions _input;

	private readonly float _maxRayDistance;
	private readonly LayerMask _buildSystemLayer, _moduleLayer;

	public Vector2 CursorPosition => _input.CursorPosition.ReadValue<Vector2>();

	public static bool cameraAutoAdjustEnabled = true;

	public PlayerInput(Camera camera, LayerMask buildSystemLayer, LayerMask moduleLayer) {
		_camera = camera;
		_orbitCamera = camera.GetComponent<OrbitalCamera>();
		_maxRayDistance = camera.farClipPlane;
		_buildSystemLayer = buildSystemLayer;
		_moduleLayer = moduleLayer;
			
		_input = new Input().Game;
		_input.Enable();
	}

	public GridCoordinate GetCameraFocusPoint() => _orbitCamera.focusPointCoordinate;

	public void SetCameraFocusPointToLinkCenter(GridCoordinate linkCenter) {
		if ( cameraAutoAdjustEnabled ) {
			_orbitCamera.SetFocusPointFromCoordinate(linkCenter);
		}
	}

	public bool IsCursorOverGridObject(out GridCoordinate hitCoordinate) {
		if ( EventSystem.current.IsPointerOverGameObject() ) {
			hitCoordinate = GridCoordinate.Invalid;
			return false;
		}
			
		Ray worldRay = _camera.ScreenPointToRay(CursorPosition);

		if ( Physics.SphereCast(worldRay, BuildSystem.EighthScale, out RaycastHit hit, _maxRayDistance, _buildSystemLayer) ) {
			hitCoordinate = hit.point.ToGridCoordinate();
			return true;
		}

		hitCoordinate = GridCoordinate.Invalid;
		return false;
	}
		
	public bool IsCursorOverGridCoordinate(Vector3 testPlaneOrigin, Vector3 testPlaneNormal, out GridCoordinate hitCoordinate) {
		if ( EventSystem.current.IsPointerOverGameObject() ) {
			hitCoordinate = GridCoordinate.Invalid;
			return false;
		}
			
		Plane interactionPlane = new Plane(testPlaneNormal, testPlaneOrigin);
		Ray worldRay = _camera.ScreenPointToRay(CursorPosition);

		if ( interactionPlane.Raycast(worldRay, out float distance) ) {
			hitCoordinate = worldRay.GetPoint(distance).ToGridCoordinate();
			return true;
		}
			
		hitCoordinate = GridCoordinate.Invalid;
		return false;
	}
		
	public bool IsCursorOverGridObject(out GridBehaviour hitObject) {
		if ( EventSystem.current.IsPointerOverGameObject() ) {
			hitObject = null;
			return false;
		}
			
		Ray worldRay = _camera.ScreenPointToRay(CursorPosition);

		if ( Physics.Raycast(worldRay, out RaycastHit hit, _maxRayDistance, _moduleLayer) ) {
			if ( hit.transform.TryGetComponent(out GridMeshObject meshObject) ) {
				hitObject = meshObject.gridBehaviour;
				return true;
			}
		}

		hitObject = null;
		return false;
	}

	public bool IsCancelling() {
		return _input.Cancel.WasPerformedThisFrame();
	}

	public bool IsSelecting() {
		return _input.Select.WasPerformedThisFrame();
	}

	public bool IsMultiBuildPerformed() {
		return _input.ToggleContinuous.WasPerformedThisFrame();
	}

	public bool IsRotateHorizontal() {
		return _input.RotateHorizontal.WasPerformedThisFrame();
	}

	public bool IsRotateVertical() {
		return _input.RotateVertical.WasPerformedThisFrame();
	}

	public bool IsRotateForward() {
		return _input.RotateForward.WasPerformedThisFrame();
	}

	public bool IsShiftHeld() {
		return Keyboard.current.shiftKey.ReadValue() > 0.1f;
	}

	public bool IsToggleGizmos() {
		return _input.ToggleGizmos.WasPerformedThisFrame();
	}

	public bool IsPlaneAlongYZRequested() {
		return _input.RotatePlaneHorizontal.WasPerformedThisFrame();
	}

	public bool IsPlaneAlongXZRequested() {
		return _input.RotatePlaneVertical.WasPerformedThisFrame();
	}

	public bool IsPlaneAlongXYRequested() {
		return _input.RotatePlaneForward.WasPerformedThisFrame();
	}

	public bool IsPlaneMoveUp() {
		return _input.MovePlaneUp.WasPerformedThisFrame();
	}

	public bool IsPlaneMoveDown() {
		return _input.MovePlaneDown.WasPerformedThisFrame();
	}
}