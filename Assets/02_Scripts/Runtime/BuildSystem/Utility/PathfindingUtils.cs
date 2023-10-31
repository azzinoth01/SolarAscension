using UnityEngine;

public static class PathfindingUtils {
	public static GridCoordinate GetPathfindingDirection(int i, Vector3 planeNormal, LinkDirection blockedDirections) {
		float rDot = Vector3.Dot(planeNormal, Vector3.right);
		float fDot = Vector3.Dot(planeNormal, Vector3.forward);

		if ( Mathf.Abs(rDot) > 0.9f ) {
			if ( (blockedDirections & (LinkDirection.Forward | LinkDirection.Back)) != 0 ) {
				return i switch {
					0 => GridCoordinate.Forward,
					1 => GridCoordinate.Back,
					2 => GridCoordinate.Up,
					3 => GridCoordinate.Down,
					_ => throw new System.IndexOutOfRangeException()
				};
			}

			return i switch {
				0 => GridCoordinate.Up,
				1 => GridCoordinate.Down,
				2 => GridCoordinate.Forward,
				3 => GridCoordinate.Back,
				_ => throw new System.IndexOutOfRangeException()
			};
		}

		if ( Mathf.Abs(fDot) > 0.9f ) {
			if ( (blockedDirections & (LinkDirection.Left | LinkDirection.Right)) != 0 ) {
				return i switch {
					0 => GridCoordinate.Left,
					1 => GridCoordinate.Right,
					2 => GridCoordinate.Up,
					3 => GridCoordinate.Down,
					_ => throw new System.IndexOutOfRangeException()
				};
			}
				
			return i switch {
				0 => GridCoordinate.Up,
				1 => GridCoordinate.Down,
				2 => GridCoordinate.Left,
				3 => GridCoordinate.Right,
				_ => throw new System.IndexOutOfRangeException()
			};
		}
			
		if ( (blockedDirections & (LinkDirection.Forward | LinkDirection.Back)) != 0 ) {
			return i switch {
				0 => GridCoordinate.Forward,
				1 => GridCoordinate.Back,
				2 => GridCoordinate.Left,
				3 => GridCoordinate.Right,
				_ => throw new System.IndexOutOfRangeException()
			};
		}

		return i switch {
			0 => GridCoordinate.Left,
			1 => GridCoordinate.Right,
			2 => GridCoordinate.Forward,
			3 => GridCoordinate.Back,
			_ => throw new System.IndexOutOfRangeException()
		};
	}
}