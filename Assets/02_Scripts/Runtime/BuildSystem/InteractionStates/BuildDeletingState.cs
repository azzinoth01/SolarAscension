using System.Collections.Generic;
using SolarAscension;

public class BuildDeletingState : BuildState {
	public BuildDeletingState(PlayerState state, RotationMarkerVisualizer markerVisualizer, BuildingGrid buildingGrid) : base(state, markerVisualizer, buildingGrid) { }
	
	public static readonly List<GridObject> ObjectsToDelete = new(20);
	
	public override void Enter() {
		stateData.SetFlags(StateFlag.Selected | StateFlag.Hover | StateFlag.Deleting);
		stateData.selectedGridBehaviour = null;
		ObjectsToDelete.Clear();
	}

	public override void ProcessInput(PlayerInput input) {
		if ( input.IsCancelling() ) {
			Exit(InteractionState.Maintain);
		}
			
		if ( input.IsSelecting() ) {
			if ( input.IsCursorOverGridObject(out GridBehaviour hitBehaviour) ) {
				GridObject gridObject = hitBehaviour.GridObject;

				if ( ObjectsToDelete.Contains(gridObject) ) {
					ObjectsToDelete.Remove(gridObject);

					for (int i = ObjectsToDelete.Count - 1; i >= 0; i--) {
						GridObject objectToValidate = ObjectsToDelete[i];
						if ( !ValidateMoveDelete(objectToValidate) ) {
							ObjectsToDelete.RemoveAt(i);
						}
					}

					return;
				} 
					
				ObjectsToDelete.Add(gridObject);
					
				if ( !ValidateMoveDelete(gridObject) ) {
					ObjectsToDelete.Remove(gridObject);
					AudioManager.Instance.Play("action_moduleBuildNP");
					MenuManager.Instance.warningDisplayHandler.SetWarningDisplay("deletionNotPossible", WarningType.Warning);
				} else {
					AudioManager.Instance.Play("action_moduleDemSelect");
				}
			}
		}
	}

	public void PerformDeletion() {
		foreach (GridObject gridObject in ObjectsToDelete) {
			GridBehaviour gridBehaviour = gridObject.gridBehaviour;
			if ( stateData.lastHoveredGridBehaviour == gridBehaviour ) {
				stateData.lastHoveredGridBehaviour = null;
			}

			if ( stateData.hoveredGridBehaviour == gridBehaviour ) {
				stateData.hoveredGridBehaviour = null;
			}

			if ( gridObject is ResourceDistributor distributor ) {
				grid.BalanceInfo.startRedistributionEvent -= distributor.BeginDistribution;
			}

			stateData.selectedGridBehaviour = null;
			gridObject.TearDown();
		}

		Exit(InteractionState.Maintain);
		AudioManager.Instance.Play("action_moduleDemExecute");
		WarningDisplayHandler.Root.SetActive(false);
	}

	private bool ValidateMoveDelete(GridObject hitObject) {
		return hitObject.objectType != GridObjectType.MainModule && hitObject.ValidateTearDown(ObjectsToDelete);
	}
}