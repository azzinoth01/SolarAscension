using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class ModuleConnectionPathfinder {
	private const int MaxStepCount = 2000;
	private static readonly PathfindingNode[] OpenSetArray = new PathfindingNode[MaxStepCount];
	private static readonly Heap<PathfindingNode> OpenSet = new(OpenSetArray);
	private static readonly HashSet<PathfindingNode> ClosedSet = new(MaxStepCount);
	private static readonly List<PathfindingNode> Neighbors = new(6);

	private static readonly Queue<ModuleConnectionPathfindingRequest> OpenRequests;
	private static BuildingGrid _grid;

	public static int RequestsToHandle => OpenRequests.Count;

	static ModuleConnectionPathfinder() {
		OpenRequests = new Queue<ModuleConnectionPathfindingRequest>();
	}

	public static void SetGrid(BuildingGrid grid) {
		_grid = grid;
	}

	public static void AddConnectionToEvaluate(ModuleConnectionPathfindingRequest request) {
		OpenRequests.Enqueue(request);
	}

	public static void EvaluateConnection() {
		if ( OpenRequests.Count == 0 ) {
			return;
		}

		ModuleConnectionPathfindingRequest request = OpenRequests.Dequeue();

		OpenSet.Clear();
		ClosedSet.Clear();
		TraceConnection(request, null);
		request.sender.HandleFinishedConnection(request);
	}

	public static void EvaluateConnectionImmediately(ModuleConnectionPathfindingRequest request, List<GridObject> objectsToIgnore) {
		OpenSet.Clear();
		ClosedSet.Clear();
		TraceConnection(request, objectsToIgnore);
	}

	private static void TraceConnection(ModuleConnectionPathfindingRequest request, [CanBeNull] List<GridObject> objectsToIgnore) {
		PathfindingNode startNode = new PathfindingNode(request.connectionToEvaluate.moduleA.GetClusterLinkLocation(request.sender));
		PathfindingNode endNode = new PathfindingNode(request.connectionToEvaluate.moduleB.GetClusterLinkLocation(request.sender));

		if ( startNode.position == endNode.position ) {
			if ( _grid.TubeMap.HasTubeAtLocation(startNode.position) ) {
				Tube tube = _grid.TubeMap.GetTubeAtLocation(startNode.position);

				if ( objectsToIgnore != null ) {
					if ( objectsToIgnore.Contains(tube) ) {
						request.success = false;
						return;
					}
				}

				request.path = new List<Tube> { tube };
				request.success = true;
				return;
			}

			request.success = false;
			return;
		}
			
		OpenSet.Add(startNode);
			
		int step = 0;

		for (; step < MaxStepCount; step++) {
			if ( OpenSet.Count == 0 ) {
				break;
			}

			PathfindingNode current = OpenSet.RemoveFirst();
			ClosedSet.Add(current);

			if ( current.Equals(endNode) ) {
				if ( objectsToIgnore != null ) {
					request.success = true;
					return;
				}

				RetracePath(startNode, current, request);
				return;
			}

			foreach (PathfindingNode neighbor in GetNeighbors(current, objectsToIgnore)) {
				int costToNeighbor = current.gCost + GridCoordinate.Distance(current.position, neighbor.position);
				if ( !OpenSet.Contains(neighbor) ) {
					neighbor.parent = current;
					neighbor.gCost = costToNeighbor;
					neighbor.hCost = GridCoordinate.Distance(neighbor.position, endNode.position);
					OpenSet.Add(neighbor);
				}
			}
		}

		request.success = false;
	}

	private static void RetracePath(PathfindingNode start, PathfindingNode end, ModuleConnectionPathfindingRequest request) {
		List<Tube> path = new List<Tube>();

		PathfindingNode current = end;

		while (current.position != start.position) {
			Tube tube = _grid.TubeMap.GetTubeAtLocation(current.position);
			path.Add(tube);
			current = current.parent;
		}

		Tube startTube = _grid.TubeMap.GetTubeAtLocation(start.position);
		path.Add(startTube);
		path.Reverse();
			
		request.success = true;
		request.path = path;
	}

	private static List<PathfindingNode> GetNeighbors(PathfindingNode current, [CanBeNull] List<GridObject> objectsToIgnore) {
		Neighbors.Clear();

		if ( !_grid.TubeMap.HasTubeAtLocation(current.position) ) {
			return Neighbors;
		}

		Tube tubeAtLocation = _grid.TubeMap.GetTubeAtLocation(current.position);

		if ( objectsToIgnore != null && objectsToIgnore.Contains(tubeAtLocation) ) {
			return Neighbors;
		}
			
		Span<GridCoordinate> offsets = stackalloc GridCoordinate[6];
		int neighborCount = tubeAtLocation.Connections.ToGridVectorsAll(ref offsets);

		for (int i = 0; i < neighborCount; i++) {
			GridCoordinate neighborPosition = current.position + offsets[i];
				
			if ( _grid.TubeMap.HasTubeAtLocation(neighborPosition) ) {
				Tube neighborTube = _grid.TubeMap.GetTubeAtLocation(neighborPosition);
					
				if ( neighborTube.isTearingDown ) {
					continue;
				}
					
				if ( objectsToIgnore != null && objectsToIgnore.Contains(neighborTube) ) {
					return Neighbors;
				}
					
				PathfindingNode newNode = new PathfindingNode(neighborPosition);

				if ( ClosedSet.Contains(newNode) ) {
					continue;
				}
					
				Neighbors.Add(newNode);
			}
		}

		return Neighbors;
	}
}