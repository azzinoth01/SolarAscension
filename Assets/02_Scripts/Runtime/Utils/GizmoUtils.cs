using UnityEngine;

namespace SolarAscension {
	public static class GizmoUtils {
		public static readonly GUIStyle LabelStyle = new() {
			fontSize = 20,
			normal = {
				textColor = Color.white
			},
			alignment = TextAnchor.LowerCenter
		};
		
		public static readonly GUIStyle StabilizerLabelStyle = new() {
			fontSize = 20,
			normal = {
				textColor = new Color(0.6f, 0.0f, 0.6f, 1f)
			},
			alignment = TextAnchor.LowerCenter
		};
		
		public static readonly GUIStyle BufferLabelStyle = new() {
			fontSize = 20,
			normal = {
				textColor = new Color(0f, 1f, 1f, 1f)
			},
			alignment = TextAnchor.LowerCenter
		};
		
		public static readonly Color VolumeFillColor = new(1f, 1f, 1f, 0.05f);
		public static readonly Color VolumeWireColor = new(1f, 1f, 1f, 0.4f);
		
		public static readonly Color SelectionFillColor = new(0.8f, 0.4f, 0.2f, 0.2f);
		public static readonly Color SelectionWireColor = new(0.8f, 0.4f, 0.2f, 0.4f);
		
		public static readonly Color LinkFillColor = new(0.2f, 0.4f, 1f, 0.2f);
		public static readonly Color LinkWireColor = new(0.2f, 0.4f, 1f, 0.4f);
		
		public static readonly Color LockFillColor = new(1f, 0.2f, 0.2f, 0.2f);
		public static readonly Color BufferFillColor = new(0f, 1f, 1f, 0.2f);

		public static readonly Color StabilizerFillColor = new(0.6f, 0f, 0.6f, 0.2f);
	}
}