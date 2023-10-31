using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class ModulePopupHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        [SerializeField] VisualTreeAsset slotDefinitionTemplate, resourceDisplayTemplate, populationNeedTemplate;

        //BuildInfo Popup
        VisualElement _bIPopup, _bIModuleIcon, _bIConstructionData;
        Label _bINameLabel, _bIDescriptionLabel;
        Button _bICloseButton;

        //SelectionInfo Popup
        VisualElement _sIPopup, _sIModuleIcon, _sIMaintenanceData, _sIConsumptionData, _sIEfficiencyData, _sIProductionData, _sIAttachmentData, _sIPriorityData, _sIPopulationData, _sINeedsData, _sIPopulationTabData, _sIDescriptionData;
        VisualElement _sIPopulationHappinessMarker, _sINeedTemplateContainer;
        VisualElement[] _sIMaintenanceDisplays, _sIMaintenanceIcons, _sIConsumptionDisplays, _sIConsumptionIcons, _sIProductionDisplays, _sIProductionIcons;
        Label _sINameLabel, _sIEfficiencyLabel, _sIDescriptionLabel, _sIPopulationCountLabel;
        Label[] _sIMaintenanceLabels, _sIConsumptionLabels, _sIProductionLabels;
        Button _sICloseButton, _sISleepModeButton, _sIBuildingInfoButton, _sINeedsButton;
        Button[] _sIPriorityButtons;

        GridObject _selectedGridObject;
        Building _selectedBuilding;
        RessourcesProduction _selectedProduction;
        ModuleData _selectedModuleData;

        int[] _resourceDataHeights = { 15, 25, 35 }, _resourceDisplayHeights = { 60, 35, 25 };
        bool deactivateOnClick = true, _showProductionData = true;

        public static VisualElement Root {
            get {
                return _root;
            }
        }

        public VisualTreeAsset ResourceDisplayTemplate { get { return resourceDisplayTemplate; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void OnDisable() => UnregisterCallbacks();

        void Update() {
            UpdateSelectionData();
            UpdateBuildInfoConstructionData();
        }

        public void Init() {
            _sIMaintenanceDisplays = new VisualElement[6];
            _sIMaintenanceIcons = new VisualElement[6];
            _sIMaintenanceLabels = new Label[6];

            _sIConsumptionDisplays = new VisualElement[6];
            _sIConsumptionIcons = new VisualElement[6];
            _sIConsumptionLabels = new Label[6];

            _sIProductionDisplays = new VisualElement[6];
            _sIProductionIcons = new VisualElement[6];
            _sIProductionLabels = new Label[6];

            _sIPriorityButtons = new Button[3];

            //Build Info Popup
            _bIPopup = _root.Q<VisualElement>("popup_buildInfo");
            _bIConstructionData = _root.Q<VisualElement>("bI_constructionData");   
            _bINameLabel = _root.Q<Label>("label_bIModuleName");
            _bIDescriptionLabel = _root.Q<Label>("label_bIDescription");
            _bIModuleIcon = _root.Q<VisualElement>("bI_ModuleIcon");
            _bICloseButton = _root.Q<Button>("button_bIClose");

            //Selection Info Popup
            _sIPopup = _root.Q<VisualElement>("popup_selectionInfo");
            _sINameLabel = _root.Q<Label>("label_sIModuleName");
            _sIModuleIcon = _root.Q<VisualElement>("sI_ModuleIcon");

            _sIMaintenanceData = _root.Q<VisualElement>("sI_maintenanceData");
            for (int i = 0; i < 6; i++) {
                _sIMaintenanceDisplays[i] = _root.Q<VisualElement>($"sI_maintenanceDisplay_{i}");
                _sIMaintenanceIcons[i] = _sIMaintenanceDisplays[i].Q<VisualElement>("sI_maintenanceIcon");
                _sIMaintenanceLabels[i] = _sIMaintenanceDisplays[i].Q<Label>("sI_maintenanceLabel");
            }

            _sIConsumptionData = _root.Q<VisualElement>("sI_consumptionData");
            for (int i = 0; i < 6; i++) {
                _sIConsumptionDisplays[i] = _root.Q<VisualElement>($"sI_consumptionDisplay_{i}");
                _sIConsumptionIcons[i] = _sIConsumptionDisplays[i].Q<VisualElement>("sI_consumptionIcon");
                _sIConsumptionLabels[i] = _sIConsumptionDisplays[i].Q<Label>("sI_consumptionLabel");
            }

            _sIEfficiencyData = _root.Q<VisualElement>("sI_efficiencyData");
            _sIEfficiencyLabel = _root.Q<Label>("sI_efficiencyLabel");

            _sIProductionData = _root.Q<VisualElement>("sI_productionData");
            for (int i = 0; i < 6; i++) {
                _sIProductionDisplays[i] = _root.Q<VisualElement>($"sI_productionDisplay_{i}");
                _sIProductionIcons[i] = _sIProductionDisplays[i].Q<VisualElement>("sI_productionIcon");
                _sIProductionLabels[i] = _sIProductionDisplays[i].Q<Label>("sI_productionLabel");
            }

            _sIAttachmentData = _root.Q<VisualElement>("sI_attachmentData");

            _sIPriorityData = _root.Q<VisualElement>("sI_priorityData");
            _sISleepModeButton = _root.Q<Button>("button_sleepMode");
            for (int i = 0; i < 3; i++) {
                _sIPriorityButtons[i] = _root.Q<Button>($"button_priority_{i + 1}");
            }

            _sIPopulationTabData = _root.Q<VisualElement>("sI_populationTabData");
            _sIBuildingInfoButton = _root.Q<Button>("button_buildingInfo");
            _sINeedsButton = _root.Q<Button>("button_needs");

            _sIPopulationData = _root.Q<VisualElement>("sI_populationData");
            _sIPopulationHappinessMarker = _root.Q<VisualElement>("population_happinessMarker");
            _sIPopulationCountLabel = _root.Q<Label>("label_populationCount");

            _sINeedsData = _root.Q<VisualElement>("sI_needsData");
            _sINeedTemplateContainer = _root.Q<VisualElement>("needTemplateContainer");

            _sIDescriptionData = _root.Q<VisualElement>("sI_descriptionData");
            _sIDescriptionLabel = _root.Q<Label>("sI_descriptionLabel");

            _sICloseButton = _root.Q<Button>("button_sIClose");


            CloseBuildInfoPopup();
            CloseSelectionInfoPopup();

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        public void OpenBuildInfoPopup(ModuleData data) {
            _selectedModuleData = data;

            _bINameLabel.text = data.descriptionData.moduleName;
            _bIDescriptionLabel.text = data.descriptionData.moduleDescription;
            _bIModuleIcon.SetBackgroundSprite(data.descriptionData.moduleIcon);

            SetConstructionData(data.buildingId);

            _root.SetActive(true);
            _sIPopup.SetActive(false);
            _bIPopup.SetActive(true);
        }

        void UpdateBuildInfoConstructionData() {
            if(_bIConstructionData == null) {
                return;
            }
            foreach(VisualElement display in _bIConstructionData.Children()) {
                ResourceDisplayController resourceDisplayController = display.userData as ResourceDisplayController;
                resourceDisplayController.UpdateResourceData();
            }
        }

        public void CloseBuildInfoPopup() {
            _bIPopup.SetActive(false);
            _root.SetActive(false);
        }

        void SetConstructionData(int ID) {
            _bIConstructionData.SetActive(false);
            _bIConstructionData.Clear();
            if(TryGetConstructionCosts(ID, out List<RessourcesValue> constructionCosts)) {
                _bIConstructionData.SetActive(true);
                _bIConstructionData.SetHeight(15 + (10 * Mathf.FloorToInt((constructionCosts.Count - 1) / 2)), LengthType.Percent);
                foreach (RessourcesValue cost in constructionCosts) {
                    VisualElement resourceDisplay = resourceDisplayTemplate.Instantiate();
                    ResourceDisplayController displayController = new ResourceDisplayController(resourceDisplay, cost, constructionCosts, true);
                    resourceDisplay.userData = displayController;
                    _bIConstructionData.Add(resourceDisplay);
                }
            }
        }

        bool TryGetConstructionCosts(int buildingID, out List<RessourcesValue> resourceList) {
            if (EconemySystemInfo.Instanz.GetBuildingProductionDescription(buildingID).RessourceCostList != null) {
                resourceList = EconemySystemInfo.Instanz.GetBuildingProductionDescription(buildingID).RessourceCostList;
                return true;
            }
            resourceList = new List<RessourcesValue>();
            return false;
        }

        #region Selection Info Popup

        public void OpenSelectionInfoPopup(GridObject gridObject) {
            InitializeSelectionData(gridObject);

            _root.SetActive(true);
            _bIPopup.SetActive(false);
            _sIPopup.SetActive(true);
        }

        void InitializeSelectionData(GridObject gridObject) {
            _showProductionData = true;

            _selectedGridObject = gridObject;
            _selectedBuilding = gridObject.economyBuilding;
            _selectedProduction = _selectedBuilding.GetProductionInformationPerMinute(true);

            _sINameLabel.text = _selectedBuilding.Name;
            _sIModuleIcon.SetBackgroundSprite(gridObject.moduleData.descriptionData.moduleIcon);

            SetMaintenanceData();
            SetConsumptionData();
            SetProductionData();

            _sIEfficiencyData.SetActive(gridObject.objectType != GridObjectType.Tube && gridObject.objectType != GridObjectType.MainModule && gridObject.objectType != GridObjectType.PopulationModule);

            _sIAttachmentData.SetActive(_selectedBuilding is BuildingContainer);
            SetAttachmentData();

            _sIPriorityData.SetActive(gridObject.objectType != GridObjectType.Tube && gridObject.objectType != GridObjectType.MainModule && gridObject.objectType != GridObjectType.PopulationModule);
            SetPriorityData();

            _sIPopulationTabData.SetActive(gridObject.objectType == GridObjectType.PopulationModule);

            _sIPopulationData.SetActive(false);

            _sINeedsData.SetActive(false);

            _sIBuildingInfoButton.AddToClassList("populationTab-button-selected");
            _sINeedsButton.RemoveFromClassList("populationTab-button-selected");

            _sIDescriptionData.SetActive(gridObject.objectType == GridObjectType.Tube || gridObject.objectType == GridObjectType.MainModule);
            _sIDescriptionLabel.text = gridObject.moduleData.descriptionData.moduleDescription;
        }

        void UpdateSelectionData() {
            if (_selectedBuilding != null) {
                _selectedProduction = _selectedBuilding.GetProductionInformationPerMinute(true);
                if (_selectedProduction != null) {
                    SetMaintenanceData();
                    SetConsumptionData();
                    SetEfficiencyData();
                    SetProductionData();
                }
                UpdatePopulationData();
            }
        }

        void SetMaintenanceData() {
            if (!_showProductionData) {
                return;
            }
            int maintenanceIndex = 0;
            foreach (VisualElement display in _sIMaintenanceDisplays) {
                display.SetActive(false);
            }
            if (_selectedProduction != null) {
                for (int i = 0; i < _selectedProduction.ConsumptionList.Count; i++) {
                    RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(_selectedProduction.ConsumptionList[i].Ressources);
                    if (info.Type == RessourceTyp.limitType) {
                        _sIMaintenanceDisplays[maintenanceIndex].SetActive(true);
                        _sIMaintenanceLabels[maintenanceIndex].text = $"{_selectedProduction.ConsumptionList[i].Value.ToString("F0")}";
                        _sIMaintenanceIcons[maintenanceIndex].SetBackgroundSprite(info.Icon);
                        maintenanceIndex++;
                    }
                }
            }
            _sIMaintenanceData.SetHeight(_resourceDataHeights[(maintenanceIndex - 1) / 2], LengthType.Percent);
            _sIMaintenanceData.SetMinHeight(_resourceDataHeights[(maintenanceIndex - 1) / 2], LengthType.Percent);
            foreach(VisualElement maintenanceDisplay in _sIMaintenanceDisplays) {
                maintenanceDisplay.SetHeight(_resourceDisplayHeights[(maintenanceIndex - 1) / 2], LengthType.Percent);
                maintenanceDisplay.SetMinHeight(_resourceDisplayHeights[(maintenanceIndex - 1) / 2], LengthType.Percent);
            }
            _sIMaintenanceData.SetActive(maintenanceIndex > 0);
        }

        void SetConsumptionData() {
            if (!_showProductionData) {
                return;
            }
            int consumptionIndex = 0;
            foreach (VisualElement display in _sIConsumptionDisplays) {
                display.SetActive(false);
            }
            if (_selectedProduction != null) {
                for (int i = 0; i < _selectedProduction.ConsumptionList.Count; i++) {
                    RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(_selectedProduction.ConsumptionList[i].Ressources);
                    if (info.Type != RessourceTyp.limitType) {
                        _sIConsumptionDisplays[consumptionIndex].SetActive(true);
                        _sIConsumptionLabels[consumptionIndex].text = $"{_selectedProduction.ConsumptionList[i].Value.ToString("F0")} | min";
                        _sIConsumptionIcons[consumptionIndex].SetBackgroundSprite(info.Icon);
                        consumptionIndex++;
                    }
                }
            }
            _sIConsumptionData.SetHeight(_resourceDataHeights[(consumptionIndex - 1) / 2], LengthType.Percent);
            _sIConsumptionData.SetMinHeight(_resourceDataHeights[(consumptionIndex - 1) / 2], LengthType.Percent);
            foreach (VisualElement consumptionDisplay in _sIConsumptionDisplays) {
                consumptionDisplay.SetHeight(_resourceDisplayHeights[(consumptionIndex - 1) / 2], LengthType.Percent);
                consumptionDisplay.SetMinHeight(_resourceDisplayHeights[(consumptionIndex - 1) / 2], LengthType.Percent);
            }
            _sIConsumptionData.SetActive(consumptionIndex > 0);
        }

        void SetEfficiencyData() {
            if (_selectedProduction != null) {
                _sIEfficiencyLabel.text = $"{(_selectedProduction.Efficiency * 100).ToString("F0")}%";
                _sIEfficiencyLabel.style.color = _selectedProduction.Efficiency < 1 ? Color.red : Color.white;
            }
        }

        void SetProductionData() {
            if (!_showProductionData) {
                return;
            }
            _sIProductionData.SetActive(false);
            foreach (VisualElement display in _sIProductionDisplays) {
                display.SetActive(false);
            }
            if (_selectedProduction != null) {
                _sIProductionData.SetActive(_selectedProduction.ProductionList.Count > 0);
                _sIProductionData.SetHeight(_resourceDataHeights[(_selectedProduction.ProductionList.Count - 1) / 2], LengthType.Percent);
                _sIProductionData.SetMinHeight(_resourceDataHeights[(_selectedProduction.ProductionList.Count - 1) / 2], LengthType.Percent);
                if (_selectedProduction.ProductionList.Count > 0) {
                    for (int i = 0; i < _selectedProduction.ProductionList.Count; i++) {
                        _sIProductionDisplays[i].SetActive(true);
                        _sIProductionDisplays[i].SetHeight(_resourceDisplayHeights[(_selectedProduction.ProductionList.Count - 1) / 2], LengthType.Percent);
                        _sIProductionDisplays[i].SetMinHeight(_resourceDisplayHeights[(_selectedProduction.ProductionList.Count - 1) / 2], LengthType.Percent);
                        _sIProductionLabels[i].text = $"{_selectedProduction.ProductionList[i].Value.ToString("F0")} | min";
                        _sIProductionIcons[i].SetBackgroundSprite(MenuManager.Instance.economyDisplayHandler.ResourceInfos[(int)_selectedProduction.ProductionList[i].Ressources].Icon);
                    }
                }
            }
        }

        void SetAttachmentData() {
            _sIAttachmentData.Clear();
            if (_selectedBuilding is BuildingContainer) {
                BuildingContainer containter = (BuildingContainer)_selectedBuilding;
                foreach (SlotDefiniton slot in containter.Slots) {
                    VisualElement attachmentSlot = slotDefinitionTemplate.Instantiate();
                    SlotDefinitionController slotController = new SlotDefinitionController(attachmentSlot, containter, _selectedGridObject, slot);
                    attachmentSlot.userData = slotController;
                    _sIAttachmentData.Add(attachmentSlot);
                }
                _sIAttachmentData.SetHeight(25 * containter.Slots.Count, LengthType.Percent);
                _sIAttachmentData.SetMinHeight(25 * containter.Slots.Count, LengthType.Percent);
            }
        }

        void SetPriorityData() {
            int priority = _selectedBuilding.CurrentPriority;
            for (int i = 0; i < _sIPriorityButtons.Length; i++) {
                _sIPriorityButtons[i].ClearClassList();
                _sIPriorityButtons[i].AddToClassList((i + 1) * 100 == priority ? "button-priority-selected" : "button-priority");
            }
            _sISleepModeButton.ClearClassList();
            _sISleepModeButton.AddToClassList(_selectedBuilding.IsActive ? "button-sleepMode" : "button-sleepMode-selected");
        }

        void UpdatePopulationData() {
            if (_selectedGridObject.objectType != GridObjectType.PopulationModule) {
                return;
            }

            BuildingContainer buildingContainer = _selectedBuilding as BuildingContainer;
            buildingContainer.GetPopulationStats(buildingContainer.Slots[0], out List<NeedsAndWants> currentNeeds, out float currentHappiness, out List<RessourcesValue> currentPopulation);
            SetPopulationData(currentHappiness, currentPopulation);
            SetNeedsData(currentNeeds);
        }

        void SetPopulationData(float currentHappiness, List<RessourcesValue> currentPopulation) {
            int totalPopulation = 0;
            foreach(RessourcesValue value in currentPopulation) {
                totalPopulation += (int)value.Value;
            }
            _sIPopulationCountLabel.text = $"{totalPopulation}";
            _sIPopulationHappinessMarker.SetPosition(Position.Left, Mathf.Clamp((int)(currentHappiness * 100), 0, 100), LengthType.Percent);
            _sIPopulationHappinessMarker.style.backgroundColor = Color.Lerp(Color.red, Color.green, currentHappiness);
        }

        void SetNeedsData(List<NeedsAndWants> currentNeeds) {
            if (_showProductionData) {
                return;
            }
            _sINeedTemplateContainer.Clear();
            foreach(NeedsAndWants need in currentNeeds) {
                VisualElement needDisplay = populationNeedTemplate.Instantiate();
                PopulationNeedTemplateController needController = new PopulationNeedTemplateController(needDisplay, need, currentNeeds);
                needDisplay.userData = needController;
                _sINeedTemplateContainer.Add(needDisplay);
            }
            foreach(VisualElement child in _sINeedTemplateContainer.Children()) {
                PopulationNeedTemplateController needController = child.userData as PopulationNeedTemplateController;
                needController.UpdateFulfillmentBar();
            }
            _sINeedsData.SetMinAndMaxHeight(_resourceDataHeights[(currentNeeds.Count - 1) / 2], LengthType.Percent);
            _sINeedsData.SetActive(currentNeeds.Count > 0);
        }

        void SetBuildingInfoTab() {
            _sIMaintenanceData.SetActive(true);
            _sIConsumptionData.SetActive(true);
            _sIProductionData.SetActive(true);
            _sIAttachmentData.SetActive(true);

            _sIPopulationData.SetActive(false);
            _sINeedsData.SetActive(false);

            _showProductionData = true;

            _sIBuildingInfoButton.AddToClassList("populationTab-button-selected");
            _sINeedsButton.RemoveFromClassList("populationTab-button-selected");
        }

        void SetNeedsTab() {
            _sIPopulationData.SetActive(true);
            _sINeedsData.SetActive(true);

            _sIMaintenanceData.SetActive(false);
            _sIConsumptionData.SetActive(false);
            _sIProductionData.SetActive(false);
            _sIAttachmentData.SetActive(false);

            _showProductionData = false;

            _sINeedsButton.AddToClassList("populationTab-button-selected");
            _sIBuildingInfoButton.RemoveFromClassList("populationTab-button-selected");
        }

        public void CloseSelectionInfoPopup() {
            _sIPopup.SetActive(false);
            _sIMaintenanceData.SetActive(false);
            _sIConsumptionData.SetActive(false);
            _sIProductionData.SetActive(false);
            _sIAttachmentData.SetActive(false);
            _sIPriorityData.SetActive(false);
            _sIDescriptionData.SetActive(false);
            _selectedBuilding = null;

            _showProductionData = true;
        }

        #endregion

        void SetDeactivateOnClick(IMouseEvent ev) => deactivateOnClick = ev is MouseEnterEvent ? false : true;

        void RegisterCallbacks() {
            _root.RegisterCallback<MouseEnterEvent>(SetDeactivateOnClick);
            _root.RegisterCallback<MouseLeaveEvent>(SetDeactivateOnClick);
            _bICloseButton.clicked += delegate {
                _bIPopup.SetActive(false);
            };
            _sICloseButton.clicked += CloseSelectionInfoPopup;
            _sIPriorityButtons[0].clicked += delegate {
                _selectedBuilding.CurrentPriority = 100;
                SetPriorityData();
            };
            _sIPriorityButtons[1].clicked += delegate {
                _selectedBuilding.CurrentPriority = 200;
                SetPriorityData();
            };
            _sIPriorityButtons[2].clicked += delegate {
                _selectedBuilding.CurrentPriority = 300;
                SetPriorityData();
            };
            _sISleepModeButton.clicked += delegate {
                _selectedBuilding.IsActive = !_selectedBuilding.IsActive;
                SetPriorityData();
            };
            _sIBuildingInfoButton.clicked += SetBuildingInfoTab;
            _sINeedsButton.clicked += SetNeedsTab;
            SlotDefinitionController.OnAttachmentBuild += delegate { SetMaintenanceData(); SetConsumptionData(); };
        }

        void UnregisterCallbacks() {
            _root.UnregisterCallback<MouseEnterEvent>(SetDeactivateOnClick);
            _root.UnregisterCallback<MouseLeaveEvent>(SetDeactivateOnClick);
            _bICloseButton.clicked -= delegate {
                _bIPopup.visible = false;
            };
            _sICloseButton.clicked -= CloseSelectionInfoPopup;
            _sIPriorityButtons[0].clicked -= delegate {
                _selectedBuilding.CurrentPriority = 100;
                SetPriorityData();
            };
            _sIPriorityButtons[1].clicked -= delegate {
                _selectedBuilding.CurrentPriority = 200;
                SetPriorityData();
            };
            _sIPriorityButtons[2].clicked -= delegate {
                _selectedBuilding.CurrentPriority = 300;
                SetPriorityData();
            };
            _sISleepModeButton.clicked -= delegate {
                _selectedBuilding.IsActive = !_selectedBuilding.IsActive;
                SetPriorityData();
            };
            _sIBuildingInfoButton.clicked -= SetBuildingInfoTab;
            _sINeedsButton.clicked -= SetNeedsTab;
            SlotDefinitionController.OnAttachmentBuild -= delegate { SetMaintenanceData(); SetConsumptionData(); };
        }
    }
}