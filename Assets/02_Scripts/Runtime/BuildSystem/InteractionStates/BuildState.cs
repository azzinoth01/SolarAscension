using SolarAscension;

public abstract class BuildState : State {
	protected readonly BuildingGrid grid;
	protected readonly RotationMarkerVisualizer rotationVisualizer;

	protected GridOrientation buildRotation;

	protected BuildState(PlayerState state, RotationMarkerVisualizer markerVisualizer, BuildingGrid buildingGrid) : base(state) {
		grid = buildingGrid;
		rotationVisualizer = markerVisualizer;
	}
		
	protected GridOrientation UpdateBuildRotation(PlayerInput input) {
		if ( input.IsRotateHorizontal() ) {
			AudioManager.Instance.Play("action_moduleTurn");
			return buildRotation.RotateHorizontal();
		}

		if ( input.IsRotateVertical() ) {
			AudioManager.Instance.Play("action_moduleTurn");
			return buildRotation.RotateVertical();
		}

		if ( input.IsRotateForward() ) {
			AudioManager.Instance.Play("action_moduleTurn");
			return buildRotation.RotateForward();
		}

		return buildRotation;
	}
}