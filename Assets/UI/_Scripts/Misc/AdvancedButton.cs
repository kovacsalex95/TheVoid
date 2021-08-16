using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.UI._Scripts.Misc
{
    public class AdvancedButton : UnityEngine.UI.Button
    {
        public AdvancedSelectionState State { get; private set; }

        public EventHandler StateChanged;

        private void UpdateState()
        {
            State = (AdvancedSelectionState)((int)currentSelectionState);

            if (StateChanged != null)
                StateChanged.Invoke(this, new EventArgs());
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            UpdateState();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            UpdateState();
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
            UpdateState();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            UpdateState();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            UpdateState();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            UpdateState();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            UpdateState();
        }
    }

    public enum AdvancedSelectionState
    {
        Normal = 0,
        Highlighted = 1,
        Pressed = 2,
        Selected = 3,
        Disabled = 4
    }
}
