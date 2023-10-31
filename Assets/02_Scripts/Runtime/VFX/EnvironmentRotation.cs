using System.Collections.Generic;
using UnityEngine;

public class EnvironmentRotation : MonoBehaviour {
    public float speed = 1f;
    private List<Transform> environmentObjects = new();
    private List<Vector3> rotationalAxes = new();

    void Start() {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++) {
            Transform child = transform.GetChild(i);
            environmentObjects.Add(child);

            Vector3 axisOfRotation = Random.onUnitSphere;
            rotationalAxes.Add(axisOfRotation);
        }
    }
    
    void Update() {
        float deltaAngle = speed * Time.deltaTime;
        for (int i = 0; i < environmentObjects.Count; i++) {
            Transform t = environmentObjects[i];
            t.localRotation *= Quaternion.AngleAxis(deltaAngle, rotationalAxes[i]);
        }
    }
}
