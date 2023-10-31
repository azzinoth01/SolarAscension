/// void* src = https://gist.github.com/andrew-raphael-lukasik/72a4d3d14dd547a1d61ae9dc4c4513da
///
/// Copyright (C) 2022 Andrzej Rafa? ?ukasik (also known as: Andrew Raphael Lukasik)

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

// NOTE: this class assumes that you designate StringTable keys in label fields (as seen in Label, Button, etc)
// and start them all with '#' char (so other labels will be left be)
// example: https://i.imgur.com/H5RUIej.gif

namespace SolarAscension {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIDocument))]
    public class UIDocumentLocalization : MonoBehaviour {

        [SerializeField] LocalizedStringTable _table = null;
        public UIDocument _document;

        [HideInInspector]
        public StringTable currentTable;

        /// <summary> Executed after hierarchy is cloned fresh and translated. </summary>
        public event System.Action onCompleted = () => { };


        void OnEnable() => _table.TableChanged += OnTableChanged;

        void OnDisable() => _table.TableChanged -= OnTableChanged;

        public bool TryGetEntry(string key, out StringTableEntry entry) {
            entry = currentTable.GetEntry(key);
            return entry == null ? false : true;
        }

        void OnTableChanged(StringTable table) {
            var root = _document.rootVisualElement;
            root.Clear();
            _document.visualTreeAsset.CloneTree(root);

            var op = _table.GetTableAsync();
            op.Completed -= OnTableLoaded;
            op.Completed += OnTableLoaded;
        }

        void OnTableLoaded(AsyncOperationHandle<StringTable> op) {
            currentTable = op.Result;
            var root = _document.rootVisualElement;

            LocalizeChildrenRecursively(root);
            SetTooltipsRecursively(root);
            SetSFXRecursively(root);
            onCompleted();

            root.MarkDirtyRepaint();
        }

        void Localize(VisualElement next) {
            if (typeof(TextElement).IsInstanceOfType(next)) {
                TextElement textElement = (TextElement)next;
                string key = textElement.text;
                if (!string.IsNullOrEmpty(key) && key[0] == '#') {
                    key = key.TrimStart('#');
                    StringTableEntry entry = currentTable[key];
                    if (entry != null)
                        textElement.text = entry.LocalizedValue;
                    else
                        Debug.LogWarning($"No {currentTable.LocaleIdentifier.Code} translation for key: '{key}'");
                }
            }
        }

        void LocalizeChildrenRecursively(VisualElement element) {
            VisualElement.Hierarchy elementHierarchy = element.hierarchy;
            int numChildren = elementHierarchy.childCount;
            for (int i = 0; i < numChildren; i++) {
                VisualElement child = elementHierarchy.ElementAt(i);
                Localize(child);
            }
            for (int i = 0; i < numChildren; i++) {
                VisualElement child = elementHierarchy.ElementAt(i);
                VisualElement.Hierarchy childHierarchy = child.hierarchy;
                int numGrandChildren = childHierarchy.childCount;
                if (numGrandChildren != 0)
                    LocalizeChildrenRecursively(child);
            }
        }

        void SetTooltipsRecursively(VisualElement element) {
            VisualElement.Hierarchy elementHierarchy = element.hierarchy;
            int numChildren = elementHierarchy.childCount;
            for (int i = 0; i < numChildren; i++) {
                VisualElement child = elementHierarchy.ElementAt(i);
                if (child.tooltip != null) {
                    child.AddManipulator(new TooltipManipulator());
                }
            }
            for (int i = 0; i < numChildren; i++) {
                VisualElement child = elementHierarchy.ElementAt(i);
                VisualElement.Hierarchy childHierarchy = child.hierarchy;
                int numGrandChildren = childHierarchy.childCount;
                if (numGrandChildren != 0)
                    SetTooltipsRecursively(child);
            }
        }

        void SetSFXRecursively(VisualElement element) {
            VisualElement.Hierarchy elementHierarchy = element.hierarchy;
            int numChildren = elementHierarchy.childCount;
            for (int i = 0; i < numChildren; i++) {
                VisualElement child = elementHierarchy.ElementAt(i);
                if(child is Button) {
                    child.AddManipulator(new SFXManipulator());
                }
            }
            for (int i = 0; i < numChildren; i++) {
                VisualElement child = elementHierarchy.ElementAt(i);
                VisualElement.Hierarchy childHierarchy = child.hierarchy;
                int numGrandChildren = childHierarchy.childCount;
                if(numGrandChildren != 0) {
                    SetSFXRecursively(child);
                }
            }
        }
    }
}

