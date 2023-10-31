using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum StateFlag {
	None = 0,
	Selected = 1,
	Hover = 2,
	Placing = 4,
	ShowPath = 8,
	Moving = 16,
	Deleting = 32,
	OpenLinks = 64,
	OverlayOxygen = 128
}
	
public class PlayerStateData {
	public GridBehaviour startingGridBehaviour;
	public GridBehaviour lastHoveredGridBehaviour;
	public GridBehaviour hoveredGridBehaviour;
	public GridBehaviour selectedGridBehaviour;
	public ModuleData selectedModuleData;

	public RequestResponse lastRequestResponse;
	public List<GridCoordinate> pathfinderPath;

	public bool showGizmos = true;
	public bool showBlockedVolume = true;

	private static readonly GridOrientation XYOrientation = new(GridDirection.Right, GridDirection.Forward, GridDirection.Down);
	private static readonly GridOrientation XZOrientation = GridOrientation.Identity;
	private static readonly GridOrientation YZOrientation = new(GridDirection.Down, GridDirection.Right, GridDirection.Forward);
		
	private static GridCoordinate _planeCoordinate;
	private static GridOrientation _planeOrientation;

	private StateFlag _stateFlag;

	public PlayerStateData() {
		_planeOrientation = GridOrientation.Identity;
	}

	public static GridCoordinate GetPlaneNormalGridVector() {
		return _planeOrientation[GridDirection.Up].ToGridVector();
	}

	public static GridCoordinate GetPlanePositionAlongNormal() {
		GridCoordinate normalDirection = GetPlaneNormalGridVector();
		return _planeCoordinate * normalDirection;
	}

	public static GridOrientation GetPlaneMarkerOrientation() {
		if (_planeOrientation == XZOrientation) return XYOrientation;
		if ( _planeOrientation == YZOrientation ) return GridOrientation.Identity.RotateHorizontal();
		return GridOrientation.Identity;
	}

	public static GridOrientation GetPlaneOrientation() => _planeOrientation;

	public void SetFlags(StateFlag flag) {
		_stateFlag = flag;
	}

	public void SetFlag(StateFlag flag, bool value) {
		if ( value ) {
			_stateFlag |= flag;
		} else {
			_stateFlag &= ~flag;
		}
	}

	public void ResetFlags() {
		_stateFlag = 0;
	}

	public bool CheckFlag(StateFlag flag) {
		return (_stateFlag & flag) != 0;
	}

	public void SetPlaneCoordinate(GridCoordinate coordinate) {
		_planeCoordinate = coordinate;
	}

	public GridCoordinate GetPlaneCoordinate() => _planeCoordinate;

	public Vector3 GetPlanePosition() => _planeCoordinate.ToWorldPositionCentered();


	public Vector3 GetPlaneNormal() {
		return _planeOrientation.Up;
	}

	public GridObjectType GetSelectedObjectType() {
		return selectedModuleData.objectType;
	}

	public void SetPlaneAlongXY() {
		_planeOrientation = XYOrientation;
	}

	public void SetPlaneAlongXZ() {
		_planeOrientation = XZOrientation;
	}

	public void SetPlaneAlongYZ() {
		_planeOrientation = YZOrientation;
	}

	public void MovePlaneUp(PlayerInput input) {
		_planeCoordinate += new GridCoordinate(0, 1, 0).Rotate(_planeOrientation);

		if ( GridCoordinate.Distance(input.GetCameraFocusPoint(), _planeCoordinate) > 10 ) {
			input.SetCameraFocusPointToLinkCenter(_planeCoordinate);
		}
	}

	public void MovePlaneDown(PlayerInput input) {
		_planeCoordinate += new GridCoordinate(0, -1, 0).Rotate(_planeOrientation);
		if ( GridCoordinate.Distance(input.GetCameraFocusPoint(), _planeCoordinate) > 10 ) {
			input.SetCameraFocusPointToLinkCenter(_planeCoordinate);
		}
	}
}