using System.Collections.Generic;
using SolarAscension;

public class BuildMultipleObjectsState : BuildState {
	public BuildMultipleObjectsState(PlayerState state, RotationMarkerVisualizer markerVisualizer, BuildingGrid buildingGrid) : base(state, markerVisualizer, buildingGrid) {
		_pathfinder = new BuildPathfinder(grid);
	}

	public static int pathLength;

	private readonly BuildPathfinder _pathfinder;
	private GridCoordinate _startCoordinate;
	private bool _isPathfinding;

	public override void Enter() {
		_startCoordinate = GridCoordinate.Invalid;
		_isPathfinding = false;

		stateData.SetFlags(StateFlag.Placing | StateFlag.OpenLinks);
	}

	public override void ProcessInput(PlayerInput input) {
		if ( input.IsCancelling() ) {
			Exit(InteractionState.BuildSingle);
			return;
		}

		if ( _isPathfinding ) {
			if ( _pathfinder.HasPath ) {
				pathLength = _pathfinder.Path.Count;
				if ( input.IsSelecting() || input.IsMultiBuildPerformed() ) {
					BuildContinuous(input);
					return;
				}
			} else {
				pathLength = 0;
			}
				
			stateData.SetFlag(StateFlag.ShowPath, _pathfinder.HasPath);
			stateData.pathfinderPath = _pathfinder.Path;

			if ( input.IsCursorOverGridCoordinate(stateData.GetPlanePosition(), stateData.GetPlaneNormal(), out GridCoordinate hitCoordinate) ) {
				_pathfinder.FindPath(hitCoordinate);
			}
		} else {
			BuildRequestResponse requestResponse = default;
			
			if ( input.IsCursorOverGridObject(out GridCoordinate hitCoordinate) ) {
				requestResponse = grid.ValidateBuildRequestFromLink(stateData.selectedModuleData, false, hitCoordinate, buildRotation);
			}

			if ( requestResponse.response != RequestResponse.Valid &&
			     input.IsCursorOverGridCoordinate(stateData.GetPlanePosition(), stateData.GetPlaneNormal(), out hitCoordinate) ) {
				BuildVisualizer.ClearDrawCommands();
				requestResponse =
					grid.ValidateBuildRequestFromLink(stateData.selectedModuleData, false, hitCoordinate, buildRotation);

				if ( requestResponse.response != RequestResponse.Valid ) {
					BuildVisualizer.ClearDrawCommands();
					GridCoordinate candidateOrigin = stateData.selectedModuleData.PlanePositionToOrigin(hitCoordinate, buildRotation);
					requestResponse = grid.ValidateBuildRequestFromOrigin(stateData.selectedModuleData, false, candidateOrigin, buildRotation);
				}
			}

			stateData.lastRequestResponse = requestResponse.response;
				
			if ( requestResponse.response != RequestResponse.Valid ) { 
				Exit(InteractionState.BuildSingle);
				return;
			}
				
			_startCoordinate = hitCoordinate;
			stateData.SetPlaneCoordinate(hitCoordinate);
			_pathfinder.SetStartingCoordinate(_startCoordinate);
			_isPathfinding = true;
				
			stateData.SetFlag(StateFlag.OpenLinks, false);
		}
	}

	private void BuildContinuous(PlayerInput input) {
		List<GridCoordinate> coords = _pathfinder.Path;
		
		foreach (GridCoordinate coord in coords) {
			var response = grid.ValidateBuildRequestFromOrigin(stateData.selectedModuleData, true, coord, GridOrientation.Identity);

			if ( response.response == RequestResponse.Valid ) {
				GridCoordinate linkCenter = response.linkCenterCoordinate;
				GridCoordinate currentPlane = stateData.GetPlaneCoordinate();

				if ( GridCoordinate.Distance(currentPlane, linkCenter) > 10 ) {
					stateData.SetPlaneCoordinate(linkCenter);
					input.SetCameraFocusPointToLinkCenter(linkCenter);
				}
			}
		}
			
		_pathfinder.Disable();
		pathLength = 0;
		Exit(InteractionState.BuildSingle);
			
		AudioManager.Instance.Play("action_moduleBuild");
	}
}