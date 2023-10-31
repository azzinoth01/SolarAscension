using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class EconomyDisplayHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        [SerializeField] Sprite populationSprite;

        Label[] _resourceLabels = new Label[17];
        VisualElement[] _resourceIcons = new VisualElement[17];
        Label _ecologistAvailableLabel;
        VisualElement _ecologistIcon;

        RessourceInfo[] _resourceInfos = new RessourceInfo[17];
        RessourcesValue[] _storageData = new RessourcesValue[17], _consumptionData = new RessourcesValue[17], _productionData = new RessourcesValue[17];

        public static VisualElement Root { get { return _root; } }
        public RessourceInfo[] ResourceInfos { get { return _resourceInfos; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("economyDisplay");

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;

            SetEconomyInfo();
        }

        void Update() => UpdateResourceBalanceDisplays();

        private void OnDisable() => UnregisterCallbacks();

        public void Init() {
            _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("economyDisplay");

            _resourceLabels = new Label[17];
            _resourceIcons = new VisualElement[17];

            _resourceLabels[(int)Ressources.Money] = _root.Q<Label>("label_moneyBalance");
            _resourceIcons[(int)Ressources.Money] = _root.Q<VisualElement>("moneyIcon");

            _resourceLabels[(int)Ressources.Aluminium] = _root.Q<Label>("label_aluminiumBalance");
            _resourceIcons[(int)Ressources.Aluminium] = _root.Q<VisualElement>("aluminiumIcon");

            _resourceLabels[(int)Ressources.Energy] = _root.Q<Label>("label_energyBalance");
            _resourceIcons[(int)Ressources.Energy] = _root.Q<VisualElement>("energyIcon");
            //Display Max - Total Consumption

            _resourceLabels[(int)Ressources.Water] = _root.Q<Label>("label_waterBalance");
            _resourceIcons[(int)Ressources.Water] = _root.Q<VisualElement>("waterIcon");

            _resourceLabels[(int)Ressources.WorkforceEcology] = _root.Q<Label>("label_workforceEcologyBalance");
            _resourceIcons[(int)Ressources.WorkforceEcology] = _root.Q<VisualElement>("workforceEcologyIcon");

            _resourceLabels[(int)Ressources.BioPolymer] = _root.Q<Label>("label_biopolymereBalance");
            _resourceIcons[(int)Ressources.BioPolymer] = _root.Q<VisualElement>("biopolymereIcon");

            //Dirty AF Implementation
            _ecologistAvailableLabel = _root.Q<Label>("label_ecologistAvailable");
            _ecologistIcon = _root.Q<VisualElement>("ecologistIcon");

            SetResourceIcons();

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        void SetResourceIcons() {
            for (int i = 1; i < _resourceIcons.Length; i++) {
                if (_resourceIcons[i] != null) {
                    _resourceIcons[i].SetBackgroundSprite(_resourceInfos[i].Icon);
                }
            }

            _ecologistIcon.SetBackgroundSprite(_resourceInfos[(int)Ressources.WorkforceEcology].Icon);
            _resourceIcons[(int)Ressources.WorkforceEcology].SetBackgroundSprite(populationSprite);
        }

        void SetEconomyInfo() {
            _resourceInfos = new RessourceInfo[17];
            for (int i = 1; i < _resourceInfos.Length; i++) {
                _resourceInfos[i] = EconemySystemInfo.Instanz.GetRessourceDescription((Ressources)i);
            }
            _storageData = new RessourcesValue[17];
            for (int i = 1; i < _storageData.Length; i++) {
                _storageData[i] = EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue((Ressources)i);
            }
            _consumptionData = new RessourcesValue[17];
            for (int i = 1; i < _consumptionData.Length; i++) {
                _consumptionData[i] = EconemySystemInfo.Instanz.PlayerList[0].GetTotalConsumption((Ressources)i);
            }
            _productionData = new RessourcesValue[17];
            for (int i = 0; i < _productionData.Length; i++) {
                _productionData[i] = EconemySystemInfo.Instanz.PlayerList[0].GetTotalProduction((Ressources)i);
            }
        }

        void UpdateResourceBalanceDisplays() {
            for (int i = 1; i < _resourceLabels.Length; i++) {
                if (_resourceLabels[i] != null) {
                    if (_resourceInfos[i].Type == RessourceTyp.valueType) {
                        _resourceLabels[i].text = _storageData[i].Value.ToString("F0");
                    }
                    else if (_resourceInfos[i].Type == RessourceTyp.limitType) {
                        if (_resourceInfos[i].Ressources == Ressources.WorkforceEcology || _resourceInfos[i].Ressources == Ressources.WorkforceEngineer) {
                            _resourceLabels[i].text = $"{_storageData[i].MaxValue.ToString("F0")}";
                        }
                        if (_resourceInfos[i].Ressources == Ressources.Energy) {
                            _resourceLabels[i].text = $"{_consumptionData[i].Value.ToString("F0")} | {_productionData[i].Value.ToString("F0")}";
                        }
                    }
                }
            }
            if(_ecologistAvailableLabel != null)
            {
                _ecologistAvailableLabel.text = $"{_storageData[(int)Ressources.WorkforceEcology].Value.ToString("F0")}";
            }
        }

        void OpenStatisticsPanel(ClickEvent e) => MenuManager.Instance.statisticsDisplayHandler.OpenPanelByIndex(0);

        void RegisterCallbacks() {
            foreach (Label label in _resourceLabels) {
                if (label != null) {
                    label.RegisterCallback<ClickEvent>(OpenStatisticsPanel);
                }
            }
            foreach (VisualElement icon in _resourceIcons) {
                if (icon != null) {
                    icon.RegisterCallback<ClickEvent>(OpenStatisticsPanel);
                }
            }
        }

        void UnregisterCallbacks() {
            foreach (Label label in _resourceLabels) {
                if (label != null) {
                    label.UnregisterCallback<ClickEvent>(OpenStatisticsPanel);
                }
            }
            foreach (VisualElement icon in _resourceIcons) {
                if (icon != null) {
                    icon.UnregisterCallback<ClickEvent>(OpenStatisticsPanel);
                }
            }
        }
    }
}