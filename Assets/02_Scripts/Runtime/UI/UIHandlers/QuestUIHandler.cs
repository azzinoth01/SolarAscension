using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class QuestUIHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        [SerializeField] VisualTreeAsset _listEntryTemplate;

        VisualElement _questDiary;
        Button _activeQuestListButton, _doneQuestListButton, _closeDiaryButton;

        ListView _questDiaryListView;
        List<VisualElement> _listEntries = new List<VisualElement>();

        bool _isOpen = false;

        public static VisualElement Root { get { return _root; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void OnDisable() => UnregisterCallbacks();

        void Init() {
            _questDiary = _root.Q<VisualElement>("quest_diary");

            _activeQuestListButton = _root.Q<Button>("button_activeQuestList");
            _doneQuestListButton = _root.Q<Button>("button_doneQuestList");
            _closeDiaryButton = _root.Q<Button>("button_closeQuestDiary");

            _questDiaryListView = _root.Q<ListView>("questDiary_listView");

            _questDiary.SetActive(false);

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        public void OpenQuestDiary() {
            _questDiary.SetActive(true);
            SetOpenQuestList();
        }

        public void UpdateQuestDiary() {
            if (_isOpen) {
                SetOpenQuestList();
            }
            else {
                SetClaimedQuestList();
            }
        }

        public void UpdateQuestEntryCompletionStatus() {
            foreach (VisualElement questEntry in _listEntries) {
                QuestListEntryController entryController = questEntry.userData as QuestListEntryController;
                entryController.UpdateQuestCompletionDisplay();
            }
        }

        public void CloseQuestDiary() {
            _questDiary.SetActive(false);
            IngameMenuHandler.QuestDiaryButton.RemoveFromClassList("quest-button-selected");
        }

        void SetOpenQuestList() {
            _isOpen = true;
            if(QuestSystem.Instance.questSystemInfo.TryGetOpenQuests(out List<Quest> openQuests)) {
                PopulateQuestDiary(openQuests);
            }
            else {
                _questDiaryListView.itemsSource = null;
                _questDiaryListView.Rebuild();
            }
        }

        void SetClaimedQuestList() {
            _isOpen = false;
            if (QuestSystem.Instance.questSystemInfo.TryGetClaimedQuests(out List<Quest> claimedQuests)) {
                PopulateQuestDiary(claimedQuests);
            }
            else {
                _questDiaryListView.itemsSource = null;
                _questDiaryListView.Rebuild();
            }
        }

        void PopulateQuestDiary(List<Quest> questList) {
            _questDiaryListView.fixedItemHeight = 80;
            _questDiaryListView.itemsSource = questList;
            _questDiaryListView.selectionType = SelectionType.None;
            _listEntries = new List<VisualElement>();

            _questDiaryListView.makeItem = () =>
            {
                VisualElement newListEntry = _listEntryTemplate.Instantiate();
                QuestListEntryController entryController = new QuestListEntryController();
                newListEntry.userData = entryController;
                entryController.Initialize(newListEntry);
                _listEntries.Add(newListEntry);
                return newListEntry;
            };

            _questDiaryListView.bindItem = (item, index) =>
            {
                (item.userData as QuestListEntryController).AssignQuestData((Quest)_questDiaryListView.itemsSource[index]);
            };
        }


        void RegisterCallbacks() {
            _activeQuestListButton.clicked += SetOpenQuestList;
            _doneQuestListButton.clicked += SetClaimedQuestList;
            _closeDiaryButton.clicked += CloseQuestDiary;
        }

        void UnregisterCallbacks() {
            _activeQuestListButton.clicked -= SetOpenQuestList;
            _doneQuestListButton.clicked -= SetClaimedQuestList;
            _closeDiaryButton.clicked -= CloseQuestDiary;
        }
    }
}