public abstract class State {
	protected readonly PlayerState playerState;
	protected readonly PlayerStateData stateData;

	protected State(PlayerState state) {
		playerState = state;
		stateData = state.StateData;
	}

	public virtual void Enter() { }

	public virtual void ProcessInput(PlayerInput input) { }

	public virtual void Exit(InteractionState exitState) {
		stateData.ResetFlags();
		playerState.ChangeState(exitState);
	}
}