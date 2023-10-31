using SolarAscension;
using UnityEngine.EventSystems;

public class BuildSingleObjectState : BuildState {
	public BuildSingleObjectState(PlayerState state, RotationMarkerVisualizer markerVisualizer, BuildingGrid buildingGrid) : base(state, markerVisualizer, buildingGrid) { }

	public override void Enter() {
		stateData.SetFlags(StateFlag.Placing | StateFlag.OpenLinks);
		buildRotation = GridOrientation.Identity;
	}

	public override void ProcessInput(PlayerInput input) {
		if ( EventSystem.current.IsPointerOverGameObject() ) {
			return;
		}
			
		if ( input.IsCancelling() ) {
			Exit(InteractionState.Maintain);
			return;
		}

		if ( input.IsMultiBuildPerformed() && stateData.GetSelectedObjectType() == GridObjectType.Tube ) {
			Exit(InteractionState.BuildMultiple);
			return;
		}

		buildRotation = UpdateBuildRotation(input);
			
		bool tryBuild = input.IsSelecting();
		BuildRequestResponse requestResponse = default;
			
		if ( input.IsCursorOverGridObject(out GridCoordinate hitCoordinate) ) {
			requestResponse = grid.ValidateBuildRequestFromLink(stateData.selectedModuleData, tryBuild, hitCoordinate, buildRotation);
		}

		if ( requestResponse.response != RequestResponse.Valid &&
		     input.IsCursorOverGridCoordinate(stateData.GetPlanePosition(), stateData.GetPlaneNormal(), out hitCoordinate) ) {
			BuildVisualizer.ClearDrawCommands();
			requestResponse =
				grid.ValidateBuildRequestFromLink(stateData.selectedModuleData, tryBuild, hitCoordinate, buildRotation);

			if ( requestResponse.response != RequestResponse.Valid ) {
				BuildVisualizer.ClearDrawCommands();
				GridCoordinate candidateOrigin = stateData.selectedModuleData.PlanePositionToOrigin(hitCoordinate, buildRotation);
				requestResponse = grid.ValidateBuildRequestFromOrigin(stateData.selectedModuleData, tryBuild, candidateOrigin, buildRotation);
			}
		}

		if ( stateData.showBlockedVolume ) {
			BuildVisualizer.TryRenderNeighboringBlockZones(hitCoordinate);
		}

		if ( stateData.showGizmos && stateData.selectedModuleData.objectType != GridObjectType.Tube ) {
			if ( input.IsShiftHeld() ) {
				rotationVisualizer.RenderPlaneMarkers(stateData.selectedModuleData.buildData.gridSize, hitCoordinate + requestResponse.buildDirection);
			} else {
				rotationVisualizer.RenderArrowMarkers(stateData.selectedModuleData.buildData.gridSize, hitCoordinate + requestResponse.buildDirection);
			}
		}

		if ( tryBuild ) {
			AudioManager.Instance.Play(requestResponse.response != RequestResponse.Valid
				? "action_moduleBuildNP"
				: stateData.selectedModuleData.buildData.buildSfxName);

			if ( requestResponse.response == RequestResponse.Valid ) {
				GridCoordinate linkCenter = requestResponse.linkCenterCoordinate;
				GridCoordinate currentPlane = stateData.GetPlaneCoordinate();

				if ( GridCoordinate.Distance(currentPlane, linkCenter) > 10 ) {
					stateData.SetPlaneCoordinate(linkCenter);
					input.SetCameraFocusPointToLinkCenter(linkCenter);
				}
			}
		}

		stateData.lastRequestResponse = requestResponse.response;
	}
}