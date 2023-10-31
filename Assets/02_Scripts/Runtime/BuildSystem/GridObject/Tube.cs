using System;
using System.Collections.Generic;

[Serializable]
public class Tube : GridObject {
	[NonSerialized]
	public TubeCluster cluster;

	private LinkDirection _connections;

	public LinkDirection Connections {
		get => _connections;
		private set {
			_connections = value;
			gridBehaviour.UpdateTubeMeshAndRotation(_connections);
		}
	}

	public void Setup(Building building, ModuleData data, GridCoordinate location, LinkDirection linkDirection) {
		base.Setup(building, data, location, GridOrientation.Identity);
		Connections = linkDirection.Flip();
			
		Span<GridCoordinate> offsets = stackalloc GridCoordinate[6];
		int originCount = Connections.ToGridVectorsAll(ref offsets);
			
		for (int i = 0; i < originCount; i++) {
			grid.StabilizerMap.CheckForNewStabilizers(location + offsets[i]);
		}
	}

	public void AddModuleConnection(GridDirection direction) {
		Connections |= direction.ToLinkDirection().Flip();
	}

	protected override void ProcessLinks() {
		for (int i = 0; i < 6; i++) {
			GridCoordinate linkLocation = linkLocations[i];

			if ( linkLocation == GridCoordinate.Invalid ) {
				continue;
			}

			if ( grid.TubeMap.HasTubeAtLocation(linkLocation) ) {
				Tube neighbor = grid.TubeMap.GetTubeAtLocation(linkLocation);
				neighbor.HandleTubeConnection(this);
			}
		}

		cluster ??= new TubeCluster(this);
			
		if ( grid.LinkMap.TryGetModuleAtLocation(origin, out Module[] modules) ) {
			foreach (Module module in modules) {
				module?.HandleConnection(this);
				// cluster.AddModule(module);
			}
		}
			
		grid.TubeMap.AddTube(origin, this);
	}

	protected override void SetLocks() {
		grid.LockGrid.AddLock(origin);
	}

	public void HandleTubeConnection(Tube neighbor) {
		GridCoordinate delta = origin - neighbor.origin;
		LinkDirection neighborDirection = delta.ToLinkDirection();

		AddConnectionDirection(neighborDirection, neighbor);

		if ( neighbor.cluster == null ) {
			cluster.AddTube(neighbor);
		} else {
			if ( cluster != neighbor.cluster ) {
				cluster.Merge(neighbor.cluster);
			}
		}
	}

	private void RemoveTubeConnection(GridDirection direction) {
		if ( isTearingDown ) {
			return;
		}
		RemoveConnectionDirection(direction.ToLinkDirection());
	}

	private void AddConnectionDirection(LinkDirection direction, Tube neighbor) {
		Connections |= direction.Flip();
		neighbor.Connections |= direction;
	}

	private void RemoveConnectionDirection(LinkDirection direction) {
		Connections ^= direction.Flip();
	}

	protected override void RemoveLinks() {
		for (int i = 0; i < linkDirections.Length; i++) {
			GridCoordinate linkLocation = linkLocations[i];
			GridDirection direction = linkDirections[i];

			grid.LinkMap.RemoveLink(linkLocation, direction);
				
			if ( grid.TubeMap.HasTubeAtLocation(linkLocation) ) {
				Tube neighbor = grid.TubeMap.GetTubeAtLocation(linkLocation);
				neighbor.RemoveTubeConnection(direction);
			}
		}

		grid.TubeMap.RemoveTube(origin);
		cluster.RemoveTube(this);
	}

	protected override void RemoveLocks() {
		grid.LockGrid.RemoveLock(origin);
		RemoveStabilizers();
	}

	public override bool ValidateTearDown(List<GridObject> objectsToExclude) {
		Span<GridCoordinate> offsets = stackalloc GridCoordinate[6];
			
		int connectionCount = _connections.ToGridVectorsAll(ref offsets);
		for (int i = 0; i < connectionCount; i++) {
			if ( grid.TubeMap.HasTubeAtLocation(origin + offsets[i]) ) {
				Tube neighbor = grid.TubeMap.GetTubeAtLocation(origin + offsets[i]);

				if ( objectsToExclude.Contains(neighbor) ) {
					continue;
				}
					
				if ( IntegrityChecker.TryFindConnectionToModule(neighbor, this, objectsToExclude) ) {
					continue;
				}

				return false;
			}
		}
			
		return cluster.ValidateModuleIntegrityExcludingObjects(objectsToExclude);
	}

	private void RemoveStabilizers() {
		Span<GridCoordinate> offsets = stackalloc GridCoordinate[6];
			
		int connectionCount = _connections.ToGridVectorsAll(ref offsets);
		for (int i = 0; i < connectionCount; i++) {
			grid.StabilizerMap.RemoveStabilizers(origin + offsets[i], origin);
		}
	}
}