using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class BuildingCostHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        [SerializeField] VisualTreeAsset resourceDisplayTemplate;

        VisualElement _buildingCostData, _buildingCostPopup;

        List<RessourcesValue> _currentCost;
        PlayerInput _playerInput;

        public static VisualElement Root { get { return _root; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        private void Update() {
            SetBuildingCostPosition();
            UpdateBuildingCostData();
        }

        void Init() {
            _playerInput = MenuManager.Instance.player.Input;

            _buildingCostPopup = _root.Q<VisualElement>("buildingCost_popup");
            _buildingCostData = _root.Q<VisualElement>("buildingCost_data");

            _root.SetActive(false);
        }

        public void SetBuildingCostData(ModuleData data) {
            if (TryGetConstructionCosts(data.buildingId, out List<RessourcesValue> constructionCosts)) {
                _root.SetActive(true);
                _buildingCostData.Clear();
                _buildingCostData.SetMinHeight(100 + 100 * ((constructionCosts.Count - 1) / 2), LengthType.Percent);
                foreach(RessourcesValue cost in constructionCosts)
                {
                    VisualElement costDisplay = resourceDisplayTemplate.Instantiate();
                    ResourceDisplayController costDisplayController = new ResourceDisplayController(costDisplay, cost, constructionCosts, true, 15, true);
                    costDisplay.userData = costDisplayController;
                    _buildingCostData.Add(costDisplay);
                }
                _currentCost = constructionCosts;
            }
        }

        void UpdateBuildingCostData() {
            if(_buildingCostData == null) {
                return;
            }
            foreach(VisualElement display in _buildingCostData.Children()) {
                ResourceDisplayController resourceDisplayController = display.userData as ResourceDisplayController;
                resourceDisplayController.UpdateResourceData();
                resourceDisplayController.valueMultiplier = BuildMultipleObjectsState.pathLength > 0 ? BuildMultipleObjectsState.pathLength : 1;
            }
        }

        void SetBuildingCostPosition() {
            if (!_root.IsActive()) {
                return;
            }
            Vector2 screenPosition = _playerInput.CursorPosition + new Vector2(0, 150);
            _buildingCostPopup.SetPosition(screenPosition, LengthType.Pixel);
        }

        bool TryGetConstructionCosts(int buildingID, out List<RessourcesValue> resourceList) {
            if (EconemySystemInfo.Instanz.GetBuildingProductionDescription(buildingID).RessourceCostList != null) {
                resourceList = EconemySystemInfo.Instanz.GetBuildingProductionDescription(buildingID).RessourceCostList;
                return true;
            }
            resourceList = new List<RessourcesValue>();
            return false;
        }
    }
}