using UnityEngine;

[CreateAssetMenu(menuName = "Nebra/AttachmentData", order = 0)]
public class AttachmentData : ScriptableObject {
	public GridObjectType objectType;
	public Mesh mesh;
	public Material[] materials;
	public Material[] previewMaterials;
	public Sprite icon;
	[TextArea(2, 20)]
	public string description;
}
