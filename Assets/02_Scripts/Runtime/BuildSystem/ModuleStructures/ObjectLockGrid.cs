using System.Collections.Generic;

/// <summary>
/// The lock grid contains a hashset of coordinates where an object is currently filling the volume.
/// </summary>
public class ObjectLockGrid {
	private const int InitialCapacity = 16348; // 2^14
		
	private readonly HashSet<GridCoordinate> _lockedCoordinates;

	public ObjectLockGrid() {
		_lockedCoordinates = new HashSet<GridCoordinate>(InitialCapacity);
	}

	public void AddLock(GridCoordinate coordinate) {
		_lockedCoordinates.Add(coordinate);
	}

	public void RemoveLock(GridCoordinate coordinate) {
		_lockedCoordinates.Remove(coordinate);
	}

	public bool IsLocked(GridCoordinate coordinate) {
		return _lockedCoordinates.Contains(coordinate);
	}
}