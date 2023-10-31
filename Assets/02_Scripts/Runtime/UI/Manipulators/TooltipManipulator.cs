using UnityEngine.UIElements;

namespace SolarAscension {
    public class TooltipManipulator : Manipulator {
        protected override void RegisterCallbacksOnTarget() {
            target.RegisterCallback<MouseEnterEvent>(MouseEnter);
            target.RegisterCallback<MouseLeaveEvent>(MouseLeave);
        }

        protected override void UnregisterCallbacksFromTarget() {
            target.UnregisterCallback<MouseEnterEvent>(MouseEnter);
            target.UnregisterCallback<MouseLeaveEvent>(MouseLeave);
        }

        void MouseEnter(MouseEnterEvent e) => TooltipHandler.SetTooltip(target, true);

        void MouseLeave(MouseLeaveEvent e) => TooltipHandler.SetTooltip(target, false);
    }
}