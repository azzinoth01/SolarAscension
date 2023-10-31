public class BuildingGrid {
	private const int TubeMapCapacity = 8192;
	private const int LinkMapCapacity = 4096;
	private const int StabilizerMapCapacity = 2048;
	private const int BufferMapCapacity = 16348;
		
	public string PlayerID { get; }
	public PlayerState PlayerState { get; private set; }
	public ObjectLockGrid LockGrid { get; }
	public GridLinkMap LinkMap { get; }
	public GridTubeMap TubeMap { get; }
	public GridStabilizerMap StabilizerMap { get; }
	public GridBufferMap BufferMap { get; }
	public BuildRequestValidator BuildRequestValidator { get; }
	public PlayerBilanzInfo BalanceInfo { get; }

	public BuildingGrid(PlayerBilanzInfo balance, GridObject startObject) {
		PlayerID = balance.PlayerID;
		BalanceInfo = balance;
		startObject.grid = this;
			
		LockGrid = new ObjectLockGrid();
		LinkMap = new GridLinkMap(LinkMapCapacity);
		TubeMap = new GridTubeMap(TubeMapCapacity);
		StabilizerMap = new GridStabilizerMap(StabilizerMapCapacity, TubeMap);
		BufferMap = new GridBufferMap(BufferMapCapacity);
		BuildRequestValidator = new BuildRequestValidator(this);
	}

	public void SetPlayerState(PlayerState state) {
		PlayerState = state;
	}

	public BuildRequestResponse ValidateBuildRequestFromLink(ModuleData data, bool enactOnValidation,
		GridCoordinate linkLocation, GridOrientation orientation) {
		return BuildRequestValidator.ValidateBuildRequestFromLink(data, enactOnValidation, linkLocation, orientation);
	}

	public BuildRequestResponse ValidateBuildRequestFromOrigin(ModuleData data, bool enactOnValidation, GridCoordinate origin, GridOrientation orientation) {
		return BuildRequestValidator.ValidateBuildRequestFromOrigin(data, enactOnValidation, origin, orientation);
	}
		
	public BuildRequestResponse ValidateMoveRequestFromLink(Module module, bool enactOnValidation, GridCoordinate linkLocation,
		GridOrientation orientation) {
		return BuildRequestValidator.ValidateMoveRequestFromLink(module, enactOnValidation, linkLocation, orientation);
	}

	public BuildRequestResponse ValidateMoveRequestFromOrigin(Module module, bool enactOnValidation, GridCoordinate origin,
		GridOrientation orientation) {
		return BuildRequestValidator.ValidateMoveRequestFromOrigin(module, enactOnValidation, origin, orientation);
	}

	public bool LocationIsFree(GridCoordinate location) {
		return !LockGrid.IsLocked(location) && !StabilizerMap.IsLocked(location) && !BufferMap.LocationIsBlocked(location);
	}
}