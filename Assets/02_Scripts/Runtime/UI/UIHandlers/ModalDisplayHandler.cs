using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class ModalDisplayHandler : MonoBehaviour {
        static VisualElement _root;
        static UIDocumentLocalization _uIDocumentLocalization;

        static Label _modalDisplayLabel;
        Button _confirmButton, _cancelButton;

        public static VisualElement Root { get { return _root; } }

        public delegate void OnConfirm();
        public static event OnConfirm OnConfirmEvent;

        void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        private void OnDisable() => UnregisterCallbacks();

        void Init() {
            _modalDisplayLabel = _root.Q<Label>("label_modalMessage");
            _confirmButton = _root.Q<Button>("button_confirm");
            _cancelButton = _root.Q<Button>("button_cancel");

            _root.SetActive(false);

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        public static void SetModalDisplay(string tableKey, OnConfirm function) {
            if (_uIDocumentLocalization.currentTable[tableKey] != null) {
                _modalDisplayLabel.text = _uIDocumentLocalization.currentTable[tableKey].LocalizedValue;
            }
            else {
                Debug.LogError("No ModalTable Entry found for given Key");
            }

            OnConfirmEvent += function;
            _root.SetActive(true);
        }

        void Confirm() {
            OnConfirmEvent?.Invoke();
            CloseModalDisplay();
        }

        void CloseModalDisplay() {
            _root.SetActive(false); ;
            OnConfirmEvent = null;
        }

        void RegisterCallbacks() {
            _confirmButton.clicked += Confirm;
            _cancelButton.clicked += CloseModalDisplay;
        }

        void UnregisterCallbacks() {
            _confirmButton.clicked -= Confirm;
            _cancelButton.clicked -= CloseModalDisplay;
        }
    }
}