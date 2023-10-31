using System.Collections.Generic;

public class GridStabilizerMap {
	private static readonly GridCoordinate[,] StabilizerNeighborOffsets = {
		{ GridCoordinate.Left, GridCoordinate.Forward },
		{ GridCoordinate.Right, GridCoordinate.Forward },
		{ GridCoordinate.Left, GridCoordinate.Back },
		{ GridCoordinate.Right, GridCoordinate.Back },
		{ GridCoordinate.Up, GridCoordinate.Forward },
		{ GridCoordinate.Down, GridCoordinate.Forward },
		{ GridCoordinate.Down, GridCoordinate.Back },
		{ GridCoordinate.Up, GridCoordinate.Back },
		{ GridCoordinate.Left, GridCoordinate.Up },
		{ GridCoordinate.Right, GridCoordinate.Up },
		{ GridCoordinate.Left, GridCoordinate.Down },
		{ GridCoordinate.Right, GridCoordinate.Down }
	};
		
	private static readonly GridCoordinate[] StabilizerOffsets = {
		new(-1, 0, 1),
		new(1, 0, 1), 
		new(-1, 0, -1),
		new(1, 0, -1),
		new(0, 1, 1), 
		new(0, -1, 1),
		new(0, -1, -1),
		new(0, 1, -1),
		new(-1, 1, 0),
		new(1, 1, 0), 
		new(-1, -1, 0),
		new(1, -1, 0),
	};

	private readonly Dictionary<GridCoordinate, Stabilizer> _stabilizerMap;
	private readonly GridTubeMap _tubeMap;

	public GridStabilizerMap(int initialCapacity, GridTubeMap tubeMap) {
		_stabilizerMap = new Dictionary<GridCoordinate, Stabilizer>(initialCapacity);
		_tubeMap = tubeMap;
	}

	public void CheckForNewStabilizers(GridCoordinate linkOrigin) {
		for (int i = 0; i < 12; i++) {
			if ( _tubeMap.HasTubeAtLocation(linkOrigin + StabilizerNeighborOffsets[i, 0]) && _tubeMap.HasTubeAtLocation(linkOrigin + StabilizerNeighborOffsets[i, 1]) ) {
				GridCoordinate stabilizerLocation = linkOrigin + StabilizerOffsets[i];
					
				if ( _stabilizerMap.TryGetValue(stabilizerLocation, out Stabilizer stabilizer) ) {
					stabilizer.AddConnectingLocation(i);

					if ( !BlockHandler.BlockedCoordinates.Contains(stabilizerLocation) ) {
						BlockHandler.BlockedCoordinates.Add(stabilizerLocation);
					}
				} else {
					stabilizer = new Stabilizer();
					stabilizer.AddConnectingLocation(i); 
					_stabilizerMap.Add(stabilizerLocation, stabilizer); 
					BlockHandler.BlockedCoordinates.Add(stabilizerLocation);
				}
			}
		}
	}

	public void RemoveStabilizers(GridCoordinate linkOrigin, GridCoordinate removedTubeLocation) {
		GridCoordinate delta = removedTubeLocation - linkOrigin;
			
		for (int i = 0; i < 12; i++) {
			if ( delta != StabilizerNeighborOffsets[i, 0] && delta != StabilizerNeighborOffsets[i, 1] ) {
				continue;
			}
				
			if ( !_stabilizerMap.TryGetValue(linkOrigin + StabilizerOffsets[i], out Stabilizer stabilizer) ) {
				return;
			}
				
			if ( !stabilizer.HasConnectingLocation(i) ) {
				continue;
			}
				
			stabilizer.RemoveConnectingLocation(i);

			if ( !stabilizer.IsActive ) {
				BlockHandler.CheckBlockStatus(linkOrigin + StabilizerOffsets[i]);
			}
		}
	}

	public bool IsLocked(GridCoordinate location) {
		return _stabilizerMap.TryGetValue(location, out Stabilizer stabilizer) && stabilizer.IsActive;
	}

	public bool DebugTryGetStabilizer(GridCoordinate location, out Stabilizer stabilizer) {
		return _stabilizerMap.TryGetValue(location, out stabilizer);
	}
}