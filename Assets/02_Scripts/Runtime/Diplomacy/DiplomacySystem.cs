using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SolarAscension {
    public class DiplomacySystem : MonoBehaviour {
        private static DiplomacySystem _instance;
        public static DiplomacySystem Instance { get { return _instance; } }

        [SerializeField] private FactionInfo factionsInfo;

        public FactionInfo FactionsInfo { get { return factionsInfo; } }

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
            }      
        }
    }
}