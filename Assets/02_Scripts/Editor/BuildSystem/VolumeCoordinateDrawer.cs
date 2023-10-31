using UnityEditor;
using UnityEngine;

namespace SolarAscension {
	[CustomPropertyDrawer(typeof(GridCoordinate))]
	public class VolumeCoordinateDrawer : PropertyDrawer {
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUIUtility.singleLineHeight + 1;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);

			var x = property.FindPropertyRelative("_x");
			var y = property.FindPropertyRelative("_y");
			var z = property.FindPropertyRelative("_z");

			Vector3Int coord = new Vector3Int(x.intValue, y.intValue, z.intValue);
			
			EditorGUI.BeginChangeCheck();

			coord = EditorGUI.Vector3IntField(position, label, coord);

			if ( EditorGUI.EndChangeCheck() ) {
				x.intValue = coord.x;
				y.intValue = coord.y;
				z.intValue = coord.z;
			}
			
			EditorGUI.EndProperty();
		}
	}
}