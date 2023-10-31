using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class QuestListEntryController {
        VisualElement _speakerIcon, _questCompletionFill, _questCompletionBar;
        Label _questTitle, _questDescription;

        Quest _quest;

        public void Initialize(VisualElement root) {
            _speakerIcon = root.Q<VisualElement>("entry_speakerIcon");
            _questTitle = root.Q<Label>("entry_questTitle");
            _questDescription = root.Q<Label>("entry_questDescription");
            _questCompletionFill = root.Q<VisualElement>("entry_completionFill");
            _questCompletionBar = root.Q<VisualElement>("entry_completionBar");
        }

        public void AssignQuestData(Quest quest) {
            _quest = quest;
            _speakerIcon.SetBackgroundSprite(quest.AQuestGiver.PreviewImage.Asset.LoadAssetAsSprite());
            _questTitle.text = quest.Name;
            _questDescription.text = quest.AQuestDialogue.Text;
            _questCompletionBar.SetActive(quest.goal.amountNeeded > 0);
            UpdateQuestCompletionDisplay();
        }

        public void UpdateQuestCompletionDisplay() {
            if(_quest.goal.amountNeeded <= 0) {
                return;
            }
            float fillAmount = (float)_quest.goal.currentAmount / (float)_quest.goal.amountNeeded;
            _questCompletionFill.SetWidth((int)(fillAmount * 100), LengthType.Percent);
            if(_quest.goal.currentAmount >= _quest.goal.amountNeeded) {
                _questCompletionFill.style.backgroundColor = Color.green;
            }
        }
    }
}