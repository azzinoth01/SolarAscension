using System;
using System.Collections.Generic;
using UnityEngine;

namespace SolarAscension {
    [Serializable]
    public class QuestReward {
        [SerializeField] List<Reward> rewards = new List<Reward>();

        bool claimed = false;

        public List<Reward> Rewards { get { return rewards; } }

        public bool Claimed {
            get { return claimed; }
            set {
                claimed = value;
                if (value) {
                    OnClaim?.Invoke();
                }
            }
        }

        public delegate void OnClaimHandler();
        public event OnClaimHandler OnClaim;

        public void ClaimReward() {
            if(Claimed == true) {
                return;
            }
            foreach (Reward reward in rewards) {
                reward.Claim();
            }
            Claimed = true;
            MenuManager.Instance.ingameMenuHandler.UpdateOpenQuestDisplay();
            MenuManager.Instance.questUIHandler.UpdateQuestDiary();
        }
    }
}