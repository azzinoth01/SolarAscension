using UnityEngine;

/// <summary>
/// Describes a coordinate within any grid-based volume.
/// </summary>
[System.Serializable]
public struct GridCoordinate : System.IEquatable<GridCoordinate> {
	[SerializeField] private int _x, _y, _z;

	public int x => _x;
	public int y => _y;
	public int z => _z;
		
	public static readonly GridCoordinate Invalid = new(int.MaxValue);
	public static readonly GridCoordinate Zero = new(0, 0, 0);
	public static readonly GridCoordinate Left = new(-1, 0, 0);
	public static readonly GridCoordinate Back = new(0, 0, -1);
	public static readonly GridCoordinate Down = new(0, -1, 0);
	public static readonly GridCoordinate Up = new(0, 1, 0);
	public static readonly GridCoordinate Forward = new(0, 0, 1);
	public static readonly GridCoordinate Right = new(1, 0, 0);

	public static GridCoordinate GetGridVector(int i) {
		return i switch {
			0 => Left,
			1 => Back,
			2 => Down,
			3 => Up,
			4 => Forward,
			5 => Right,
			_ => throw new System.IndexOutOfRangeException()
		};
	}
		
	public GridCoordinate(int x, int y, int z) {
		_x = x;
		_y = y;
		_z = z;
	}
		
	public GridCoordinate(int n) {
		_x = n;
		_y = n;
		_z = n;
	}

	public GridCoordinate RotateX() {
		return new GridCoordinate(_x, -_z, _y);
	}

	public GridCoordinate RotateY() {
		return new GridCoordinate(_z, _y, -_x);
	}

	public GridCoordinate Rotate(GridOrientation o) {
		GridDirection rotatedX = o[GridDirection.Right];
		GridDirection rotatedY = o[GridDirection.Up];
		GridDirection rotatedZ = o[GridDirection.Forward];

		return _x * rotatedX.ToGridVector() + _y * rotatedY.ToGridVector() + _z * rotatedZ.ToGridVector();
	}

	public static GridCoordinate Abs(GridCoordinate c) {
		return new GridCoordinate(Mathf.Abs(c._x), Mathf.Abs(c._y), Mathf.Abs(c._z));
	}

	public static int Distance(GridCoordinate a, GridCoordinate b) {
		return Mathf.Abs(b._x - a._x) + Mathf.Abs(b._y - a._y) + Mathf.Abs(b._z - a._z);
	}

	public static implicit operator Vector3(GridCoordinate vc) => new(vc._x, vc._y, vc._z);
		
	public static GridCoordinate operator +(GridCoordinate lhs, GridCoordinate rhs) {
		return new GridCoordinate(lhs._x + rhs._x, lhs._y + rhs._y, lhs._z + rhs._z);
	}
		
	public static GridCoordinate operator -(GridCoordinate lhs, GridCoordinate rhs) {
		return new GridCoordinate(lhs._x - rhs._x, lhs._y - rhs._y, lhs._z - rhs._z);
	}

	public static GridCoordinate operator *(GridCoordinate lhs, GridCoordinate rhs) {
		return new GridCoordinate(lhs._x * rhs._x, lhs._y * rhs._y, lhs._z * rhs._z);
	}

	public static GridCoordinate operator *(GridCoordinate lhs, int rhs) {
		return new GridCoordinate(lhs._x * rhs, lhs._y * rhs, lhs._z * rhs);
	}
		
	public static GridCoordinate operator *(int lhs, GridCoordinate rhs) {
		return new GridCoordinate(lhs * rhs._x, lhs * rhs._y, lhs * rhs._z);
	}

	public static bool operator ==(GridCoordinate lhs, GridCoordinate rhs) {
		return lhs._x == rhs._x && lhs._y == rhs._y && lhs._z == rhs._z;
	}

	public static bool operator !=(GridCoordinate lhs, GridCoordinate rhs) {
		return lhs._x != rhs._x || lhs._y != rhs._y || lhs._z != rhs._z;
	}

	public bool Equals(GridCoordinate other) {
		return _x == other._x && _y == other._y && _z == other._z;
	}

	public override bool Equals(object obj) {
		return obj is GridCoordinate other && Equals(other);
	}

	public override string ToString() {
		return "VC(" + _x + ", " + _y + ", " + _z + ")";
	}

	public override int GetHashCode() {
		unchecked {
			int hash = 17;
			hash = hash * 92821 + _x.GetHashCode();
			hash = hash * 92821 + _y.GetHashCode();
			hash = hash * 92821 + _z.GetHashCode();
			return hash;
		}
	}
}
	
public static class GridCoordinateExtensions {
	public static LinkDirection ToLinkDirection(this GridCoordinate c) {
		if ( c == GridCoordinate.Left ) {
			return LinkDirection.Left;
		}

		if ( c == GridCoordinate.Back ) {
			return LinkDirection.Back;
		}

		if ( c == GridCoordinate.Down ) {
			return LinkDirection.Down;
		}

		if ( c == GridCoordinate.Up ) {
			return LinkDirection.Up;
		}

		if ( c == GridCoordinate.Forward ) {
			return LinkDirection.Forward;
		}

		if ( c == GridCoordinate.Right ) {
			return LinkDirection.Right;
		}

		return 0;
	}

	public static GridDirection ToVolumeDirection(this GridCoordinate c) {
		if ( c == GridCoordinate.Left ) {
			return GridDirection.Left;
		}

		if ( c == GridCoordinate.Back ) {
			return GridDirection.Back;
		}

		if ( c == GridCoordinate.Down ) {
			return GridDirection.Down;
		}

		if ( c == GridCoordinate.Up ) {
			return GridDirection.Up;
		}

		if ( c == GridCoordinate.Forward ) {
			return GridDirection.Forward;
		}

		if ( c == GridCoordinate.Right ) {
			return GridDirection.Right;
		}

		return 0;
	}

	public static Vector3 ToWorldPositionCentered(this GridCoordinate c) {
		return (Vector3)c * BuildSystem.VolumeScale;
	}
}