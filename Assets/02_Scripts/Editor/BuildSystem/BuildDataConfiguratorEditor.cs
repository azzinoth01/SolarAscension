using UnityEditor;
using UnityEngine;

namespace SolarAscension {
	[CustomEditor(typeof(BuildDataConfigurator))]
	public class BuildDataConfiguratorEditor : Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			if ( GUILayout.Button("Apply attachment transforms") ) {
				((BuildDataConfigurator)target).ApplyAttachmentTransforms();
			}
			
			if ( GUILayout.Button("Generate radial attachment transforms") ) {
				((BuildDataConfigurator)target).GenerateRadialAttachmentTransforms();
			}
		}
	}
}