using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class WarningDisplayHandler : MonoBehaviour {
        [SerializeField] LocalizedStringTable warningTable = null;

        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        Label _warningLabel;
        Button _closeButton;
        VisualElement _warningIcon;

        StringTable _currentTable;

        public static VisualElement Root { get { return _root; } }

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("warningDisplay");

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;

            warningTable.TableChanged += OnTableChanged;
        }

        void OnDisable() {
            warningTable.TableChanged -= OnTableChanged;
            UnregisterCallbacks();
        }

        void OnTableChanged(StringTable table) {
            var op = warningTable.GetTableAsync();
            op.Completed -= OnTableLoaded;
            op.Completed += OnTableLoaded;
        }

        void OnTableLoaded(AsyncOperationHandle<StringTable> op) => _currentTable = op.Result;

        void Init() {
            _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("warningDisplay");

            _warningLabel = _root.Q<Label>("label_warningMessage");
            _closeButton = _root.Q<Button>("button_closeWarning");
            _warningIcon = _root.Q<VisualElement>("warningIcon");

            UnregisterCallbacks();
            RegisterCallbacks();

            _root.SetActive(false);
        }

        public void SetWarningDisplay(string key, WarningType type = WarningType.Info) {
            StringTableEntry entry = _currentTable.GetEntry(key);
            _warningLabel.text = entry != null ? entry.Value : string.Empty;
            _warningIcon.SetActive(type == WarningType.Warning);
            _root.SetActive(true);
        }

        public void SetWarningDisplay(InteractionState state) {
            switch (state) {
                case InteractionState.BuildMoving:
                    SetWarningDisplay("moveMode");
                    break;
                case InteractionState.BuildDeleting:
                    SetWarningDisplay("demolishMode");
                    break;
                case InteractionState.BuildSingle:
                    SetWarningDisplay("buildingMode");
                    break;
                case InteractionState.BuildMultiple:
                    SetWarningDisplay("buildingMode");
                    break;
                default:
                    _root.SetActive(false);
                    break;
            }
        }

        void RegisterCallbacks() => _closeButton.clicked += delegate { _root.SetActive(false); };

        void UnregisterCallbacks() => _closeButton.clicked -= delegate { _root.SetActive(false); };
    }

    public enum WarningType
    {
        Info,
        Warning,
    }
}