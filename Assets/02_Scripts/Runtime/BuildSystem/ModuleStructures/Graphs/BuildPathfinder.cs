using System.Collections.Generic;
using UnityEngine;

public class BuildPathfinder {
	private const int MaxPathLength = 30;

	private BuildingGrid _grid;
	private GridCoordinate _startingCoordinate = GridCoordinate.Invalid;

	public bool Enabled { get; private set; }
	public bool HasPath => Path.Count > 0;
	public List<GridCoordinate> Path { get; } = new(MaxPathLength);
		
	public BuildPathfinder(BuildingGrid grid) {
		_grid = grid;
	}

	public void Disable() {
		if ( HasPath ) {
			Path.Clear();
		}

		Enabled = false;
		_startingCoordinate = GridCoordinate.Invalid;
	}

	public void SetStartingCoordinate(GridCoordinate startCoord) {
		_startingCoordinate = startCoord;
		Enabled = true;
	}

	public void FindPath(GridCoordinate endCoord) {
		if ( !Enabled ) {
			return;
		}
			
		Path.Clear();

		GridCoordinate current = _startingCoordinate;
		GridCoordinate delta = endCoord - _startingCoordinate;
		GridCoordinate target = DetermineTargetFromDelta(delta, out GridCoordinate targetDirection);

		int stepsToTarget = Mathf.Min(GridCoordinate.Distance(_startingCoordinate, target), MaxPathLength);

		for (int i = 0; i <= stepsToTarget; i++) {
			if ( _grid.LocationIsFree(current) ) {
				Path.Add(current);
			} else {
				break;
			}

			current += targetDirection;
		}
	}

	private GridCoordinate DetermineTargetFromDelta(GridCoordinate delta, out GridCoordinate directionToTarget) {
		GridCoordinate deltaAbs = GridCoordinate.Abs(delta);
		if ( deltaAbs.x > deltaAbs.y ) {
			if ( deltaAbs.x > deltaAbs.z ) {
				directionToTarget = delta.x > 0 ? GridCoordinate.Right : GridCoordinate.Left;
				return _startingCoordinate + new GridCoordinate(delta.x, 0, 0);
			}

			directionToTarget = delta.z > 0 ? GridCoordinate.Forward : GridCoordinate.Back;
			return _startingCoordinate + new GridCoordinate(0, 0, delta.z);
		}

		if ( deltaAbs.y > deltaAbs.z ) {
			directionToTarget = delta.y > 0 ? GridCoordinate.Up : GridCoordinate.Down;
			return _startingCoordinate + new GridCoordinate(0, delta.y, 0);
		}

		directionToTarget = delta.z > 0 ? GridCoordinate.Forward : GridCoordinate.Back;
		return _startingCoordinate + new GridCoordinate(0, 0, delta.z);
	}
}