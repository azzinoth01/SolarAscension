using UnityEngine;

namespace SolarAscension {
    [System.Serializable]
    public class QuestTrigger {
        public QuestTriggerType type;
        [Tooltip("Time before QuestTrigger in minutes. \nRelevant if Trigger of type 'Time' or 'TimeAfter'")]
        public float activationDelay = 0f;
        [Tooltip("Relevant if Trigger of type 'Quest' or 'TimeAfter'")]
        public Quest triggerQuest;
        [HideInInspector]
        public float TimePassed = 0f; /*{ get { return PlayerPrefs.GetFloat(parentQuest.ID.ToString(), 0f); } set { PlayerPrefs.SetFloat(parentQuest.ID.ToString(), value); } }*/

        public delegate void OnTriggerHandler();
        public OnTriggerHandler OnTrigger;

        public void InitializeTrigger(Quest quest) {
            switch (type) {
                case QuestTriggerType.Time:
                    TriggerManager.Instance.SetTriggerAfterDelay(this, quest);
                    break;
                case QuestTriggerType.TimeAfter:
                    triggerQuest.goal.OnCompletion += delegate { TriggerManager.Instance.SetTriggerAfterDelay(this, quest); };
                    break;
                case QuestTriggerType.Quest:
                    triggerQuest.goal.OnCompletion += delegate { OnTrigger?.Invoke(); };
                    break;
                default:
                    break;
            }
            OnTrigger += quest.OpenQuest;
        }
    }

    public enum QuestTriggerType {
        Time,
        TimeAfter,
        Quest
    }
}