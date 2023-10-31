using System;

namespace SolarAscension {
    [Serializable]
    public class Reward {
        public Ressources rewardType;
        public int rewardAmount;

        public void Claim() => EconemySystemInfo.Instanz.PlayerList[0].AddingRessourceValueLocked(new RessourcesValue(rewardType, rewardAmount));
    }
}