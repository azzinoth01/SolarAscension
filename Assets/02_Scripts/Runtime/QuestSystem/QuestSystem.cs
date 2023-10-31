using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SolarAscension {
    public class QuestSystem : MonoBehaviour {
        private static QuestSystem _instance;
        public static QuestSystem Instance { get { return _instance; } }

        public static Queue<Quest> selectedQuests = new Queue<Quest>();
        public static Quest selectedQuest;

        public static bool updateResourceGoals = false;

        public QuestSystemInfo questSystemInfo;

        public delegate void OnQuestGoalUpdateHandler(QuestGoalType type, int amountAdded, int amountOverride = 0);
        public event OnQuestGoalUpdateHandler OnQuestGoalUpdate;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
            }
        }

        private void Start() => InitializeQuests();

        private void Update() {
            if (updateResourceGoals) {
                EconemySystemInfo.Instanz.CheckRessourceQuestGoals();
                updateResourceGoals = false;
            }        
        }

        public void InitializeQuests() {
            foreach (Quest quest in questSystemInfo.Quests) {
                quest.Reset();
                quest.goal.InitializeGoal(quest);
                if (quest.IsNew) {
                    quest.trigger.InitializeTrigger(quest);
                }
            }
        }

        public static void UpdateSelectedQuests(Quest quest) {
            selectedQuests.Enqueue(quest);
            if(selectedQuest == null) {
                DequeueSelectedQuest();
            }
        }

        public static void DequeueSelectedQuest() {
            selectedQuest = selectedQuests.Count > 0 ? selectedQuests.Dequeue() : null;
            if (selectedQuest != null) {
                MenuManager.Instance.dialogueHandler.OpenDialogueModal(selectedQuest);
            }
        }

        public void InvokeGoalUpdate(QuestGoalType type, int amountAdded, int amountOverride = 0) => OnQuestGoalUpdate?.Invoke(type, amountAdded, amountOverride);

        public static QuestGoalType RessourcesToQuestGoal(Ressources resourceType) {
            QuestGoalType type = resourceType switch {
                Ressources.Money => QuestGoalType.Money,
                Ressources.Aluminium => QuestGoalType.Aluminum,
                Ressources.Oxygen => QuestGoalType.Oxygen,
                Ressources.Energy => QuestGoalType.Energy,
                Ressources.Water => QuestGoalType.Water,
                Ressources.WorkforceEcology => QuestGoalType.WorkforceEcology,
                Ressources.BioWaste => QuestGoalType.BioWaste,
                Ressources.Oil => QuestGoalType.Oil,
                Ressources.Foodbars => QuestGoalType.Foodbars,
                Ressources.BioPolymer => QuestGoalType.BioPolymer,
                Ressources.WorkforceEngineer => QuestGoalType.WorkforceEngineer,
                Ressources.Fruits => QuestGoalType.Fruits,
                Ressources.Ethanol => QuestGoalType.Ethanol,
                Ressources.FruitSlices => QuestGoalType.FruitSlices,
                Ressources.AcrylicGlass => QuestGoalType.AcrylicGlass,
                Ressources.Vegtables => QuestGoalType.Vegetables,
                _ => QuestGoalType.None
            };
            return type;
        }
        public static QuestGoalType GridObjectToQuestGoal(GridObjectType objectType) {
            QuestGoalType type = objectType switch {
                GridObjectType.None => QuestGoalType.None,
                GridObjectType.MainModule => QuestGoalType.MainModule,
                GridObjectType.Tube => QuestGoalType.Tube,
                GridObjectType.SolarModule => QuestGoalType.SolarModule,
                GridObjectType.ScrapGatherer => QuestGoalType.ScrapGatherer,
                GridObjectType.IceMiner => QuestGoalType.IceMiner,
                GridObjectType.OxygenModule => QuestGoalType.OxygenModule,
                GridObjectType.PopulationModule => QuestGoalType.PopulationModule,
                GridObjectType.EcologistHousing => QuestGoalType.EcologistHousing,
                GridObjectType.BathAttachment => QuestGoalType.BathAttachment,
                GridObjectType.AquaponicsModule => QuestGoalType.AquaponicsModule,
                GridObjectType.AlgaeAttachment => QuestGoalType.AlgaeAttachment,
                GridObjectType.VeggieAttachment => QuestGoalType.VeggieAttachment,
                GridObjectType.BiopolymereModule => QuestGoalType.BiopolymerModule,
                _ => QuestGoalType.None,
            };
            return type;
        }
    }
}