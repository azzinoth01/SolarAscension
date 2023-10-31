using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class LoadingScreenHandler : MonoBehaviour {
        static VisualElement _root;
        UIDocumentLocalization _uIDocumentLocalization;

        static VisualElement _progressBar;

        public static VisualElement Root { get { return _root; } }

        private void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void Init() {
            _progressBar = _root.Q<VisualElement>("progressBar");
            _root.SetActive(false);
        }

        public static void SetProgress(float progress) => _progressBar.SetWidth(Mathf.RoundToInt(progress * 100f), LengthType.Percent);
    }
}