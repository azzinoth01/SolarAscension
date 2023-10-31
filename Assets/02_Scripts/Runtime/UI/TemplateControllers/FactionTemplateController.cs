using UnityEngine.UIElements;

namespace SolarAscension {
    public class FactionTemplateController {
        VisualElement _root;

        VisualElement _factionIcon;
        Label _factionNameLabel, _factionStandingLabel;

        Faction _faction;
        DiplomacyDisplayHandler _displayHandler;

        public Faction Faction { get { return _faction; } }

        public FactionTemplateController(VisualElement root, Faction faction, DiplomacyDisplayHandler displayHandler) {
            _root = root;
            _faction = faction;
            _displayHandler = displayHandler;

            Init();
        }

        void Init() {
            _factionIcon = _root.Q<VisualElement>("faction_portrait");
            _factionNameLabel = _root.Q<Label>("faction_name");
            _factionStandingLabel = _root.Q<Label>("faction_diplomaticStanding");

            SetFactionTemplate();

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        void SetFactionTemplate() {
            _factionIcon.SetBackgroundSprite(_faction.AFaction.PreviewImage.Asset.LoadAssetAsSprite());
            _factionNameLabel.text = _faction.AFaction.DisplayName;
            if(_faction.PlayerRelation > 40) {
                _factionStandingLabel.text = "Ally";
            }
            else if(_faction.PlayerRelation < -40) {
                _factionStandingLabel.text = "Competitor";
            }
            else {
                _factionStandingLabel.text = "Neutral";
            }
        }

        void ToggleFactionSelection(ClickEvent e) {
            if(_displayHandler.SelectedFaction == _faction)
            {
                _displayHandler.DeselectFaction();
                _root.RemoveFromClassList("faction-template-selected");
            }
            else
            {
                _displayHandler.SelectFaction(_faction);
                _root.AddToClassList("faction-template-selected");
            }
        }

        void RegisterCallbacks() {
            _root.RegisterCallback<ClickEvent>(ToggleFactionSelection);
        }

        void UnregisterCallbacks() {
            _root.UnregisterCallback<ClickEvent>(ToggleFactionSelection);
        }
    }
}