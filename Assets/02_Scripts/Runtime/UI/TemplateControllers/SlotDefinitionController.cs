using UnityEngine.UIElements;

namespace SolarAscension {
    public class SlotDefinitionController {
        VisualElement _attachmentIcon, _attachmentProductionData, _attachmentData;
        Label _slotDefinitionLabel, _buildingSpaceLabel, _attachmentNameLabel, _attachmentDescriptionLabel;
        Button _addAttachmentButton, _removeAttachmentButton;
        Button[] _attachmentSelectors = new Button[15];

        VisualElement _template;
        BuildingContainer _parentBuilding;
        GridObject _parentGridObject;
        SlotDefiniton _slotDefinition;
        int _selectedID;
        AttachmentData _selectedAttachmentData;

        public delegate void OnAttachmentBuildHandler();
        public static event OnAttachmentBuildHandler OnAttachmentBuild;

        public SlotDefinitionController(VisualElement template, BuildingContainer parentBuilding, GridObject parentGridObject, SlotDefiniton slotDefinition) {
            _template = template;
            _parentBuilding = parentBuilding;
            _parentGridObject = parentGridObject;
            _slotDefinition = slotDefinition;

            Initialize();
        }

        void Initialize() {
            _attachmentIcon = _template.Q<VisualElement>("attachment_icon");
            _attachmentData = _template.Q<VisualElement>("buildingSlot_attachmentData");
            _attachmentProductionData = _template.Q<VisualElement>("attachment_productionData");
            _attachmentProductionData.userData = new AttachmentProductionController(_attachmentProductionData, _attachmentData);

            _slotDefinitionLabel = _template.Q<Label>("label_slotDefinitionType");
            _buildingSpaceLabel = _template.Q<Label>("label_buildingSpace");
            _attachmentNameLabel = _template.Q<Label>("label_attachmentName");
            _attachmentDescriptionLabel = _template.Q<Label>("label_attachmentDescription");

            _addAttachmentButton = _template.Q<Button>("button_addAttachment");
            _removeAttachmentButton = _template.Q<Button>("button_removeAttachment");

            _attachmentSelectors = new Button[15];
            for (int i = 0; i < _attachmentSelectors.Length; i++) {
                _attachmentSelectors[i] = _template.Q<Button>($"button_attachment_{i}");
            }

            OnAttachmentBuild += SetButtonState;
            SetSlotDefinition();
            SetButtonState();

            RegisterCallbacks();
        }

        void SetSlotDefinition() {
            _slotDefinitionLabel.text = string.Empty;
            _buildingSpaceLabel.text = $"Building Space: {_slotDefinition.Used} | {_slotDefinition.Slots}";

            for (int i = 0; i < _attachmentSelectors.Length; i++) {
                _attachmentSelectors[i].SetActive(false);
                if (i < _slotDefinition.BuildingIDs.Count) {
                    _attachmentSelectors[i].SetActive(true);
                    _attachmentSelectors[i].SetBackgroundSprite(BuildSystem.GetAttachmentData(_slotDefinition.BuildingIDs[i]).icon);
                }
            }

            _template.SetWidth(100, LengthType.Percent);
            _template.SetHeight(100 / _parentBuilding.Slots.Count, LengthType.Percent);

            SetAttachmentData(0);
        }

        void SetAttachmentData(int index) {
            _selectedID = _slotDefinition.BuildingIDs[index];

            BuildingHelper.TrySetBuildingID(_selectedID, out Building attachment);
            _selectedAttachmentData = BuildSystem.GetAttachmentData(_selectedID);
            _attachmentNameLabel.text = attachment.Name;
            _attachmentDescriptionLabel.text = _selectedAttachmentData.description;
            _attachmentIcon.SetBackgroundSprite(_selectedAttachmentData.icon);

            AttachmentProductionController attachmentProductionController = _attachmentProductionData.userData as AttachmentProductionController;
            attachmentProductionController.SetAttachmentData(attachment);
        }

        void AddAttachment() {
            if (_selectedAttachmentData == null) {
                return;
            }

            BuildingHelper.TrySetBuildingID(_selectedID, out Building attachment);
            AttachmentBehaviour attachmentBehaviour = AttachmentBehaviourPool.Rent(_selectedAttachmentData, _parentGridObject, attachment, _slotDefinition);

            attachment.BuildingPlaced("0", _parentBuilding, false);

            _buildingSpaceLabel.text = $"Building Space: {_slotDefinition.Used} | {_slotDefinition.Slots}";

            OnAttachmentBuild?.Invoke();
        }

        void RemoveAttachment() {
            if (_selectedAttachmentData == null) {
                return;
            }

            for (int i = _slotDefinition.AttachmentBehaviours.Count - 1; i >= 0; i--) {
                if(_slotDefinition.AttachmentBehaviours[i].associatedBuilding.ID == _selectedID) {
                    AttachmentBehaviourPool.Return(_slotDefinition.AttachmentBehaviours[i]);
                    _slotDefinition.AttachmentBehaviours.RemoveAt(i);
                    break;
                }
            }

            _buildingSpaceLabel.text = $"Building Space: {_slotDefinition.Used} | {_slotDefinition.Slots}";

            OnAttachmentBuild?.Invoke();
        }

        void SetButtonState() {
            _addAttachmentButton.SetVisible(_slotDefinition.Used < _slotDefinition.Slots);
            _removeAttachmentButton.SetVisible(_slotDefinition.Used > 0);
        }

        void RegisterCallbacks() {
            _addAttachmentButton.clicked += AddAttachment;
            _removeAttachmentButton.clicked += RemoveAttachment;

            _attachmentSelectors[0].clicked += delegate { SetAttachmentData(0); };
            _attachmentSelectors[1].clicked += delegate { SetAttachmentData(1); };
            _attachmentSelectors[2].clicked += delegate { SetAttachmentData(2); };
            _attachmentSelectors[3].clicked += delegate { SetAttachmentData(3); };
            _attachmentSelectors[4].clicked += delegate { SetAttachmentData(4); };
            _attachmentSelectors[5].clicked += delegate { SetAttachmentData(5); };
            _attachmentSelectors[6].clicked += delegate { SetAttachmentData(6); };
            _attachmentSelectors[7].clicked += delegate { SetAttachmentData(7); };
            _attachmentSelectors[8].clicked += delegate { SetAttachmentData(8); };
            _attachmentSelectors[9].clicked += delegate { SetAttachmentData(9); };
            _attachmentSelectors[10].clicked += delegate { SetAttachmentData(10); };
            _attachmentSelectors[11].clicked += delegate { SetAttachmentData(11); };
            _attachmentSelectors[12].clicked += delegate { SetAttachmentData(12); };
            _attachmentSelectors[13].clicked += delegate { SetAttachmentData(13); };
            _attachmentSelectors[14].clicked += delegate { SetAttachmentData(14); };
        }
    }
}