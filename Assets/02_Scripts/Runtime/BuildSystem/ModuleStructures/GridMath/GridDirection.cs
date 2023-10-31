using UnityEngine;

public enum GridDirection {
	Left,
	Back,
	Down,
	Up,
	Forward,
	Right
}
	
public static class GridDirectionExtensions {
	public static LinkDirection ToLinkDirection(this GridDirection d) {
		return (LinkDirection)(1 << (int)d);
	}
		
	public static GridDirection Flip(this GridDirection d) {
		int id = (int)d - 5;
		return (GridDirection)(((id >> 31) | 1) * id); // = Mathf.Abs // didn't want to import Mathf just for this.
	}

	public static GridCoordinate ToGridVector(this GridDirection d) {
		return d switch {
			GridDirection.Left => GridCoordinate.Left,
			GridDirection.Back => GridCoordinate.Back,
			GridDirection.Down => GridCoordinate.Down,
			GridDirection.Up => GridCoordinate.Up,
			GridDirection.Forward => GridCoordinate.Forward,
			GridDirection.Right => GridCoordinate.Right,
			_ => new GridCoordinate(0, 0, 0)
		};
	}

	public static Vector3 ToWorldVector(this GridDirection d) {
		return d switch {
			GridDirection.Left => Vector3.left,
			GridDirection.Back => Vector3.back,
			GridDirection.Down => Vector3.down,
			GridDirection.Up => Vector3.up,
			GridDirection.Forward => Vector3.forward,
			GridDirection.Right => Vector3.right,
			_ => Vector3.zero
		};
	}
}