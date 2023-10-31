using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Articy.Sola;
using Articy.Unity;
using Articy.Unity.Interfaces;

namespace SolarAscension {
    public class DialogueHandler : MonoBehaviour, IArticyFlowPlayerCallbacks {
        static VisualElement _root;
        ArticyFlowPlayer _flowPlayer;
        UIDocumentLocalization _uIDocumentLocalization;

        VisualElement _questModal, _speakerIcon, _factionIcon;
        Label _dialogueLabel;
        Button _acceptButton, _declineButton, _claimButton, _nextButton, _closeButton, _completeButton, _skipButton;

        string _currentAudioKey;
        List<string> _currentKeys = new List<string>();

        public static VisualElement Root { get { return _root; } }
        
        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _flowPlayer = GetComponent<ArticyFlowPlayer>();
            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void OnDisable() => UnregisterCallbacks();

        void Init() {
            _questModal = _root.Q<VisualElement>("quest_modal");
            _speakerIcon = _root.Q<VisualElement>("speaker_portrait");
            _factionIcon = _root.Q<VisualElement>("faction_portrait");

            _dialogueLabel = _root.Q<Label>("label_dialogue");
            //_dialogueLabel.AddManipulator(new TooltipManipulator());

            _acceptButton = _root.Q<Button>("button_acceptQuest");
            _declineButton = _root.Q<Button>("button_declineQuest");
            _claimButton = _root.Q<Button>("button_claimQuest");
            _nextButton = _root.Q<Button>("button_next");
            _closeButton = _root.Q<Button>("button_close");
            _completeButton = _root.Q<Button>("button_complete");
            _skipButton = _root.Q<Button>("button_skip");

            _questModal.SetActive(false);

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        public void OpenDialogueModal(Quest quest) {
            _questModal.SetActive(true);
            _flowPlayer.StartOn = quest.ACurrentDialogue;
        }

        public void OpenDialogueModal(IArticyObject aObject) {
            _questModal.SetActive(true);
            _flowPlayer.StartOn = aObject;
        }

        void CloseDialogueModal() {
            _questModal.SetActive(false);
            if (_currentKeys.Contains("Claim")) {
                QuestSystem.selectedQuest.reward.ClaimReward();
            }

            _flowPlayer.Play();

            AudioManager.Instance.Stop(_currentAudioKey);
            QuestSystem.DequeueSelectedQuest();
        }

        void SetDialogueModal(List<string> keys) {
            _acceptButton.SetActive(keys.Contains("Accept"));
            _declineButton.SetActive(keys.Contains("Decline"));
            _closeButton.SetActive(keys.Contains("Close"));
            _claimButton.SetActive(false);
            if (keys.Contains("Claim"))
            {
                _claimButton.SetActive(QuestSystem.selectedQuest.reward.Rewards.Count > 0);
                _closeButton.SetActive(QuestSystem.selectedQuest.reward.Rewards.Count == 0);
            }
            _nextButton.SetActive(keys.Contains("Next"));
            _completeButton.SetActive(keys.Contains("Complete"));
            _skipButton.SetActive(keys.Contains("Skip"));

            _dialogueLabel.tooltip = (!keys.Contains("Accept") && !keys.Contains("Decline") && !keys.Contains("Claim") && !keys.Contains("Next") && !keys.Contains("Close") ? $"l:{keys[0]}" : "");
        }

        void Play() {
            AudioManager.Instance.Stop(_currentAudioKey);
            _flowPlayer.Play();
        }

        void Skip()
        {
            QuestSystem.selectedQuest.reward.ClaimReward();
            MenuManager.Instance.buildingMenuHandler.SkipTutorial();
            CloseDialogueModal();
        }

        public void OnBranchesUpdated(IList<Branch> aBranches) {

        }

        public void OnFlowPlayerPaused(IFlowObject aObject) {
            if(QuestSystem.selectedQuest != null) {
                QuestSystem.selectedQuest.ACurrentDialogue = aObject as DialogueFragment;
            }

            IObjectWithText aObjectWithText = aObject as IObjectWithText;
            if (aObjectWithText != null) { _dialogueLabel.text = aObjectWithText.Text; }

            IObjectWithStageDirections aObjectWithStageDirections = aObject as IObjectWithStageDirections;
            if (aObjectWithStageDirections != null) {
                _currentAudioKey = aObjectWithStageDirections.StageDirections;
                AudioManager.Instance.Play(_currentAudioKey);
            }

            IObjectWithMenuText aObjectWithMenuText = aObject as IObjectWithMenuText;
            if (aObjectWithMenuText != null) {
                string[] menuKeys = aObjectWithMenuText.MenuText.Split(",");
                List<string> keys = new List<string>();
                foreach(string key in menuKeys) {
                    keys.Add(key);
                }
                _currentKeys = keys;
                SetDialogueModal(keys); 
            }

            IObjectWithSpeaker aObjectWithSpeaker = aObject as IObjectWithSpeaker;
            if (aObjectWithSpeaker != null) {
                DefaultFirmCharacterShort speaker = aObjectWithSpeaker.Speaker as DefaultFirmCharacterShort;
                if(speaker != null) {
                    _speakerIcon.SetBackgroundSprite(speaker.PreviewImage.Asset.LoadAssetAsSprite());

                    DefaultFactionTemplate speakerFaction = ArticyDatabase.GetObject<DefaultFactionTemplate>(speaker.Template.DefaultBasicCharacterTemplate.Faction);
                    if(speakerFaction != null) {
                        _factionIcon.SetBackgroundSprite(speakerFaction.PreviewImage.Asset.LoadAssetAsSprite());
                    }
                }
            }
        }

        void RegisterCallbacks() {
            _acceptButton.clicked += delegate { QuestSystem.selectedQuest.AcceptQuest(); Play(); };
            _declineButton.clicked += delegate { QuestSystem.selectedQuest.DeclineQuest(); CloseDialogueModal(); };
            _claimButton.clicked += delegate { QuestSystem.selectedQuest.reward.ClaimReward(); CloseDialogueModal(); };
            _nextButton.clicked += Play;
            _closeButton.clicked += CloseDialogueModal;
            _completeButton.clicked += delegate { QuestSystem.selectedQuest.goal.Completed = true; CloseDialogueModal(); };
            _skipButton.clicked += Skip;
        }

        void UnregisterCallbacks() {
            _acceptButton.clicked -= delegate { QuestSystem.selectedQuest.AcceptQuest(); Play(); };
            _declineButton.clicked -= delegate { QuestSystem.selectedQuest.DeclineQuest(); CloseDialogueModal(); };
            _claimButton.clicked -= delegate { QuestSystem.selectedQuest.reward.ClaimReward(); CloseDialogueModal(); };
            _nextButton.clicked -= Play;
            _closeButton.clicked -= CloseDialogueModal;
            _completeButton.clicked -= delegate { QuestSystem.selectedQuest.goal.Completed = true; CloseDialogueModal(); };
            _skipButton.clicked -= Skip;
        }
    }
}