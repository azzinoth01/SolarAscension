using System;

public class IntegrityNode : IHeapItem<IntegrityNode>, IEquatable<IntegrityNode> {
	public readonly GridObject source;

	public int gCost, hCost;
	private int FCost => gCost + hCost;
		
	public IntegrityNode(GridObject sourceObject) {
		source = sourceObject;
	}
		
	public int HeapIndex { get; set; }

	public int CompareTo(IntegrityNode other) {
		int compare = FCost.CompareTo(other.FCost);
		return -compare;
	}

	public bool Equals(IntegrityNode other) {
		if ( ReferenceEquals(null, other) ) return false;
		if ( ReferenceEquals(this, other) ) return true;
		return source.Equals(other.source);
	}

	public override bool Equals(object obj) {
		if ( ReferenceEquals(null, obj) ) return false;
		if ( ReferenceEquals(this, obj) ) return true;
		if ( obj.GetType() != GetType() ) return false;
		return Equals((IntegrityNode)obj);
	}

	public override int GetHashCode() {
		return source.origin.GetHashCode();
	}
}