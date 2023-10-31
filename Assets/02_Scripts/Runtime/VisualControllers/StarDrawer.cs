using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StarDrawer : MonoBehaviour {
    [SerializeField] private Material starMaterial;
    [SerializeField] private Mesh starMesh;

    [SerializeField] private int starCount;
    [SerializeField] private float starRange;

    [SerializeField] private float starMinSize, starMaxSize;

    private List<Matrix4x4> _starMatrices;
    

    private void Start() {
        _starMatrices = new List<Matrix4x4>(starCount);

        for (int i = 0; i < starCount; i++) {
            Vector3 starPos = Random.onUnitSphere * starRange;
            Matrix4x4 starMatrix = Matrix4x4.TRS(starPos, Quaternion.identity, Vector3.one * Random.Range(starMinSize, starMaxSize));
            _starMatrices.Add(starMatrix);
        }
    }

    private void Update() {
        Graphics.DrawMeshInstanced(starMesh, 0, starMaterial, _starMatrices, null, ShadowCastingMode.Off);
    }
}
