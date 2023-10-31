using System.Collections.Generic;

public class GridLinkMap {
	private readonly Dictionary<GridCoordinate, LinkData> _linkMap;

	public GridLinkMap(int initialCapacity) {
		_linkMap = new Dictionary<GridCoordinate, LinkData>(initialCapacity);
	}
		
	public void AddLink(GridCoordinate location, GridDirection direction, GridObject source) {
		if ( _linkMap.TryGetValue(location, out LinkData linkData) ) {
			linkData.AddLink(direction, source);
		} else {
			linkData = new LinkData(direction, source);
			_linkMap.Add(location, linkData);
		}
	}

	public void RemoveLink(GridCoordinate linkLocation, GridDirection direction) {
		if ( _linkMap.TryGetValue(linkLocation, out LinkData linkData) ) {
			linkData.RemoveLink(direction);
		}
	}

	public bool HasLink(GridCoordinate coordinate) {
		if ( _linkMap.TryGetValue(coordinate, out LinkData linkData) ) {
			return linkData.linkDirection != 0;
		}

		return false;
	}

	public bool TryGetModuleAtLocation(GridCoordinate linkLocation, out Module[] modules) {
		if ( _linkMap.TryGetValue(linkLocation, out LinkData linkData) ) {
			modules = linkData.sourceModules;
			return modules != null;
		}

		modules = null;
		return false;
	}

	public LinkData GetLinkData(GridCoordinate location) {
		return _linkMap.TryGetValue(location, out LinkData link) ? link : null;
	}

	public bool TryGetLinkData(GridCoordinate location, out LinkData linkData) {
		return _linkMap.TryGetValue(location, out linkData);
	}
}