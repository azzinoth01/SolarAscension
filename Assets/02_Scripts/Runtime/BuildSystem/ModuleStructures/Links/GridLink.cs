using System;

[System.Serializable]
public struct GridLink {
	public GridCoordinate localCoordinate;
	public LinkDirection connectorDirection;

	public GridLink(GridCoordinate coordinate, LinkDirection direction) {
		localCoordinate = coordinate;
		connectorDirection = direction;
	}

	public GridLink Rotate(GridOrientation o) {
		return new GridLink(localCoordinate.Rotate(o), connectorDirection.Rotate(o));
	}
}
	
public static class GridLinkerExtensions {
	public static bool TryLink(this GridLink link, LinkDirection anchor) {
		return (anchor & link.connectorDirection.Flip()) != 0;
	}

	public static int InnerLinksToWorld(this GridLink[] links, ref Span<GridLink> worldLinks, GridOrientation orientation, GridCoordinate origin) {
		Span<GridDirection> directions = stackalloc GridDirection[6];
		int linkerCount = 0;

		foreach (GridLink linker in links) {
			GridLink rotatedLink = linker.Rotate(orientation);

			int linkCount = rotatedLink.connectorDirection.ToGridDirectionsAll(ref directions);
			for (int i = 0; i < linkCount; i++) {
				GridCoordinate worldLocation = origin + rotatedLink.localCoordinate;
				worldLinks[linkerCount++] = new GridLink(worldLocation, directions[i].ToLinkDirection());
			}
		}

		return linkerCount;
	}
		
	public static int OuterLinksToWorld(this GridLink[] links, ref Span<GridLink> worldLinks, GridOrientation orientation, GridCoordinate origin) {
		Span<GridDirection> directions = stackalloc GridDirection[6];
		int linkerCount = 0;

		foreach (GridLink linker in links) {
			GridLink rotatedLink = linker.Rotate(orientation);

			int linkCount = rotatedLink.connectorDirection.ToGridDirectionsAll(ref directions);
			for (int i = 0; i < linkCount; i++) {
				GridCoordinate worldLocation = origin + rotatedLink.localCoordinate + directions[i].ToGridVector();
				worldLinks[linkerCount++] = new GridLink(worldLocation, directions[i].ToLinkDirection());
			}
		}

		return linkerCount;
	}

	public static bool Contains(this Span<GridLink> worldLinks, GridCoordinate worldCoordinate) {
		for (int i = 0; i < worldLinks.Length; i++) {
			if ( worldLinks[i].localCoordinate == worldCoordinate ) {
				return true;
			}
		}

		return false;
	}
}