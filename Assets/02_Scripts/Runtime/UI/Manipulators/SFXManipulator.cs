using UnityEngine.UIElements;

namespace SolarAscension {
    public class SFXManipulator : Manipulator {
        protected override void RegisterCallbacksOnTarget() {
            target.RegisterCallback<MouseEnterEvent>(MouseEnter);
            target.RegisterCallback<ClickEvent>(MouseClick);
        }

        protected override void UnregisterCallbacksFromTarget() {
            target.UnregisterCallback<MouseEnterEvent>(MouseEnter);
            target.UnregisterCallback<ClickEvent>(MouseClick);
        }

        void MouseEnter(MouseEnterEvent e) => AudioManager.Instance.Play("ui_hover");
        void MouseClick(ClickEvent e) => AudioManager.Instance.Play("ui_click");
    }
}