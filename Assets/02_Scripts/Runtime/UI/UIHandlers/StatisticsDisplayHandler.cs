using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class StatisticsDisplayHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        Button[] _panelSelectors;
        VisualElement[] _statisticPanels, _resourceProductionIcons, _resourceStorageIcons, _resourceStorageBars, _resourceProductionBars, _resourceConsumptionBars;
        Label[] _resourceProductionBalances, _resourceConsumptionBalances, _resourceStorageBalances;

        Button _closeButton;

        RessourceInfo[] _resourceInfos = new RessourceInfo[17];
        RessourcesValue[] _storageData = new RessourcesValue[17], _consumptionData = new RessourcesValue[17], _productionData = new RessourcesValue[17];

        public static VisualElement Root { get { return _root; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;

            SetEconomyInfo();
        }

        private void Update() => UpdateResourceInfo();

        void OnDisable() => UnregisterCallbacks();

        void Init() {
            _panelSelectors = new Button[2];
            _panelSelectors[0] = _root.Q<Button>("button_productionPanel");
            _panelSelectors[1] = _root.Q<Button>("button_storagePanel");

            _statisticPanels = new VisualElement[2];
            _statisticPanels[0] = _root.Q<VisualElement>("panel_production");
            _statisticPanels[1] = _root.Q<VisualElement>("panel_storage");

            _resourceStorageIcons = new VisualElement[17];
            _resourceStorageIcons[(int)Ressources.Aluminium] = _root.Q<VisualElement>("aluminum-icon");
            _resourceStorageIcons[(int)Ressources.Water] = _root.Q<VisualElement>("water-icon");
            _resourceStorageIcons[(int)Ressources.Vegtables] = _root.Q<VisualElement>("vegetables-icon");
            _resourceStorageIcons[(int)Ressources.Oil] = _root.Q<VisualElement>("oil-icon");
            _resourceStorageIcons[(int)Ressources.BioPolymer] = _root.Q<VisualElement>("biopolymere-icon");

            _resourceStorageBalances = new Label[17];
            _resourceStorageBalances[(int)Ressources.Aluminium] = _root.Q<Label>("aluminum_storageBalance");
            _resourceStorageBalances[(int)Ressources.Water] = _root.Q<Label>("water_storageBalance");
            _resourceStorageBalances[(int)Ressources.Foodbars] = _root.Q<Label>("vegetables_storageBalance");
            _resourceStorageBalances[(int)Ressources.Oil] = _root.Q<Label>("oil_storageBalance");
            _resourceStorageBalances[(int)Ressources.BioPolymer] = _root.Q<Label>("biopolymere_storageBalance");

            _resourceStorageBars = new VisualElement[17];
            _resourceStorageBars[(int)Ressources.Aluminium] = _root.Q<VisualElement>("aluminum_storageBar");
            _resourceStorageBars[(int)Ressources.Water] = _root.Q<VisualElement>("water_storageBar");
            _resourceStorageBars[(int)Ressources.Vegtables] = _root.Q<VisualElement>("vegetables_storageBar");
            _resourceStorageBars[(int)Ressources.Oil] = _root.Q<VisualElement>("oil_storageBar");
            _resourceStorageBars[(int)Ressources.BioPolymer] = _root.Q<VisualElement>("biopolymere_storageBar");

            _resourceProductionIcons = new VisualElement[17];
            _resourceProductionIcons[(int)Ressources.Aluminium] = _root.Q<VisualElement>("aluminum_productionIcon");
            _resourceProductionIcons[(int)Ressources.Water] = _root.Q<VisualElement>("water_productionIcon");
            _resourceProductionIcons[(int)Ressources.Vegtables] = _root.Q<VisualElement>("vegetables_productionIcon");
            _resourceProductionIcons[(int)Ressources.Oil] = _root.Q<VisualElement>("oil_productionIcon");
            _resourceProductionIcons[(int)Ressources.BioPolymer] = _root.Q<VisualElement>("biopolymere_productionIcon");

            _resourceProductionBalances = new Label[17];
            _resourceProductionBalances[(int)Ressources.Aluminium] = _root.Q<Label>("aluminum_productionBalance");
            _resourceProductionBalances[(int)Ressources.Water] = _root.Q<Label>("water_productionBalance");
            _resourceProductionBalances[(int)Ressources.Vegtables] = _root.Q<Label>("vegetables_productionBalance");
            _resourceProductionBalances[(int)Ressources.Oil] = _root.Q<Label>("oil_productionBalance");
            _resourceProductionBalances[(int)Ressources.BioPolymer] = _root.Q<Label>("biopolymere_productionBalance");

            _resourceProductionBars = new VisualElement[17];
            _resourceProductionBars[(int)Ressources.Aluminium] = _root.Q<VisualElement>("aluminum_productionBar");
            _resourceProductionBars[(int)Ressources.Water] = _root.Q<VisualElement>("water_productionBar");
            _resourceProductionBars[(int)Ressources.Vegtables] = _root.Q<VisualElement>("vegetables_productionBar");
            _resourceProductionBars[(int)Ressources.Oil] = _root.Q<VisualElement>("oil_productionBar");
            _resourceProductionBars[(int)Ressources.BioPolymer] = _root.Q<VisualElement>("biopolymere_productionBar");

            _resourceConsumptionBalances = new Label[17];
            _resourceConsumptionBalances[(int)Ressources.Aluminium] = _root.Q<Label>("aluminum_consumptionBalance");
            _resourceConsumptionBalances[(int)Ressources.Water] = _root.Q<Label>("water_consumptionBalance");
            _resourceConsumptionBalances[(int)Ressources.Vegtables] = _root.Q<Label>("vegetables_consumptionBalance");
            _resourceConsumptionBalances[(int)Ressources.Oil] = _root.Q<Label>("oil_consumptionBalance");
            _resourceConsumptionBalances[(int)Ressources.BioPolymer] = _root.Q<Label>("biopolymere_consumptionBalance");

            _resourceConsumptionBars = new VisualElement[17];
            _resourceConsumptionBars[(int)Ressources.Aluminium] = _root.Q<VisualElement>("aluminum_consumptionBar");
            _resourceConsumptionBars[(int)Ressources.Water] = _root.Q<VisualElement>("water_consumptionBar");
            _resourceConsumptionBars[(int)Ressources.Vegtables] = _root.Q<VisualElement>("vegetables_consumptionBar");
            _resourceConsumptionBars[(int)Ressources.Oil] = _root.Q<VisualElement>("oil_consumptionBar");
            _resourceConsumptionBars[(int)Ressources.BioPolymer] = _root.Q<VisualElement>("biopolymere_consumptionBar");

            _closeButton = _root.Q<Button>("button_closeStatisticsPanel");

            _root.SetActive(false);

            SetResourceIcons();
            SetPanelWithIndex(0);

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        public void OpenPanelByIndex(int index) {
            _root.SetActive(true);
            SetPanelWithIndex(index);
        }

        void SetPanelWithIndex(int index) {
            for (int i = 0; i < _panelSelectors.Length; i++) {
                _statisticPanels[i].SetActive(i == index ? true : false);
            }
        }

        void SetResourceIcons() {
            for (int i = 1; i < 17; i++) {
                if (_resourceProductionIcons[i] != null) {
                    _resourceProductionIcons[i].SetBackgroundSprite(_resourceInfos[i].Icon);
                }
                if (_resourceStorageIcons[i] != null) {
                    _resourceStorageIcons[i].SetBackgroundSprite(_resourceInfos[i].Icon);
                }
            }
        }

        void SetEconomyInfo() {
            _resourceInfos = new RessourceInfo[17];
            for (int i = 1; i < _resourceInfos.Length; i++) {
                _resourceInfos[i] = EconemySystemInfo.Instanz.GetRessourceDescription((Ressources)i);
            }
            _storageData = new RessourcesValue[17];
            for (int i = 0; i < _storageData.Length; i++) {
                _storageData[i] = EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue((Ressources)i);
            }
            _consumptionData = new RessourcesValue[17];
            for (int i = 0; i < _consumptionData.Length; i++) {
                _consumptionData[i] = EconemySystemInfo.Instanz.PlayerList[0].GetTotalConsumption((Ressources)i);
            }
            _productionData = new RessourcesValue[17];
            for (int i = 0; i < _productionData.Length; i++) {
                _productionData[i] = EconemySystemInfo.Instanz.PlayerList[0].GetTotalProduction((Ressources)i);
            }
        }

        void UpdateResourceInfo() {
            for (int i = 1; i < 17; i++) {
                if (_resourceStorageBalances != null && _resourceStorageBalances[i] != null) {
                    _resourceStorageBalances[i].text = $"{_storageData[i].Value.ToString("F0")} | {_storageData[i].MaxValue.ToString("F0")}";
                }
                if (_resourceStorageBars != null && _resourceStorageBars[i] != null) {
                    _resourceStorageBars[i].SetHeight(Mathf.RoundToInt(Mathf.Clamp(_storageData[i].Value/ _storageData[i].MaxValue * 100, 0, 100)), LengthType.Percent);
                }
                if (_resourceProductionBalances != null && _resourceProductionBalances[i] != null) {
                    _resourceProductionBalances[i].text = $"{(_productionData[i].Value * 60).ToString("F0")}";
                }
                if (_resourceProductionBars != null && _resourceProductionBars[i] != null) {
                    _resourceProductionBars[i].SetWidth(_productionData[i].Value > 0 ? 100 : 0, LengthType.Percent);
                }
                if (_resourceConsumptionBalances != null && _resourceConsumptionBalances[i] != null) {
                    _resourceConsumptionBalances[i].text = $"{(_consumptionData[i].Value * 60).ToString("F0")}";
                }
                if (_resourceConsumptionBars != null && _resourceConsumptionBars[i] != null) {
                    _resourceConsumptionBars[i].SetWidth(_consumptionData[i].Value > 0 ? 100 : 0, LengthType.Percent);
                }
            }
        }

        void RegisterCallbacks() {
            _panelSelectors[0].clicked += delegate { SetPanelWithIndex(0); };
            _panelSelectors[1].clicked += delegate { SetPanelWithIndex(1); };

            _closeButton.clicked += delegate { _root.SetActive(false); };
        }

        void UnregisterCallbacks() {
            _panelSelectors[0].clicked -= delegate { SetPanelWithIndex(0); };
            _panelSelectors[1].clicked -= delegate { SetPanelWithIndex(1); };

            _closeButton.clicked -= delegate { _root.SetActive(false); };
        }
    }
}