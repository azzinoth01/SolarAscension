using System.Collections.Generic;

public class ModuleConnectionPathfindingRequest {
	public ModuleConnection connectionToEvaluate;
	public TubeCluster sender;
	public bool success;
	public List<Tube> path;
}