using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class TooltipHandler : MonoBehaviour {
        static VisualElement _root;
        static UIDocumentLocalization _uIDocumentLocalization;

        static VisualElement _tooltip;
        static Label _tooltipLabel;

        public static VisualElement Root { get { return _root; } }

        private void OnEnable() {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void Init() {
            _tooltip = _root.Q<VisualElement>("tooltip");
            _tooltipLabel = _root.Q<Label>("label_tooltip");

			_root.SetActive(false);
        }

        public static void SetTooltip(VisualElement target, bool enabled) {
			if (string.IsNullOrEmpty(target.tooltip)) {
				return;
			}

			_root.SetActive(enabled);

            string key = target.tooltip;
            char hint = 'B';
            float left = 0f, top = 0f;
			Rect worldBound = _tooltip.worldBound;
            //VisualElement parent = _tooltip.parent;

            if (key.Length > 2 && key[1] == ':') {
				hint = target.tooltip[0] switch {
					'B' => 'B',	'b' => 'B',
					'T' => 'T', 't' => 'T',
					'L' => 'L',	'l' => 'L',
					'R' => 'R',	'r' => 'R',
					_ => 'B'
				};

				key = key.Substring(2);
			}

			if (_uIDocumentLocalization.TryGetEntry(key, out StringTableEntry entry)) {
                _tooltipLabel.text = entry.Value;
            }

            _tooltip.schedule.Execute(() =>
            {
                switch (hint) {
                    case 'L': // left
                        left = target.worldBound.xMin - worldBound.width - 5;
                        top = target.worldBound.center.y - (worldBound.height * 0.5f);
                        break;
                    case 'R': // right
                        left = target.worldBound.xMax + 5;
                        top = target.worldBound.center.y - (worldBound.height * 0.5f);
                        break;
                    case 'T': //top
                        left = target.worldBound.center.x - (worldBound.width * 0.5f);
                        top = target.worldBound.yMin - 50;
                        break;
                    default: // bottom
                        left = target.worldBound.center.x - (worldBound.width * 0.5f);
                        top = target.worldBound.yMax + 5;
                        break;
                }

                //if(parent != null) {
                //    if (left < parent.worldBound.xMin) left = parent.worldBound.xMin;
                //    if (left + worldBound.width > parent.worldBound.xMax) left = _tooltip.parent.worldBound.xMax - _tooltip.worldBound.width;
                //    if (top < parent.worldBound.yMin) top = parent.worldBound.yMin;
                //    if (top + worldBound.height > parent.worldBound.yMax) top = _tooltip.parent.worldBound.yMax - _tooltip.worldBound.height;
                //}

                _tooltip.SetPosition(Position.Left, (int)left, LengthType.Pixel);
                _tooltip.SetPosition(Position.Top, (int)top, LengthType.Pixel);
            });
        }
    }
}