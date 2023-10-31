using UnityEngine;

namespace SolarAscension {
    [System.Serializable]
    public class DiplomaticRelation {
        public Faction faction;
        [Range(-100, 100), Tooltip("Factions with a positive diplomatic status are considered Allies, those with a negative diplomatic status Competitors")]
        public int diplomaticStatus;
    }
}