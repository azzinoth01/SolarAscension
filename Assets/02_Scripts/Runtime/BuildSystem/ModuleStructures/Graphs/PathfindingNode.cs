using System;

public class PathfindingNode : IHeapItem<PathfindingNode>, IEquatable<PathfindingNode> {
	public readonly GridCoordinate position;
	public PathfindingNode parent;

	public int gCost, hCost;

	private int FCost => gCost + hCost;

	public PathfindingNode(GridCoordinate pos) {
		position = pos;
	}

	public int HeapIndex { get; set; }

	public int CompareTo(PathfindingNode other) {
		int compare = FCost.CompareTo(other.FCost);
			
		if ( compare == 0 ) {
			compare = hCost.CompareTo(other.hCost);
		}

		return -compare;
	}

	public bool Equals(PathfindingNode other) {
		if ( ReferenceEquals(null, other) ) return false;
		if ( ReferenceEquals(this, other) ) return true;
		return position.Equals(other.position);
	}

	public override bool Equals(object obj) {
		if ( ReferenceEquals(null, obj) ) return false;
		if ( ReferenceEquals(this, obj) ) return true;
		if ( obj.GetType() != GetType() ) return false;
		return Equals((PathfindingNode)obj);
	}

	public override int GetHashCode() {
		return position.GetHashCode();
	}
}