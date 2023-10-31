using UnityEngine;

public class GridMeshObject : MonoBehaviour {
	[SerializeField] public GridBehaviour gridBehaviour;
	[SerializeField] public MeshFilter meshFilter;
	[SerializeField] public MeshRenderer meshRenderer;
	[SerializeField] private BoxCollider boxCollider;

	public void HideMesh() {
		meshRenderer.enabled = false;
		boxCollider.enabled = false;
	}

	public void ShowMesh() {
		meshRenderer.enabled = true;
		boxCollider.enabled = true;
	}

	public void SetPropertyBlock(MaterialPropertyBlock block) {
		meshRenderer.SetPropertyBlock(block);
	}
		
	public void ConfigureMeshObject(BuildData buildData) {
		meshFilter.sharedMesh = buildData.mesh;
		meshRenderer.sharedMaterials = buildData.materials;
			
		Vector3 localPos = (buildData.gridSize - Vector3.one) * BuildSystem.HalfScale;
		Vector3 boundsSize = (Vector3)buildData.gridSize * (BuildSystem.VolumeScale * 0.8f);
			
		transform.localPosition = localPos;
		boxCollider.size = boundsSize;
		boxCollider.enabled = true;
	}

	public void UpdateTubeMeshAndRotation(LinkDirection linkDirection) {
		(Mesh mesh, Quaternion rotation) = TubeConfigurator.GetTubeState(linkDirection);
		meshFilter.sharedMesh = mesh;
		transform.localRotation = rotation;
	}

	public void ResetMeshObject() {
		meshFilter.sharedMesh = null;
			
		transform.localPosition = Vector3.zero;
		boxCollider.enabled = false;
		boxCollider.size = Vector3.one * BuildSystem.HalfScale;
	}

	public Matrix4x4 GetLocalToWorld() {
		return transform.localToWorldMatrix;
	}
}