public enum RequestResponse {
	None,
	Valid,
	MissingLink,
	IncompatibleLink,
	BlockedSpace,
	MissingResources,
	CantBeMoved,
	CantBeDeleted
}

public readonly struct BuildRequestResponse {
	public readonly RequestResponse response;
	public readonly GridCoordinate linkCenterCoordinate;
	public readonly GridCoordinate buildDirection;

	public BuildRequestResponse(RequestResponse resp, GridCoordinate linkCenter, GridCoordinate buildOffset) {
		response = resp;
		linkCenterCoordinate = linkCenter;
		buildDirection = buildOffset;
	}
}