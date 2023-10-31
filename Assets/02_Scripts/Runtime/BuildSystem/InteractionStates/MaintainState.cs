public class MaintainState : State {
	public MaintainState(PlayerState state) : base(state) { }

	public override void Enter() {
		stateData.SetFlags(StateFlag.Selected | StateFlag.Hover);
	}
}