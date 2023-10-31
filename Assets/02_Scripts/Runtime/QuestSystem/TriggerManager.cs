using System.Collections;
using UnityEngine;

namespace SolarAscension {
    public class TriggerManager : MonoBehaviour {
        private static TriggerManager _instance;
        public static TriggerManager Instance { get { return _instance; } }

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
            }
        }

        public void SetTriggerAfterDelay(QuestTrigger trigger, Quest parentQuest) {
            StartCoroutine(TriggerAfterDelay(trigger, parentQuest));
        }

        IEnumerator TriggerAfterDelay(QuestTrigger trigger, Quest parentQuest) {
            while (trigger.TimePassed < trigger.activationDelay * 60f) {
                trigger.TimePassed += Time.deltaTime;
                yield return null;
            }
            while (!DiplomacySystem.Instance.FactionsInfo.ExceedsQuestThreshold(parentQuest.Faction)) {
                yield return null;
            }
            trigger.OnTrigger?.Invoke();
        }
    }
}