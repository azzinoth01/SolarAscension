using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

namespace SolarAscension {
    public class ResourceDisplayController {
        VisualElement _template, _resourceIcon;
        Label _resoureLabel;

        List<RessourcesValue> _source;
        RessourcesValue _resourceValue;

        int _fontSize;
        bool _checkForResource, _setBackgroundColor;

        public int valueMultiplier = 1;

        public ResourceDisplayController(VisualElement template, RessourcesValue resourceValue, List<RessourcesValue> source, bool checkForResource = false, int fontSize = 20, bool setBackgroundColor = false) {
            _template = template;
            _source = source;
            _resourceValue = resourceValue;
            _checkForResource = checkForResource;
            _fontSize = fontSize;
            _setBackgroundColor = setBackgroundColor;
            valueMultiplier = 1;

            Initialize();
        }

        void Initialize() {
            _resourceIcon = _template.Q<VisualElement>("resourceIcon");
            _resoureLabel = _template.Q<Label>("resourceLabel");

            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(_resourceValue.Ressources);
            _resourceIcon.SetBackgroundSprite(info.Icon);
            _resoureLabel.text = (_resourceValue.Value * valueMultiplier).ToString("F0");
            _resoureLabel.style.fontSize = _fontSize;

            _template.SetWidth(50, LengthType.Percent);
            _template.SetHeight(100 / (_source.Count % 2 == 0 ? _source.Count / 2 : _source.Count - (_source.Count - (1 + (_source.Count / 2)))), LengthType.Percent);

            if(_checkForResource && !CheckHasResource()) {
                _resoureLabel.style.color = Color.red;
                _resourceIcon.style.unityBackgroundImageTintColor = Color.red;
            }

            if (_setBackgroundColor){
                _template.style.backgroundColor = new Color(0f, 0f, 0f, 0.8f);
            }
        }

        public void UpdateResourceData() {
            if (_checkForResource) {
                _resoureLabel.style.color = !CheckHasResource() ? Color.red : Color.white;
                _resourceIcon.style.unityBackgroundImageTintColor = !CheckHasResource() ? Color.red : Color.white;
                _resoureLabel.text = (_resourceValue.Value * valueMultiplier).ToString("F0");
            }
        }

        bool CheckHasResource() {
            RessourcesValue resourceStored = EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(_resourceValue.Ressources);
            return resourceStored.Value >= _resourceValue.Value;
        }
    }
}