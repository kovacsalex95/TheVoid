using Assets.UI._Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.UI._Scripts.Objects
{
    class UIButton : UIElement
    {
        string buttonText;
        Sprite buttonIcon = null;
        UILabel label;
        UIImage iconImage;
        AdvancedButton button;

        public EventHandler Clicked;

        public UIButton(string text, Sprite icon = null)
        {
            buttonText = text;
            buttonIcon = icon;
        }

        public bool HasIcon => buttonIcon != null;

        public string Text { get => label.Text; set { label.Text = value; } }

        public Sprite Icon { get => iconImage.Image; set { iconImage.Image = value; } }

        public HorizontalOrientation TextAlign { get => label.TextAlignH; set { label.TextAlignH = value; } }

        protected override string ElementName => "Button";

        protected override void CustomComponents(GameObject elementObject)
        {
            float iconMargin = Skin.Button.Gaps * Transform.sizeDelta.y;
            float iconSize = Transform.sizeDelta.y - iconMargin * 2;

            // Image component
            UnityEngine.UI.Image backgroundImage = elementObject.AddComponent<UnityEngine.UI.Image>();
            backgroundImage.color = UIUtil.ColorOpacity(Skin.Button.BackgroundColor);
            backgroundImage.sprite = Skin.Button.BackgroundImage;
            backgroundImage.type = Skin.Button.BackgroundSlicing;

            // Button component
            button = elementObject.AddComponent<AdvancedButton>();
            button.onClick.AddListener(ButtonClick);
            button.transition = UnityEngine.UI.Selectable.Transition.ColorTint;

            UnityEngine.UI.ColorBlock buttonColors = new UnityEngine.UI.ColorBlock();
            buttonColors.normalColor = Skin.Button.BackgroundColor;
            buttonColors.highlightedColor = buttonColors.selectedColor = Skin.Button.HoverBackgroundColor;
            buttonColors.pressedColor = Skin.Button.PressedBackgroundColor;
            buttonColors.disabledColor = Skin.Button.DisabledBackgroundColor;
            buttonColors.fadeDuration = Skin.Button.FadeDuration;
            buttonColors.colorMultiplier = 5f;

            button.colors = buttonColors;

            button.StateChanged += ButtonStateChanged;

            // Button label (TMPro) component
            label = new UILabel(buttonText);
            label.HOrientation = Skin.Button.TextAlign;
            label.VOrientation = VerticalOrientation.Center;
            label.Offsets.Top = 0;
            label.Offsets.Bottom = 0;
            label.Offsets.Left = HasIcon && Skin.Button.IconAlign == HorizontalOrientation.Left ? Transform.sizeDelta.y : iconMargin;
            label.Offsets.Right = HasIcon && Skin.Button.IconAlign == HorizontalOrientation.Right ? Transform.sizeDelta.y : iconMargin;
            label.Build(transform);

            label.Color = Skin.Button.TextColor;
            label.TextAlignH = Skin.Button.TextAlign;
            label.TextAlignV = VerticalOrientation.Center;

            // Button image component
            if (buttonIcon != null)
            {
                iconImage = new UIImage(buttonIcon);
                iconImage.HOrientation = Skin.Button.IconAlign;
                iconImage.VOrientation = VerticalOrientation.Center;
                iconImage.Offsets.Top = iconImage.Offsets.Bottom = iconImage.Offsets.Left = iconMargin;
                iconImage.Offsets.Width = iconSize;
                iconImage.Build(transform);
            }
        }

        private void ButtonClick()
        {
            EventSystem.current.SetSelectedGameObject(null);

            if (Clicked != null)
                Clicked.Invoke(this as UIButton, new EventArgs());
        }

        private void ButtonStateChanged(object sender, EventArgs e)
        {
            if (button.State == AdvancedSelectionState.Highlighted)
                label.Color = Skin.Button.HoverTextColor;
            else if (button.State == AdvancedSelectionState.Pressed)
                label.Color = Skin.Button.PressedTextColor;
            else if (button.State == AdvancedSelectionState.Disabled)
                label.Color = Skin.Button.DisabledTextColor;
            else
                label.Color = Skin.Button.TextColor;
        }
    }
}
