using System.Collections.Generic;
using SolarAscension;

public class BuildMovingState : BuildState {
	public BuildMovingState(PlayerState state, RotationMarkerVisualizer markerVisualizer, BuildingGrid buildingGrid) : base(state, markerVisualizer, buildingGrid) { }
	
	public static GridBehaviour GridBehaviourToMove { get; private set; }
	private static readonly List<GridObject> ObjectsToMove = new(1);
	private static Module _moduleToMove;

	public override void Enter() {
		GridBehaviourToMove = null;
		ObjectsToMove.Clear();
		_moduleToMove = null;
		buildRotation = GridOrientation.Identity;
			
		stateData.SetFlags(StateFlag.Selected | StateFlag.Hover);
	}

	public override void ProcessInput(PlayerInput input) {
		if ( GridBehaviourToMove == null ) {
			if ( input.IsCancelling() ) {
				Exit(InteractionState.Maintain);
				return;
			}
				
			if ( input.IsSelecting() ) {
				if ( !input.IsCursorOverGridObject(out GridBehaviour hitObject) ) {
					return;
				}
					
				if ( hitObject.GridObject is not Module module ) {
					return;
				}
					
				ObjectsToMove.Clear();
				ObjectsToMove.Add(module);
					
				if ( !module.ValidateTearDown(ObjectsToMove) ) {
					AudioManager.Instance.Play("action_moduleBuildNP");
					return;
				}
					
				GridBehaviourToMove = hitObject;
				_moduleToMove = module;
				buildRotation = module.orientation;
							
				module.BeginRelocation();
				hitObject.HideMesh();
				stateData.SetFlag(StateFlag.Placing | StateFlag.Moving | StateFlag.OpenLinks, true);
			}
		} else {
			if ( input.IsCancelling() ) {
				Exit(InteractionState.Maintain);
				return;
			}
			
			buildRotation = UpdateBuildRotation(input);

			bool tryMove = input.IsSelecting();
			BuildRequestResponse requestResponse = default;
			
			if ( input.IsCursorOverGridObject(out GridCoordinate hitCoordinate) ) {
				requestResponse = grid.ValidateMoveRequestFromLink(_moduleToMove, tryMove, hitCoordinate, buildRotation);
			}

			if ( requestResponse.response != RequestResponse.Valid &&
			     input.IsCursorOverGridCoordinate(stateData.GetPlanePosition(), stateData.GetPlaneNormal(), out hitCoordinate) ) {
				BuildVisualizer.ClearDrawCommands();
				requestResponse = grid.ValidateMoveRequestFromLink(_moduleToMove, tryMove, hitCoordinate, buildRotation);

				if ( requestResponse.response != RequestResponse.Valid ) {
					BuildVisualizer.ClearDrawCommands();
					GridCoordinate candidateOrigin = _moduleToMove.moduleData.PlanePositionToOrigin(hitCoordinate, buildRotation);
					requestResponse = grid.ValidateMoveRequestFromOrigin(_moduleToMove, tryMove, candidateOrigin, buildRotation);
				}
			}
			
			if ( stateData.showGizmos ) {
				if ( input.IsShiftHeld() ) {
					rotationVisualizer.RenderPlaneMarkers(stateData.selectedModuleData.buildData.gridSize, hitCoordinate + requestResponse.buildDirection);
				} else {
					rotationVisualizer.RenderArrowMarkers(stateData.selectedModuleData.buildData.gridSize, hitCoordinate + requestResponse.buildDirection);
				}
			}
			
			if ( stateData.showBlockedVolume ) {
				BuildVisualizer.TryRenderNeighboringBlockZones(hitCoordinate);
			}

			if ( tryMove && requestResponse.response == RequestResponse.Valid ) {
				GridBehaviourToMove.ShowMesh();
				AudioManager.Instance.Play("action_moduleBuild");
				stateData.SetFlags(StateFlag.Selected | StateFlag.Hover);
				GridBehaviourToMove = null;
				ObjectsToMove.Clear();
					
				GridCoordinate linkCenter = requestResponse.linkCenterCoordinate;
				GridCoordinate currentPlane = stateData.GetPlaneCoordinate();

				if ( GridCoordinate.Distance(currentPlane, linkCenter) > 10 ) {
					stateData.SetPlaneCoordinate(linkCenter);
					input.SetCameraFocusPointToLinkCenter(linkCenter);
				}
			}

			stateData.lastRequestResponse = requestResponse.response;
		}
	}

	public override void Exit(InteractionState exitState) {
		if ( GridBehaviourToMove != null ) {
			GridBehaviourToMove.ShowMesh();
			_moduleToMove.CancelRelocation();
		}
			
		ObjectsToMove.Clear();
		base.Exit(exitState);
	}
}