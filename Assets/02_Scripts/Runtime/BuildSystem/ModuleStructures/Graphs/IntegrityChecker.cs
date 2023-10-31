using System;
using System.Collections.Generic;

public delegate bool EndConditionDelegate(IntegrityNode current, List<GridObject> objectsToExclude);
public delegate List<IntegrityNode> NeighborsDelegate(IntegrityNode current, List<GridObject> objectsToExclude);

public static class IntegrityChecker {
	private const int MaxNodeCount = 2000;
	private static readonly IntegrityNode[] OpenSetArray = new IntegrityNode[MaxNodeCount];
	private static readonly Heap<IntegrityNode> OpenSet = new(OpenSetArray);
	private static readonly HashSet<IntegrityNode> ClosedSet = new(MaxNodeCount);
	private static readonly HashSet<TubeCluster> QueriedClusters = new(MaxNodeCount);
	private static readonly List<IntegrityNode> Neighbors = new(32);

	private static BuildingGrid _grid;

	public static void SetGrid(BuildingGrid grid) {
		_grid = grid;
	}

	public static bool TryFindConnectionToModule(Tube start, Tube origin, List<GridObject> objectsToExclude) {
		IntegrityNode startNode = new IntegrityNode(start);
		IntegrityNode originNode = new IntegrityNode(origin);
			
		OpenSet.Clear();
		ClosedSet.Clear();
		ClosedSet.Add(originNode);
		OpenSet.Add(startNode);

		return QueryStationStructure(NodeNeighborsModule, GetTubeNeighbors, objectsToExclude);
	}

	public static bool TryFindConnectionToMain(List<Module> startingModules, List<GridObject> objectsToExclude) {
		OpenSet.Clear();
		ClosedSet.Clear();
		QueriedClusters.Clear();
			
		foreach (Module startingModule in startingModules) {
			IntegrityNode startNode = new IntegrityNode(startingModule);
			startNode.hCost = GridCoordinate.Distance(startNode.source.origin, new GridCoordinate(0));
			OpenSet.Add(startNode);
		}

		return QueryStationStructure(NodeHasMainConnection, GetModuleNeighbors, objectsToExclude);
	}

	private static bool QueryStationStructure(EndConditionDelegate endFound, NeighborsDelegate getNeighbors, List<GridObject> objectsToExclude) {
		int step = 0;

		for (; step < MaxNodeCount; step++) {
			if ( OpenSet.Count == 0 ) {
				return false;
			}

			IntegrityNode current = OpenSet.RemoveFirst();
			ClosedSet.Add(current);

			if ( endFound(current, objectsToExclude) ) {
				return true;
			}

			foreach (IntegrityNode neighbor in getNeighbors(current, objectsToExclude)) {
				if ( !OpenSet.Contains(neighbor) ) {
					neighbor.gCost = current.gCost + GridCoordinate.Distance(current.source.origin, neighbor.source.origin);;
					OpenSet.Add(neighbor);
				}
			}
		}

		return false;
	}
		
	private static bool NodeHasMainConnection(IntegrityNode current, List<GridObject> objectsToExclude) {
		if ( current.source is Module module ) {
			TubeCluster[] clusters = module.GetClusters();

			foreach (TubeCluster cluster in clusters) {
				if ( cluster == null ) {
					continue;
				}

				if ( cluster.ContainsMainModule ) {
					return true;
				}
			}
		}

		return false;
	}

	private static List<IntegrityNode> GetTubeNeighbors(IntegrityNode current, List<GridObject> objectsToExclude) {
		Neighbors.Clear();

		if ( current.source is not Tube tube ) {
			return Neighbors;
		}

		Span<GridCoordinate> offsets = stackalloc GridCoordinate[6];
		int neighborCount = tube.Connections.ToGridVectorsAll(ref offsets);

		for (int i = 0; i < neighborCount; i++) {
			GridCoordinate neighborPosition = tube.origin + offsets[i];
				
			if ( _grid.TubeMap.HasTubeAtLocation(neighborPosition) ) {
				Tube neighborTube = _grid.TubeMap.GetTubeAtLocation(neighborPosition);
					
				if ( neighborTube.isTearingDown ) {
					continue;
				}

				if ( objectsToExclude.Contains(neighborTube) ) {
					continue;
				}

				IntegrityNode newNode = new IntegrityNode(neighborTube);

				if ( ClosedSet.Contains(newNode) ) {
					continue;
				}
					
				Neighbors.Add(newNode);
			}
		}

		return Neighbors;
	}

	private static List<IntegrityNode> GetModuleNeighbors(IntegrityNode current, List<GridObject> objectsToIgnore) {
		Neighbors.Clear();
			
		if ( current.source is not Module module ) {
			return Neighbors;
		}
			
		TubeCluster[] clusters = module.GetClusters();

		foreach (TubeCluster cluster in clusters) {
			if ( cluster == null ) {
				continue;
			}

			if ( QueriedClusters.Contains(cluster) ) {
				continue;
			}

			QueriedClusters.Add(cluster);

			List<Module> modules = cluster.connectedModules;

			foreach (Module clusterModule in modules) {
				if ( objectsToIgnore.Contains(clusterModule) ) {
					continue;
				}

				if ( clusterModule == module ) {
					continue;
				}

				IntegrityNode node = new IntegrityNode(clusterModule);

				if ( ClosedSet.Contains(node) ) {
					continue;
				}
						
				Neighbors.Add(node);
			}
		}

		return Neighbors;
	}

	private static bool NodeNeighborsModule(IntegrityNode current, List<GridObject> objectsToIgnore) {
		if ( _grid.LinkMap.TryGetLinkData(current.source.origin, out LinkData linkData) ) {
			if ( linkData.sourceModules == null ) {
				return false;
			}
			if ( linkData.sourceModules[0] != null && !objectsToIgnore.Contains(linkData.sourceModules[0]) ) {
				return true;
			}
			return linkData.sourceModules[1] != null && !objectsToIgnore.Contains(linkData.sourceModules[1]);
		}

		return false;
	}
}