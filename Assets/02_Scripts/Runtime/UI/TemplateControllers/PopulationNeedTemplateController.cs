using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension
{
    public class PopulationNeedTemplateController
    {
        VisualElement _root;
        VisualElement _resourceIcon, _fulfillmentBar;

        NeedsAndWants _need;
        List<NeedsAndWants> _source;

        public PopulationNeedTemplateController(VisualElement root, NeedsAndWants need, List<NeedsAndWants> source) {
            _root = root;
            _need = need;
            _source = source;

            Initialize();
        }

        void Initialize() {
            _resourceIcon = _root.Q<VisualElement>("resource_icon");
            _fulfillmentBar = _root.Q<VisualElement>("fullfillment_fill");

            SetNeedDisplay();
        }

        void SetNeedDisplay() {
            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(_need.Value.Ressources);

            _resourceIcon.SetBackgroundSprite(info.Icon);

            _root.SetWidth(50, LengthType.Percent);
            _root.SetMinAndMaxHeight(100 / (_source.Count % 2 == 0 ? _source.Count / 2 : _source.Count - (_source.Count - (1 + (_source.Count / 2)))), LengthType.Percent);

            UpdateFulfillmentBar();
        }

        public void UpdateFulfillmentBar() {
            _fulfillmentBar.SetWidth(Mathf.Clamp((int)_need.Fulfillvalue, 0, 100), LengthType.Percent);
            _fulfillmentBar.style.backgroundColor = Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(0, 100, _need.Fulfillvalue));
        }
    }
}