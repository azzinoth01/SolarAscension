using System.Collections.Generic;
using UnityEngine;

namespace SolarAscension {
    [System.Serializable]
    [CreateAssetMenu(fileName = "QuestSystemInfo", menuName = "QuestSystem/QuestSystemInfo", order = 1)]
    public class QuestSystemInfo : ScriptableObject {
        [SerializeField] private List<Quest> quests = new List<Quest>();

        public List<Quest> Quests { get { return quests; } }

        public bool TryGetNewQuests(out List<Quest> newQuests) {
            newQuests = new List<Quest>();
            foreach (Quest quest in quests) {
                if (quest.IsNew) {
                    newQuests.Add(quest);
                }
            }
            return newQuests.Count > 0;
        }
        public bool TryGetOpenQuests(out List<Quest> openQuests) {
            openQuests = new List<Quest>();
            foreach (Quest quest in quests) {
                if (quest.IsOpen) {
                    openQuests.Add(quest);
                }
            }
            return openQuests.Count > 0;
        }
        public bool TryGetActiveQuests(out List<Quest> activeQuests) {
            activeQuests = new List<Quest>();
            foreach (Quest quest in quests) {
                if (quest.IsActive) {
                    activeQuests.Add(quest);
                }
            }
            return activeQuests.Count > 0;
        }
        public bool TryGetClaimedQuests(out List<Quest> claimedQuests) {
            claimedQuests = new List<Quest>();
            foreach (Quest quest in quests) {
                if (quest.IsClaimed) {
                    claimedQuests.Add(quest);
                }
            }
            return claimedQuests.Count > 0;
        }
    }
}