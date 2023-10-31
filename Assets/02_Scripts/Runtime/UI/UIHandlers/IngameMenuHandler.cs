using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;


namespace SolarAscension {
    public class IngameMenuHandler : MonoBehaviour{
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        static Button _questDiaryButton, _oxygenOverlayButton, _diplomacyDisplayButton;
        VisualElement _openQuestDisplay;
        Label _openQuestCount;

        bool _showOxygen = false;

        public static VisualElement Root { get { return _root; } }
        public static Button QuestDiaryButton { get { return _questDiaryButton; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void OnDisable() => UnregisterCallbacks();

        void Init() {
            _questDiaryButton = _root.Q<Button>("button_questMenu");
            _oxygenOverlayButton = _root.Q<Button>("button_oxygenOverlay");
            _diplomacyDisplayButton = _root.Q<Button>("button_diplomacyMenu");
            _openQuestDisplay = _root.Q<VisualElement>("openQuest_display");
            _openQuestCount = _root.Q<Label>("label_openQuestCount");

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        public void UpdateOpenQuestDisplay()
        {
            _openQuestDisplay.SetActive(QuestSystem.Instance.questSystemInfo.TryGetOpenQuests(out List<Quest> openQuests));
            _openQuestCount.text = openQuests?.Count.ToString();
        }

        void ToggleOxygenOverlay() {
            PlayerStateData stateData = MenuManager.Instance.player.State.StateData;
            stateData.SetFlag(StateFlag.OverlayOxygen, !_showOxygen);
            _showOxygen = !_showOxygen;
            _oxygenOverlayButton.ToggleInClassList("build-menu-button-selected");
        }

        void ToggleDiplomacyDisplay() {
            _diplomacyDisplayButton.ToggleInClassList("diplomacy-button-selected");
            DiplomacyDisplayHandler.Root.ToggleActive(); 
            MenuManager.Instance.questUIHandler.CloseQuestDiary();
        }

        void OpenQuestDiary() {
            _questDiaryButton.AddToClassList("quest-button-selected");
            MenuManager.Instance.questUIHandler.OpenQuestDiary(); 
            DiplomacyDisplayHandler.Root.SetActive(false);
            _diplomacyDisplayButton.RemoveFromClassList("diplomacy-button-selected");
        }

        void RegisterCallbacks() {
            _questDiaryButton.clicked += OpenQuestDiary;
            _oxygenOverlayButton.clicked += ToggleOxygenOverlay;
            _diplomacyDisplayButton.clicked += ToggleDiplomacyDisplay;
        }

        void UnregisterCallbacks() {
            _questDiaryButton.clicked -= OpenQuestDiary;
            _oxygenOverlayButton.clicked -= ToggleOxygenOverlay;
            _diplomacyDisplayButton.clicked -= ToggleDiplomacyDisplay;
        }
    }
}