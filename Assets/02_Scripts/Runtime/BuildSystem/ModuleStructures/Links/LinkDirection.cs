using System;

[Flags]
public enum LinkDirection {
	Left = 1, 
	Back = 2, 
	Down = 4, 
	Up = 8, 
	Forward = 16, 
	Right = 32,
	All = -1
}
	
public static class LinkDirectionExtensions {
	public static LinkDirection Rotate(this LinkDirection c, GridOrientation o) {
		if ( c == LinkDirection.All ) {
			return c;
		}
			
		int output = default;
		int connection = (int)c;
			
		for (GridDirection dir = GridDirection.Left; dir <= GridDirection.Right; dir++) {
			int mask = 1 << (int)dir;
				
			if ( (connection & mask) != 0 ) {
				GridDirection rotatedDir = o[dir];
				output |= 1 << (int)rotatedDir;
			}
		}

		return (LinkDirection)output;
	}

	public static LinkDirection Flip(this LinkDirection c) {
		int output = 0;
		int linkDirection = (int)c;
		for (int i = 0; i < 6; i++) {
			if ( (linkDirection & 1 << i) != 0 ) {
				GridDirection dir = (GridDirection)i;
				GridDirection flippedDir = dir.Flip();
				output |= 1 << (int)flippedDir;
			}
		}
		return (LinkDirection)output;
	}

	public static GridDirection ToGridDirection(this LinkDirection c) {
		int dirAsInt = (int)c;
		for (int i = 0; i < 6; i++) {
			if ( dirAsInt >> i == 1 ) {
				return (GridDirection)i;
			}
		}

		return 0;
	}

	public static GridCoordinate ToGridVector(this LinkDirection c) {
		int dirAsInt = (int)c;
		for (int i = 0; i < 6; i++) {
			if ( dirAsInt >> i == 1 ) {
				return GridCoordinate.GetGridVector(i);
			}
		}

		return new GridCoordinate(0);
	}

	public static int ToGridVectorsAll(this LinkDirection c, ref Span<GridCoordinate> offsets) {
		int dirAsInt = (int)c;
		int dirCount = 0;
			
		for (int i = 0; i < 6; i++) {
			if ( (dirAsInt & 1 << i) != 0 ) {
				offsets[dirCount++] = GridCoordinate.GetGridVector(i);
			}
		}

		return dirCount;
	}
		
	public static int ToGridDirectionsAll(this LinkDirection c, ref Span<GridDirection> directions) {
		int dirAsInt = (int)c;
		int dirCount = 0;
			
		for (int i = 0; i < 6; i++) {
			if ( (dirAsInt & 1 << i) != 0 ) {
				directions[dirCount++] = (GridDirection)i;
			}
		}

		return dirCount;
	}
}