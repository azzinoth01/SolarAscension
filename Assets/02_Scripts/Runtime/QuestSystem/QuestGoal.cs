using UnityEngine;
using System;

namespace SolarAscension {
    [Serializable]
    public class QuestGoal {
        public QuestGoalType type;
        public int amountNeeded;
        [HideInInspector]
        public bool trackGoal = false;
        [HideInInspector]
        public int currentAmount;
        bool completed = false;

        public bool Completed {
            get { return completed; }
            set {
                completed = value;
                if (value) {
                    trackGoal = false;
                    OnCompletion?.Invoke();
                }
            }
        }

        public delegate void OnCompletionHandler();
        public event OnCompletionHandler OnCompletion;

        public void InitializeGoal(Quest quest) {
            QuestSystem.Instance.OnQuestGoalUpdate += UpdateCurrentAmount;
            OnCompletion += delegate { QuestSystem.UpdateSelectedQuests(quest); };
        }

        public void UpdateCurrentAmount(QuestGoalType goalType, int amountAdded, int amountOverride = 0) {
            if(goalType == type && trackGoal) {
                currentAmount = amountOverride > 0 ? amountOverride : currentAmount;
                currentAmount += amountAdded;
                MenuManager.Instance.questUIHandler.UpdateQuestEntryCompletionStatus();
                CheckForCompletion();
            }
        }

        public void CheckForCompletion() => Completed = currentAmount >= amountNeeded;
    }

    public enum QuestGoalType {
        None,
        Money,
        Aluminum,
        Oxygen,
        Energy,
        Water,
        WorkforceEcology,
        BioWaste,
        Oil,
        Foodbars,
        BioPolymer,
        WorkforceEngineer,
        Fruits,
        Ethanol,
        FruitSlices,
        AcrylicGlass,
        Vegetables ,
        MainModule,
        Tube,
        SolarModule,
        ScrapGatherer,
        IceMiner,
        OxygenModule,
        PopulationModule,
        EcologistHousing,
        BathAttachment,
        AquaponicsModule,
        AlgaeAttachment,
        VeggieAttachment,
        BiopolymerModule,
    }
}