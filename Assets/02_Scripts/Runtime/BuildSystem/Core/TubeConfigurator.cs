using System.Collections.Generic;
using UnityEngine;

public class TubeConfigurator : MonoBehaviour
{
    [SerializeField] private List<TubeConfig> configs = new();
    private static readonly Dictionary<LinkDirection, TubeConfig> Configurations = new();

    public void Setup() {
        if ( Configurations.Count > 0 ) {
            return;
        }
        foreach(TubeConfig config in configs) {
            Configurations.Add(config.connections, config);
        }
    }

    public static (Mesh, Quaternion) GetTubeState(LinkDirection direction) {
        if(Configurations.TryGetValue(direction, out TubeConfig state)) {
            return (state.tube, Quaternion.Euler(state.rotation));
        }

        TubeConfig defaultState = Configurations[LinkDirection.Forward];
        return (defaultState.tube, Quaternion.Euler(defaultState.rotation));
    }
}