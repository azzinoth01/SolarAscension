using System.Collections.Generic;
using UnityEngine;

namespace SolarAscension {
    [System.Serializable]
    [CreateAssetMenu(fileName = "FactionInfo", menuName = "Diplomacy/Faction Info", order = 1)]
    public class FactionInfo : ScriptableObject {
        public List<Faction> Factions = new List<Faction>();

        public int diplomaticQuestThreshold = 0, diplomaticTradeThreshhold = 30;

        public bool ExceedsQuestThreshold(Faction faction) => faction.PlayerRelation >= diplomaticQuestThreshold;
        public bool ExceedsTradeThreshold(Faction faction) => faction.PlayerRelation >= diplomaticTradeThreshhold;
    }
}