using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ModuleConnection {
	public Module moduleA, moduleB;
	public List<Tube> path;
	public float[] traversalCostPerResource;

	private static int _resourceTypeCount;

	public ModuleConnection(Module a, Module b) {
		moduleA = a;
		moduleB = b;
		traversalCostPerResource = new float[_resourceTypeCount];
	}

	static ModuleConnection() {
		_resourceTypeCount = (int)Enum.GetValues(typeof(Ressources)).Cast<Ressources>().Max() + 1;
	}

	public void SetPath(List<Tube> pathFound) {
		path = pathFound;

		foreach (Tube tube in pathFound) {
			foreach (DistributionCost cost in tube.GetTraversalCost()) {
				traversalCostPerResource[(int)cost.resource] += cost.cost;
			}
		}
	}

	public bool PathContainsTube(Tube tube) {
		return path.Contains(tube);
	}

	public bool ConnectionContainsModule(Module module) {
		return moduleA == module || moduleB == module;
	}

	public float GetTraversalCostOfResource(Ressources resource) {
		return traversalCostPerResource[(int)resource];
	}

	public Module GetOtherModule(Module module) {
		return module != moduleA ? moduleA : moduleB;
	}
}