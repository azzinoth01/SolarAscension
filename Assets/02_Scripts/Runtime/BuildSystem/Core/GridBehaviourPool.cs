using UnityEngine;

public static class GridBehaviourPool {
	private static readonly Vector3 HoldPosition = Vector3.one * 32000f;
	private static GridBehaviour _gridPrefab;

	public static void Setup(GridBehaviour gridPrefab) {
		_gridPrefab = gridPrefab;
	}
		
	public static GridBehaviour Rent() {
		return Object.Instantiate(_gridPrefab, HoldPosition, Quaternion.identity);
	}

	public static void Return(GridBehaviour gridBehaviour) {
		Object.Destroy(gridBehaviour.gameObject);
	}
}