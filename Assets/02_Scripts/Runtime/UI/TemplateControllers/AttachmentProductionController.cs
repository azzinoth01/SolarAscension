using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class AttachmentProductionController : MonoBehaviour {
        VisualElement _root;

        Building _attachment;
        List<RessourcesValue> _resourceCostList;
        List<RessourcesValue> _maintenanceData;

        VisualElement _hoverRoot, _attachmentBuildingCosts, _attachmentMaintenance, _buildingCostHeader, _maintenanceHeader;

        VisualTreeAsset _resourceDisplayTemplate;

        public AttachmentProductionController(VisualElement root, VisualElement hoverRoot) {
            _root = root;
            _hoverRoot = hoverRoot;
            _resourceDisplayTemplate = MenuManager.Instance.modulePopupHandler.ResourceDisplayTemplate;

            _attachmentBuildingCosts = _root.Q<VisualElement>("attachment_buildingCosts");
            _buildingCostHeader = _root.Q<VisualElement>("attachment_buildingCostHeader");
            _attachmentMaintenance = _root.Q<VisualElement>("attachment_maintenance");
            _maintenanceHeader = _root.Q<VisualElement>("attachment_maintenanceHeader");

            _root.SetActive(false);

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        public void SetAttachmentData(Building attachment) {
            _attachment = attachment;
            _resourceCostList = _attachment.RessourceCostList;
            _maintenanceData = GetMaintenanceData(_attachment.GetProductionInformationPerMinute());

            SetBuildingCostData();
            SetMaintenanceData();
        }

        List<RessourcesValue> GetMaintenanceData(RessourcesProduction production) {
            List<RessourcesValue> maintenanceData = new List<RessourcesValue>();
            foreach (RessourcesValue resourceValue in production.ConsumptionList) {
                RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(resourceValue.Ressources);
                if (info.Type == RessourceTyp.limitType) {
                    maintenanceData.Add(resourceValue);
                }
            }
            return maintenanceData;
        }

        void SetBuildingCostData() {
            _attachmentBuildingCosts.Clear();
            foreach (RessourcesValue value in _resourceCostList) {
                VisualElement resourceDisplay = _resourceDisplayTemplate.Instantiate();
                ResourceDisplayController displayController = new ResourceDisplayController(resourceDisplay, value, _resourceCostList, true);
                resourceDisplay.userData = displayController;
                _attachmentBuildingCosts.Add(resourceDisplay);
                resourceDisplay.pickingMode = PickingMode.Ignore;
                Label _resourceCount = resourceDisplay.Q<Label>("resourceLabel");
                _resourceCount.style.fontSize = 10;
            }
            _buildingCostHeader.SetActive(_resourceCostList.Count > 0);
            _attachmentBuildingCosts.SetMinAndMaxHeight(35 * ((_resourceCostList.Count + 1) / 2), LengthType.Percent);
        }

        void SetMaintenanceData() {
            _attachmentMaintenance.Clear();
            foreach (RessourcesValue value in _maintenanceData) {
                VisualElement resourceDisplay = _resourceDisplayTemplate.Instantiate();
                ResourceDisplayController displayController = new ResourceDisplayController(resourceDisplay, value, _maintenanceData);
                resourceDisplay.userData = displayController;
                _attachmentMaintenance.Add(resourceDisplay);
                resourceDisplay.pickingMode = PickingMode.Ignore;
                Label _resourceCount = resourceDisplay.Q<Label>("resourceLabel");
                _resourceCount.style.fontSize = 10;
            }
            _maintenanceHeader.SetActive(_maintenanceData.Count > 0);
            _attachmentMaintenance.SetMinAndMaxHeight(35 * ((_maintenanceData.Count + 1) / 2), LengthType.Percent);
        }

        void ToggleProductionData(IMouseEvent e) => _root.SetActive(e is MouseEnterEvent);

        void RegisterCallbacks() {
            _hoverRoot.RegisterCallback<MouseEnterEvent>(ToggleProductionData);
            _hoverRoot.RegisterCallback<MouseLeaveEvent>(ToggleProductionData);
        }

        void UnregisterCallbacks() {
            _hoverRoot.UnregisterCallback<MouseEnterEvent>(ToggleProductionData);
            _hoverRoot.UnregisterCallback<MouseLeaveEvent>(ToggleProductionData);
        }
    }
}