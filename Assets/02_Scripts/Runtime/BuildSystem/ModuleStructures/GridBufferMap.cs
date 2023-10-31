using System.Collections.Generic;

public class GridBufferMap {
	private readonly Dictionary<GridCoordinate, byte> _bufferMap;

	public GridBufferMap(int initialCapacity) {
		_bufferMap = new Dictionary<GridCoordinate, byte>(initialCapacity);
	}

	public bool LocationIsBlocked(GridCoordinate location) {
		return _bufferMap.TryGetValue(location, out byte bufferCount) && bufferCount > 0;
	}

	public void AddBuffer(GridCoordinate location) {
		if ( _bufferMap.TryGetValue(location, out byte bufferCount) ) {
			_bufferMap[location] = ++bufferCount;
		} else {
			_bufferMap.Add(location, 1);
		}

		if ( !BlockHandler.BlockedCoordinates.Contains(location) ) {
			BlockHandler.BlockedCoordinates.Add(location);
		}
	}

	public void RemoveBuffer(GridCoordinate location) {
		if ( _bufferMap.TryGetValue(location, out byte bufferCount) ) {
			if ( bufferCount > 0 ) {
				_bufferMap[location] = --bufferCount;
			}
		}

		if ( !LocationIsBlocked(location) ) {
			BlockHandler.CheckBlockStatus(location);
		}
	}

	public bool DebugTryGetBufferCount(GridCoordinate location, out byte count) {
		return _bufferMap.TryGetValue(location, out count);
	}
}