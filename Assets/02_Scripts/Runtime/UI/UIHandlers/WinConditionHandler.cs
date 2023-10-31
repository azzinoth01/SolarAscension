using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class WinConditionHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        [SerializeField] Quest triggerQuest;

        Button _closeWinScreenButton;

        public VisualElement Root { get { return _root; } }

        private void OnEnable()  {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        private void OnDisable() => UnregisterCallbacks();

        void Init() {
            _closeWinScreenButton = _root.Q<Button>("button_continue");

            _root.SetActive(false);

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        void RegisterCallbacks() {
            _closeWinScreenButton.clicked += delegate { _root.SetActive(false); };
            triggerQuest.goal.OnCompletion += delegate { _root.SetActive(true); };
        }

        void UnregisterCallbacks() {
            _closeWinScreenButton.clicked -= delegate { _root.SetActive(false); };
            triggerQuest.goal.OnCompletion -= delegate { _root.SetActive(true); };
        }
    }
}