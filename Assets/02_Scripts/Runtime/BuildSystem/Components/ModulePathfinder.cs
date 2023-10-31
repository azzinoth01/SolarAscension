using UnityEngine;

public class ModulePathfinder : MonoBehaviour {
	private int _ongoingRequests;
		
	public void SetUp(BuildingGrid grid) {
		ModuleConnectionPathfinder.SetGrid(grid);
		IntegrityChecker.SetGrid(grid);
	}

	private void Update() {
		_ongoingRequests = ModuleConnectionPathfinder.RequestsToHandle;
			
		if ( ModuleConnectionPathfinder.RequestsToHandle > 0 ) {
			ModuleConnectionPathfinder.EvaluateConnection();
		}
	}
}