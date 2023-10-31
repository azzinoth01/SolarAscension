using System.Collections.Generic;
using SolarAscension;
using UnityEngine.EventSystems;

public class PlayerState {
	private readonly PlayerInput _input;
	private readonly BuildVisualizer _visualizer;
	private readonly Dictionary<InteractionState, State> _states;

	private State _cachedState;

	public State CurrentState { get; private set; }
	public InteractionState CurrentStateType { get; private set; }
	public PlayerStateData StateData { get; }

	public PlayerState(Player player, GridBehaviour startBehaviour, BuildVisualizer visualizer, RotationMarkerVisualizer markerVisualizer) {
		_input = player.Input;
		_visualizer = visualizer;
			
		StateData = new PlayerStateData();
		StateData.startingGridBehaviour = startBehaviour;
		StateData.SetPlaneCoordinate(startBehaviour.GetLinkCenter());
		StateData.lastHoveredGridBehaviour = startBehaviour;
			
		BuildingGrid grid = player.BuildingGrid;
			
		_visualizer.SetGrids(grid);

		_states = new Dictionary<InteractionState, State> {
			{ InteractionState.Maintain , new MaintainState(this) },
			{ InteractionState.BuildSingle, new BuildSingleObjectState(this, markerVisualizer, grid) },
			{ InteractionState.BuildMultiple, new BuildMultipleObjectsState(this, markerVisualizer, grid) },
			{ InteractionState.BuildMoving , new BuildMovingState(this, markerVisualizer, grid) },
			{ InteractionState.BuildDeleting , new BuildDeletingState(this, markerVisualizer, grid) }
		};

		CurrentState = _states[InteractionState.Maintain];
		CurrentStateType = InteractionState.Maintain;
		CurrentState.Enter();

		player.enabled = true;
	}

	public void ProcessUpdate() {
		UpdateCursorStates();
		UpdatePlaneState();
		UpdateGizmoState();
		CurrentState.ProcessInput(_input);
		RenderVisuals();
	}

	private void UpdateGizmoState() {
		if ( _input.IsToggleGizmos() ) {
			StateData.showGizmos = !StateData.showGizmos;
		}
	}

	private void UpdateCursorStates() {
		if ( EventSystem.current.IsPointerOverGameObject() ) {
			return;
		}

		if ( _input.IsCursorOverGridObject(out GridBehaviour hitObject) ) {
			StateData.lastHoveredGridBehaviour = StateData.hoveredGridBehaviour = hitObject;
				
			if ( _input.IsSelecting() ) {
				if ( hitObject != StateData.selectedGridBehaviour ) {
					StateData.selectedGridBehaviour = hitObject;
					AudioManager.Instance.Play(hitObject.GridObject.moduleData.buildData.selectSfxName);
				}

				if ( CurrentStateType is not (InteractionState.BuildSingle or InteractionState.BuildMoving) ) {
					StateData.SetPlaneCoordinate(hitObject.GetLinkCenter());
					MenuManager.Instance.modulePopupHandler.OpenSelectionInfoPopup(hitObject.GridObject);
				}
			}
		} else {
			StateData.hoveredGridBehaviour = null;

			if ( _input.IsSelecting() || _input.IsCancelling() ) {
				StateData.selectedGridBehaviour = null;
				MenuManager.Instance.modulePopupHandler.CloseSelectionInfoPopup();
			}
		}
	}

	private void UpdatePlaneState() {
		// if ( _input.IsTogglePlane() ) {
		// 	StateData.planeVisible = !StateData.planeVisible;
		// }

		if ( _input.IsPlaneAlongYZRequested() ) {
			StateData.SetPlaneAlongYZ();
		}
			
		if ( _input.IsPlaneAlongXYRequested() ) {
			StateData.SetPlaneAlongXY();
		}

		if ( _input.IsPlaneAlongXZRequested() ) {
			StateData.SetPlaneAlongXZ();
		}

		if ( CurrentStateType == InteractionState.BuildMultiple ) {
			return;
		}
			
		if ( _input.IsPlaneMoveUp() ) {
			StateData.MovePlaneUp(_input);
		}

		if ( _input.IsPlaneMoveDown() ) {
			StateData.MovePlaneDown(_input);
		}
	}

	public void RenderVisuals() {
		_visualizer.RenderVisuals(StateData);
	}

	public void ChangeState(InteractionState state) {
		if ( _states.TryGetValue(state, out State next) ) {
			CurrentState = next;
			CurrentState.Enter();
			CurrentStateType = state;
		}

		MenuManager.Instance.warningDisplayHandler.SetWarningDisplay(state);
		if(state != InteractionState.BuildSingle && state != InteractionState.BuildMultiple) {
			MenuManager.Instance.modulePopupHandler.CloseBuildInfoPopup();
			BuildingCostHandler.Root.SetActive(false);
        }
	}

	public void OverrideState(InteractionState state) {
		CurrentState.Exit(state);
	}

	public void SetBuildStateWithData(ModuleData moduleData) {
		StateData.selectedModuleData = moduleData;
		CurrentState.Exit(InteractionState.BuildSingle);
		MenuManager.Instance.modulePopupHandler.OpenBuildInfoPopup(moduleData);
		MenuManager.Instance.buildingCostHandler.SetBuildingCostData(moduleData);
	}

	public void PauseInteractions() {
		_cachedState = CurrentState;
		CurrentState = _states[InteractionState.Disabled];
	}

	public void ResumeInteractions() {
		CurrentState = _cachedState;
	}
}