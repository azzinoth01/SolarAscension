using UnityEngine;
using System;
using Articy.Sola;
using Articy.Unity;
using Articy.Unity.Interfaces;

namespace SolarAscension {
    [Serializable]
    [CreateAssetMenu(fileName = "Quest", menuName = "QuestSystem/Quest", order = 0)]
    public class Quest : ScriptableObject {
        [Header("Quest Data")]
        [SerializeField, ArticyTypeConstraint(typeof(Dialogue))] private ArticyRef questDialogue;
        [SerializeField, ArticyTypeConstraint(typeof(DefaultFirmCharacterShort))] private ArticyRef questGiver;
        [Space]
        [SerializeField] private Faction faction;
        [Range(0, 200)]
        public int diplomaticGravity;

        [Header("Quest Behaviour")]
        public QuestTrigger trigger;
        public QuestGoal goal;
        public QuestReward reward;

        private bool open = false;
        private bool active = false;

        private DialogueFragment _currentDialogue;

        //Quest Data
        public Dialogue AQuestDialogue { get { return questDialogue.GetObject() as Dialogue; } }
        public int ID { get { return (int)questDialogue.id; } }
        public string Name {
            get {
                IObjectWithDisplayName aObject = questDialogue.GetObject() as IObjectWithDisplayName;
                return aObject.DisplayName;
            }
        }
        public string Description { get { Dialogue dialogue = AQuestDialogue as Dialogue; return dialogue.Text; } }
        public Faction Faction { get { return faction; } }
        public DefaultFirmCharacterShort AQuestGiver { get { return questGiver.GetObject() as DefaultFirmCharacterShort; } }
        public DialogueFragment ACurrentDialogue {
            get {
                if (_currentDialogue == null) {
                    _currentDialogue = AQuestDialogue.Children[0] as DialogueFragment;
                }
                return _currentDialogue;
            }
            set { _currentDialogue = value; }
        }

        //Quest State
        public bool IsNew { get { return IsOpen == false && IsActive == false && IsCompleted == false && IsClaimed == false; } }
        public bool IsOpen { get { return open; } }
        public bool IsActive { get { return active; } }
        public bool IsCompleted { get { return goal.Completed; } }
        public bool IsClaimed { get { return reward.Claimed; } }

        public delegate void OnOpenHandler(Quest quest);
        public event OnOpenHandler OnOpen;

        public void OpenQuest() {
            open = true;
            OnOpen?.Invoke(this);

            reward.OnClaim += delegate { open = false; };
            MenuManager.Instance.ingameMenuHandler.UpdateOpenQuestDisplay();
            MenuManager.Instance.questUIHandler.UpdateQuestDiary();

            QuestSystem.UpdateSelectedQuests(this);
        }

        public void AcceptQuest() {
            active = true;
            goal.trackGoal = true;
            faction.AdjustDiplomaticRelation(diplomaticGravity);
        }

        public void DeclineQuest() {
            active = false;
            open = false;
            faction.AdjustDiplomaticRelation(-diplomaticGravity);
        }

        //TEMP
        public void Reset() {
            open = false;
            active = false;
            trigger.TimePassed = 0f;
            goal.trackGoal = false;
            goal.currentAmount = 0;
            goal.Completed = false;
            reward.Claimed = false;
        }
    }
}