using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TubeCluster {
	public List<Tube> connectedTubes;
	public List<Module> connectedModules;
	public Dictionary<Module, ConnectionList> connectionMap;

	public TubeCluster(Tube source) {
		connectedTubes = new List<Tube>();
		connectedModules = new List<Module>();
		connectionMap = new Dictionary<Module, ConnectionList>();
		connectedTubes.Add(source);
	}

	public bool ContainsMainModule { get; set; }

	public void Merge(TubeCluster other) {
		foreach (Tube connectedTube in other.connectedTubes) {
			AddTube(connectedTube);
		}

		foreach (Module connectedModule in other.connectedModules) {
			AddModule(connectedModule);
			connectedModule.UpdateCluster(other, this);
		}
			
		other.Dispose();
	}

	public void AddTube(Tube tube) {
		tube.cluster = this;
		connectedTubes.Add(tube);
	}

	public void AddModule(Module moduleToAdd) {
		if ( connectedModules.Contains(moduleToAdd) ) {
			return;
		}

		if ( moduleToAdd.objectType == GridObjectType.MainModule ) {
			ContainsMainModule = true;
		}

		ConnectionList connectionList = new ConnectionList();
		connectionMap.Add(moduleToAdd, connectionList);

		if ( connectedModules.Count > 0 ) {
			FindConnections(moduleToAdd);
		}

		connectedModules.Add(moduleToAdd);
	}

	public void RemoveTube(Tube removedTube) {
		connectedTubes.Remove(removedTube);

		if ( connectedTubes.Count == 0 && connectedModules.Count <= 1 ) {
			Dispose();
		}

		BreakConnectionsIncludingTube(removedTube);
	}

	public void RemoveModule(Module module) {
		if ( module.objectType == GridObjectType.MainModule ) {
			ContainsMainModule = false;
		}
			
		if ( connectedModules.Count > 1 ) {
			BreakConnectionsIncludingModule(module);
		} else {
			connectionMap[connectedModules[0]].connections.Clear();
		}
			
		connectedModules.Remove(module);
	}

	public void FindConnections(Module addedModule) {
		foreach (Module connectedModule in connectedModules) {
			ModuleConnection connection = new ModuleConnection(addedModule, connectedModule);
				
			var request = new ModuleConnectionPathfindingRequest {
				connectionToEvaluate = connection,
				sender = this
			};
				
			ModuleConnectionPathfinder.AddConnectionToEvaluate(request);
		}
	}

	public void HandleFinishedConnection(ModuleConnectionPathfindingRequest request) {
		if ( request.success ) {
			connectionMap[request.connectionToEvaluate.moduleA].connections.Add(request.connectionToEvaluate);
			connectionMap[request.connectionToEvaluate.moduleB].connections.Add(request.connectionToEvaluate);
			request.connectionToEvaluate.SetPath(request.path);
		}
	}

	public void BreakConnectionsIncludingModule(Module removedModule) {
		if ( !connectionMap.TryGetValue(removedModule, out ConnectionList connectionsToRemove) ) {
			return;
		}

		foreach (ModuleConnection connection in connectionsToRemove.connections) {
			if ( connection.moduleA != removedModule ) {
				connectionMap[connection.moduleA].connections.Remove(connection);
				continue;
			}

			if ( connection.moduleB != removedModule ) {
				connectionMap[connection.moduleB].connections.Remove(connection);
			}
		}
			
		connectionsToRemove.connections.Clear();
		connectionMap.Remove(removedModule);
	}

	public void BreakConnectionsIncludingTube(Tube removedTube) {
		List<ModuleConnection> pathsToRevalidate = new List<ModuleConnection>();

		foreach (Module connectedModule in connectedModules) {
			ConnectionList connectionsToCheck = connectionMap[connectedModule];
			List<ModuleConnection> clearedConnections = new List<ModuleConnection>();
				
			foreach (ModuleConnection connection in connectionsToCheck.connections) {
				if ( pathsToRevalidate.Contains(connection) ) {
					continue;
				}
					
				if ( connection.PathContainsTube(removedTube) ) {
					pathsToRevalidate.Add(connection);
					continue;
				}
					
				clearedConnections.Add(connection);
			}

			connectionMap[connectedModule].connections = clearedConnections;
		}

		foreach (ModuleConnection connection in pathsToRevalidate) {
			if ( !connectedModules.Contains(connection.moduleA) || !connectedModules.Contains(connection.moduleB) ) {
				continue;
			}
				
			var request = new ModuleConnectionPathfindingRequest {
				connectionToEvaluate = connection,
				sender = this
			};
				
			ModuleConnectionPathfinder.AddConnectionToEvaluate(request);
		}
	}

	public bool ValidateModuleIntegrityExcludingObjects(List<GridObject> objectsToExclude) {
		List<Module> remainingModules = new List<Module>();
			
		foreach (Module connectedModule in connectedModules) {
			if ( !objectsToExclude.Contains(connectedModule) ) {
				remainingModules.Add(connectedModule);
			}
		}

		return remainingModules.Count switch {
			0 => CheckIfClusterWillBeEmpty(objectsToExclude),
			1 => IntegrityChecker.TryFindConnectionToMain(remainingModules, objectsToExclude),
			_ => ValidateRemainingModuleConnections(remainingModules, objectsToExclude) &&
			     IntegrityChecker.TryFindConnectionToMain(remainingModules, objectsToExclude)
		};
	}

	private bool CheckIfClusterWillBeEmpty(List<GridObject> objectsToExclude) {
		foreach (Tube connectedTube in connectedTubes) {
			if ( objectsToExclude.Contains(connectedTube) ) {
				continue;
			}

			return false;
		}

		return true;
	}

	public bool ValidateRemainingModuleConnections(List<Module> remainingModules, List<GridObject> excludedObjects) {
		foreach (Module remainingModule in remainingModules) {
			if ( !ValidateModuleConnections(remainingModules, excludedObjects, remainingModule) ) {
				return false;
			}
		}

		return true;
	}

	private bool ValidateModuleConnections(List<Module> remainingModules, List<GridObject> excludedObjects, Module remainingModule) {
		List<ModuleConnection> connectionsToVerify = GetListOfConnectionsToVerify(remainingModule, remainingModules);
		List<ModuleConnection> invalidatedConnections = FilterInvalidatedModuleConnections(connectionsToVerify, excludedObjects);

		foreach (ModuleConnection invalidatedConnection in invalidatedConnections) {
			if ( !TryFindNewPathForInvalidatedConnection(invalidatedConnection, excludedObjects) ) {
				return false;
			}
		}

		return true;
	}

	private List<ModuleConnection> GetListOfConnectionsToVerify(Module remainingModule, List<Module> remainingModules) {
		List<ModuleConnection> connectionsToVerify = new List<ModuleConnection>();

		ConnectionList connectionList = connectionMap[remainingModule];

		foreach (ModuleConnection connection in connectionList.connections) {
			foreach (Module module in remainingModules) {
				if ( module == remainingModule ) {
					continue;
				}

				if ( connection.ConnectionContainsModule(module) ) {
					connectionsToVerify.Add(connection);
				}
			}
		}
			
		return connectionsToVerify;
	}

	private List<ModuleConnection> FilterInvalidatedModuleConnections(List<ModuleConnection> connectionsToVerify, List<GridObject> invalidatingObjects) {
		List<ModuleConnection> invalidatedConnections = new List<ModuleConnection>(connectionsToVerify.Count);

		foreach (ModuleConnection connection in connectionsToVerify) {
			if ( ConnectionNotInvalidatedByExcludedObjects(connection, invalidatingObjects) ) {
				continue;
			}
				
			invalidatedConnections.Add(connection);
		}

		return invalidatedConnections;
	}

	private bool ConnectionNotInvalidatedByExcludedObjects(ModuleConnection moduleConnection, List<GridObject> invalidatingObjects) {
		foreach (GridObject invalidatingObject in invalidatingObjects) {
			switch (invalidatingObject) {
				case Module module when moduleConnection.ConnectionContainsModule(module):
				case Tube tube when moduleConnection.PathContainsTube(tube):
					return false;
			}
		}

		return true;
	}

	private bool TryFindNewPathForInvalidatedConnection(ModuleConnection connectionToRevalidate, List<GridObject> objectsToIgnore) {
		var request = new ModuleConnectionPathfindingRequest {
			connectionToEvaluate = connectionToRevalidate,
			sender = this
		};
			
		ModuleConnectionPathfinder.EvaluateConnectionImmediately(request, objectsToIgnore);

		return request.success;
	}

	public void Dispose() {
		foreach (Module connectedModule in connectedModules) {
			connectedModule.UpdateCluster(this, null);
		}
			
		connectedModules.Clear();
		connectedTubes.Clear();
	}

	public void HandleResourceDistribution(ResourceDistributor source, Module current, RessourcesValue distribution, uint dispenseId) {
		if ( connectionMap.TryGetValue(current, out ConnectionList connections) ) {
			foreach (ModuleConnection connection in connections.connections) {
				float pathCost = connection.GetTraversalCostOfResource(distribution.Ressources);

				if ( pathCost > distribution.Value ) {
					continue;
				}

				Module otherModule = connection.GetOtherModule(current);
				otherModule.ReceiveDistribution(source, new RessourcesValue(distribution.Ressources, distribution.Value - pathCost), dispenseId);
					
				float moduleCost = otherModule.GetTraversalCost()[(int)distribution.Ressources].cost;
				float remainder = distribution.Value - pathCost - moduleCost;

				if ( remainder <= 0 ) {
					continue;
				}

				RessourcesValue remainingResource = new RessourcesValue(distribution.Ressources, remainder);
				otherModule.ContinueDistribution(source, this, remainingResource, dispenseId);
			}
		}
	}
}