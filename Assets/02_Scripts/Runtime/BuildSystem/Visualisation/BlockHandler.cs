using System.Collections.Generic;

public static class BlockHandler {
	public static readonly List<GridCoordinate> BlockedCoordinates = new(1 << 16);

	private static BuildingGrid _grid;

	public static void SetGrid(BuildingGrid grid) {
		_grid = grid;
	}

	public static void CheckBlockStatus(GridCoordinate coordinate) {
		if ( _grid.LocationIsFree(coordinate) ) {
			BlockedCoordinates.Remove(coordinate);
		}
	}
}
