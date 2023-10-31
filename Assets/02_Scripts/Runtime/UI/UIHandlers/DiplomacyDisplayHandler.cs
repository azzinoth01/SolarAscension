using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class DiplomacyDisplayHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        VisualElement[] _factionTemplates;
        VisualElement _selectedFactionIcon, _factionSelection, _selectedStatusMarker;
        Label _selectedFactionNameLabel, _selectedFactionAllies, _selectedFactionCompetitors;

        Faction _selectedFaction;
        public Faction SelectedFaction { get { return _selectedFaction; } }

        public static VisualElement Root { get { return _root; } }


        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        private void OnDisable() {
            
        }

        void Init() {
            _factionTemplates = new VisualElement[DiplomacySystem.Instance.FactionsInfo.Factions.Count];
            for (int i = 0; i < _factionTemplates.Length; i++) {
                _factionTemplates[i] = _root.Q<VisualElement>($"faction_template_{i}");
            }
            _factionSelection = _root.Q<VisualElement>("faction_selection");
            _selectedFactionIcon = _root.Q<VisualElement>("faction_portrait_selected");
            _selectedStatusMarker = _root.Q<VisualElement>("status_marker");
            _selectedFactionNameLabel = _root.Q<Label>("faction_name_selected");
            _selectedFactionAllies = _root.Q<Label>("label_allies");
            _selectedFactionCompetitors = _root.Q<Label>("label_competitors");

            _root.SetActive(false);
            _factionSelection.SetActive(false);

            InitializeFactionTemplates();
        }

        public void ToggleDiplomacyDisplay() {

        }

        public void SelectFaction(Faction faction) {
            _selectedFaction = faction;
            _factionSelection.SetActive(true);
            SetSelectedFactionDisplay();
        }

        public void DeselectFaction() {
            _selectedFaction = null;
            _factionSelection.SetActive(false);
        }

        void InitializeFactionTemplates() {
            for (int i = 0; i < _factionTemplates.Length; i++) {
                FactionTemplateController templateController = new FactionTemplateController(_factionTemplates[i], DiplomacySystem.Instance.FactionsInfo.Factions[i], this);
                _factionTemplates[i].userData = templateController;
            }
        }

        void SetSelectedFactionDisplay() {
            _selectedFactionIcon.SetBackgroundSprite(_selectedFaction.AFaction.PreviewImage.Asset.LoadAssetAsSprite());
            _selectedStatusMarker.SetPosition(Position.Left, (int)(Mathf.InverseLerp(-100, 100, _selectedFaction.PlayerRelation) * 100), LengthType.Percent);
            _selectedFactionNameLabel.text = _selectedFaction.AFaction.DisplayName;

            _selectedFactionAllies.text = string.Empty;
            if(_selectedFaction.Allies.Count > 0) {
                foreach (DiplomaticRelation ally in _selectedFaction.Allies) {
                    _selectedFactionAllies.text += $" | {ally.faction.name}";
                }
                _selectedFactionAllies.text += " |";
                _selectedFactionAllies.style.fontSize = 10;
            }

            _selectedFactionCompetitors.text = string.Empty;
            if(_selectedFaction.Competitors.Count > 0) {
                foreach (DiplomaticRelation competitor in _selectedFaction.Competitors) {
                    _selectedFactionCompetitors.text += $" | {competitor.faction.name}";
                }
                _selectedFactionCompetitors.text += " |";
                _selectedFactionCompetitors.style.fontSize = 10;
            }

            foreach (VisualElement factionTemplate in _factionTemplates) {
                FactionTemplateController factionTemplateController = factionTemplate.userData as FactionTemplateController;
                if(factionTemplateController.Faction != _selectedFaction)
                {
                    factionTemplate.RemoveFromClassList("faction-template-selected");
                }
            }
        }
    }
}