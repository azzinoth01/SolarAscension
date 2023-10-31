using UnityEngine;
using System;
using System.Collections.Generic;
using Articy.Unity;
using Articy.Sola;

namespace SolarAscension {
    [Serializable]
    [CreateAssetMenu(fileName = "Faction", menuName = "Diplomacy/Faction", order = 0)]
    public class Faction : ScriptableObject {
        [Header("Faction Data")]
        [SerializeField, ArticyTypeConstraint(typeof(DefaultFactionTemplate))] private ArticyRef factionData;

        [Header("Resources")]
        [SerializeField] private List<RessourcesValue> resources = new List<RessourcesValue>();

        [Header("Diplomatic Relations")]
        [Range(_relationMin, _relationMax)]
        [SerializeField] private int playerRelation;
        [Space]
        [SerializeField] private List<DiplomaticRelation> allies = new List<DiplomaticRelation>();
        [SerializeField] private List<DiplomaticRelation> competitors = new List<DiplomaticRelation>();

        private const int _relationMax = 100, _relationMin = -100;

        public int PlayerRelation { get { return playerRelation; } set { playerRelation = value; Mathf.Clamp(playerRelation, _relationMin, _relationMax); } }
        public DefaultFactionTemplate AFaction { get { return factionData.GetObject() as DefaultFactionTemplate; } }
        public List<RessourcesValue> Ressources { get { return resources; } }
        public List<DiplomaticRelation> Allies { get { return allies; } }
        public List<DiplomaticRelation> Competitors { get { return competitors; } }

        public void RequestResource(Ressources resource, int amount) {
            if (DiplomacySystem.Instance.FactionsInfo.ExceedsTradeThreshold(this)) {
                if (TryGetFactionResource(resource, out RessourcesValue value)) {
                    if (value.Value >= amount) {
                        //Do Resource Transfer stuff
                        value.Value -= amount;
                        //Discern Gravity
                        int gravity = 0;
                        AdjustDiplomaticRelation(gravity);
                    }
                    else { Debug.Log($"This Faction does not own enough of Resource: {resource}"); }
                }
                else { Debug.Log($"This Faction does not own the Resource: {resource}"); }
            }
            else { Debug.Log($"Your Diplomatic status is not high enough"); }
        }

        public void SupplyResource(Ressources resource, int amount) {
            if (!TryGetFactionResource(resource, out RessourcesValue value)) {
                resources.Add(value);
            }
            //Do Resource Transfer stuff
            value.Value += amount;
            //Discern Gravity
            int gravity = 0;
            AdjustDiplomaticRelation(gravity);
        }

        public void AdjustDiplomaticRelation(int gravity) {
            PlayerRelation += gravity;
            EvaluateDiplomaticConsequences(gravity);
        }

        void EvaluateDiplomaticConsequences(int gravity) {
            foreach (DiplomaticRelation relation in allies) {
                relation.faction.PlayerRelation += Mathf.RoundToInt(gravity * (relation.diplomaticStatus / 100f));
            }
            foreach (DiplomaticRelation relation in competitors) {
                relation.faction.PlayerRelation += Mathf.RoundToInt(gravity * (relation.diplomaticStatus / 100f));
            }
        }

        bool TryGetFactionResource(Ressources resourceType, out RessourcesValue resource) {
            foreach (RessourcesValue entry in resources) {
                if (entry.Ressources == resourceType) {
                    resource = entry;
                    return true;
                }
            }
            resource = new RessourcesValue(resourceType);
            return false;
        }
    }
}