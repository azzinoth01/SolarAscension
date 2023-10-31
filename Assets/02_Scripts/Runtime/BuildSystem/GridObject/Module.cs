using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Module : GridObject {
	[NonSerialized]
	public TubeCluster[] clusters;

	public void ReceiveDistribution(ResourceDistributor source, RessourcesValue value, uint distributionId) {
		if ( distributionId != lastDistributionId ) {
			distributorsInLastReceivedDistribution.Clear();
		}
			
		lastReceivedDistributionId = distributionId;
		distributorsInLastReceivedDistribution.Add(source);
			
		economyBuilding.InputValue = new RessourceDistributionStruct {
			Value = value,
			ID = distributionId
		};
	}

	public void ContinueDistribution(ResourceDistributor sourceDistributor, TubeCluster sourceCluster, RessourcesValue value, uint distributionId) {
		foreach (TubeCluster cluster in clusters) {
			if ( cluster == sourceCluster ) {
				continue;
			}
				
			cluster?.HandleResourceDistribution(sourceDistributor, this, value, distributionId);
		}
	}

	protected override void ProcessLinks() {
		for (int i = 0; i < 6; i++) {
			GridCoordinate linkLocation = linkLocations[i];

			if ( linkLocation == GridCoordinate.Invalid ) {
				continue;
			}

			if ( grid.TubeMap.HasTubeAtLocation(linkLocation) ) {
				Tube anchor = grid.TubeMap.GetTubeAtLocation(linkLocation);
				HandleConnection(anchor);

				GridDirection linkDirection = linkDirections[i];
				anchor.AddModuleConnection(linkDirection);
			}
		}
	}

	public GridCoordinate GetClusterLinkLocation(TubeCluster cluster) {
		for (int i = 0; i < 6; i++) {
			if ( clusters[i] == cluster ) {
				return linkLocations[i];
			}
		}
			
		return GridCoordinate.Invalid;
	}

	public void HandleConnection(Tube tube) {
		clusters ??= new TubeCluster[6];

		for (int i = 0; i < 6; i++) {
			if ( linkLocations[i] == tube.origin ) {
				TubeCluster cluster = tube.cluster;
					
				clusters[i] = cluster;
				cluster.AddModule(this);
					
				return;
			}
		}
	}

	protected override void SetLocks() {
		GridCoordinate lockArea = moduleData.buildData.gridSize + new GridCoordinate(2);
		GridCoordinate lockOrigin = origin - new GridCoordinate(1).Rotate(orientation);
			
		for (int z = 0; z < lockArea.z; z++) {
			for (int y = 0; y < lockArea.y; y++) {
				for (int x = 0; x < lockArea.x; x++) {
					GridCoordinate rotatedLocalCoordinate = new GridCoordinate(x, y, z).Rotate(orientation);
					GridCoordinate worldCoordinate = lockOrigin + rotatedLocalCoordinate;

					if ( linkLocations.Contains(worldCoordinate) ) {
						continue;
					}

					if ( x == 0 || y == 0 || z == 0 || x == lockArea.x - 1 || y == lockArea.y - 1 || z == lockArea.z - 1 ) {
						grid.BufferMap.AddBuffer(worldCoordinate);
					} else {
						grid.LockGrid.AddLock(worldCoordinate);
					}
				}
			}
		}
	}

	public override bool ValidateTearDown(List<GridObject> excludedObjects) {
		if ( objectType == GridObjectType.MainModule ) {
			return false;
		}

		foreach (TubeCluster cluster in clusters) {
			if ( cluster == null ) {
				continue;
			}

			if ( !cluster.ValidateModuleIntegrityExcludingObjects(excludedObjects) ) {
				return false;
			}
		}

		return true;
	}

	protected override void RemoveLinks() {
		for (int i = 0; i < linkDirections.Length; i++) {
			GridCoordinate linkLocation = linkLocations[i];
			GridDirection direction = linkDirections[i];

			grid.LinkMap.RemoveLink(linkLocation, direction);
		}
	}

	protected override void RemoveLocks() {
		GridCoordinate lockArea = moduleData.buildData.gridSize + new GridCoordinate(2);
		GridCoordinate lockOrigin = origin - new GridCoordinate(1).Rotate(orientation);
			
		for (int z = 0; z < lockArea.z; z++) {
			for (int y = 0; y < lockArea.y; y++) {
				for (int x = 0; x < lockArea.x; x++) {
					GridCoordinate rotatedLocalCoordinate = new GridCoordinate(x, y, z).Rotate(orientation);
					GridCoordinate worldCoordinate = lockOrigin + rotatedLocalCoordinate;

					if ( linkLocations.Contains(worldCoordinate) ) {
						continue;
					}
						
					if ( x == 0 || y == 0 || z == 0 || x == lockArea.x - 1 || y == lockArea.y - 1 || z == lockArea.z - 1 ) {
						grid.BufferMap.RemoveBuffer(worldCoordinate);
					} else {
						grid.LockGrid.RemoveLock(worldCoordinate);
					}
				}
			}
		}
	}

	public void BeginRelocation() {
		Disconnect();
	}

	public void Relocate(GridCoordinate newOrigin, GridOrientation newOrientation) {
		isTearingDown = true;
		OnTearDown();

		isTearingDown = false;
		origin = newOrigin;
		orientation = newOrientation;
		gridBehaviour.transform.position = origin.ToWorldPositionCentered();
		gridBehaviour.ConfigureObject();

		Connect();
		ProcessLinks();
		
		gridBehaviour.HandleRelocation();
	}

	public void CancelRelocation() {
		Connect();
	}

	public void UpdateCluster(TubeCluster oldCluster, TubeCluster newCluster) {
		for (int i = 0; i < clusters.Length; i++) {
			if ( clusters[i] == oldCluster ) {
				clusters[i] = newCluster;
				return;
			}
		}
	}

	protected override void OnTearDown() {
		for (int i = 0; i < clusters.Length; i++) {
			clusters[i]?.RemoveModule(this);
			clusters[i] = null;
		}
	}

	public TubeCluster[] GetClusters() {
		return clusters;
	}
}