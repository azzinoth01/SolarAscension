using UnityEngine;

[System.Serializable]
public struct GridOrientation {
	[SerializeField] private GridDirection _x, _y, _z;

	public Vector3 Up => _y.ToWorldVector();
		
	public static GridOrientation Identity => new(GridDirection.Right, GridDirection.Up, GridDirection.Forward);
		
	public GridOrientation(GridDirection x, GridDirection y, GridDirection z) {
		_x = x;
		_y = y;
		_z = z;
	}
		
	/// <summary>
	/// Indexer returns what the base direction is rotated by this orientation.
	/// </summary>
	/// <param name="baseDirection"></param>
	public GridDirection this[GridDirection baseDirection] {
		get {
			return baseDirection switch {
				GridDirection.Right => _x,
				GridDirection.Up => _y,
				GridDirection.Forward => _z,
				GridDirection.Left => _x.Flip(),
				GridDirection.Down => _y.Flip(),
				GridDirection.Back => _z.Flip(),
				_ => 0
			};
		}
	}

	public static GridOrientation FromUpDirection(GridDirection upDirection) {
		return upDirection switch {
			GridDirection.Forward => new GridOrientation(GridDirection.Left, GridDirection.Forward, GridDirection.Up),
			GridDirection.Down => new GridOrientation(GridDirection.Right, GridDirection.Down, GridDirection.Back),
			GridDirection.Back => new GridOrientation(GridDirection.Left, GridDirection.Back, GridDirection.Down),
			GridDirection.Right => new GridOrientation(GridDirection.Back, GridDirection.Right, GridDirection.Down),
			GridDirection.Left => new GridOrientation(GridDirection.Back, GridDirection.Left, GridDirection.Up),
			_ => Identity
		};
	}

	public GridOrientation RotateVertical() {
		return new GridOrientation(_x, _z, _y.Flip());
	}

	public GridOrientation RotateHorizontal() {
		return new GridOrientation(_z.Flip(), _y, _x);
	}

	public GridOrientation RotateForward() {
		return new GridOrientation(_y, _x.Flip(), _z);
	}

	public static implicit operator Quaternion(GridOrientation o) {
		return Quaternion.LookRotation(o._z.ToWorldVector(), o._y.ToWorldVector());
	}

	public static bool operator ==(GridOrientation a, GridOrientation b) {
		return a._x == b._x && a._y == b._y && a._z == b._z;
	}
		
	public static bool operator !=(GridOrientation a, GridOrientation b) {
		return a._x != b._x || a._y != b._y || a._z != b._z;
	}

	public override bool Equals(object obj) {
		if ( obj is GridOrientation o ) {
			return this == o;
		}

		return false;
	}

	public override int GetHashCode() {
		unchecked {
			int x = (int)_x;
			int y = (int)_y;
			int z = (int)_z;
				
			int hash = 17;
			hash = hash * 92821 + x.GetHashCode();
			hash = hash * 92821 + y.GetHashCode();
			hash = hash * 92821 + z.GetHashCode();
			return hash;
		}
	}

	public override string ToString() {
		return "Right: " + _x + ", Up: " + _y + ", Forward:" + _z;
	}
}