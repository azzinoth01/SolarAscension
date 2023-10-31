using System;
using UnityEngine;

public static class VectorExtensions {
	public static GridCoordinate ToGridCoordinate(this Vector3 worldPosition) {
		Vector3 positionRelativeToOrigin = worldPosition;
		Span<int> coords = stackalloc int[3];
			
		for (int i = 0; i < 3; i++) {
			float coordinate = positionRelativeToOrigin[i];
			coordinate /= BuildSystem.VolumeScale;
			coords[i] = Mathf.RoundToInt(coordinate);
		}
			
		return new GridCoordinate(coords[0], coords[1], coords[2]);
	}
}