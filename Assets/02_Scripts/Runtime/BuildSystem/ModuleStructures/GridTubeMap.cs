using System.Collections.Generic;

public class GridTubeMap {
	private readonly Dictionary<GridCoordinate, Tube> _tubeMap;

	public GridTubeMap(int initialCapacity) {
		_tubeMap = new Dictionary<GridCoordinate, Tube>(initialCapacity);
	}

	public void AddTube(GridCoordinate location, Tube tube) {
		_tubeMap.Add(location, tube);
	}

	public bool HasTubeAtLocation(GridCoordinate location) {
		return _tubeMap.ContainsKey(location);
	}

	public Tube GetTubeAtLocation(GridCoordinate location) {
		if ( _tubeMap.TryGetValue(location, out Tube tube) ) {
			return tube;
		}

		return null;
	}

	public void RemoveTube(GridCoordinate location) {
		_tubeMap.Remove(location);
	}
}