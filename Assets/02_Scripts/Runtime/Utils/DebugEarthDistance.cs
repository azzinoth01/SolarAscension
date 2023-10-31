#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class DebugEarthDistance : MonoBehaviour {
	[SerializeField] private LayerMask mask = -1;
	
	#if UNITY_EDITOR
	private void OnDrawGizmos() {
		if ( Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10000f, mask) ) {
			GUIStyle style = new GUIStyle {
				fontSize = 20,
				normal = {
					textColor = Color.white
				},
				alignment = TextAnchor.LowerCenter
			};
			Handles.Label(transform.position, hit.distance.ToString(), style);
		}
	}
	#endif
}