using System.Collections.Generic;
using UnityEngine;

public class ClusterDebugger : MonoBehaviour {
    private List<TubeCluster> _clusterList;

    private static List<TubeCluster> ClusterList;

    private void Awake() {
        _clusterList = new List<TubeCluster>();
        ClusterList = _clusterList;
    }

    public static void AddCluster(TubeCluster c) {
        ClusterList.Add(c);
    }

    public static void RemoveCluster(TubeCluster c) {
        ClusterList.Remove(c);
    }
}
