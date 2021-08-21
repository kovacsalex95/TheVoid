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
        public int ID { get; private set; }
        string buttonLabel;
        UIButtonIcon buttonIcon;
        UILabel label;
        UIImage iconImage;
        UILabel iconIcon;
        AdvancedButton button;

        public EventHandler<ClickedEventArgs> Clicked;

        public UIButton(int id, string label, Sprite image = null)
        {
            ID = id;
            buttonLabel = label;
            buttonIcon = new UIButtonIcon(image);
        }
        public UIButton(int id, string label, string icon = null)
        {
            ID = id;
            buttonLabel = label;
            buttonIcon = new UIButtonIcon(icon);
        }

        public bool HasIcon => buttonIcon != null;

        public string Text { get => label.Text; set { label.Text = value; } }



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
            label = new UILabel(buttonLabel);
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

            // Button icon component
            if (buttonIcon != null && buttonIcon.Type != UIButtonIcon.UIButtonIconType.None)
            {
                if (buttonIcon.Type == UIButtonIcon.UIButtonIconType.Image)
                {
                    iconImage = new UIImage(buttonIcon.Image);
                    iconImage.HOrientation = Skin.Button.IconAlign;
                    iconImage.VOrientation = VerticalOrientation.Center;
                    iconImage.Offsets.Top = iconImage.Offsets.Bottom = iconMargin;
                    if (Skin.Button.IconAlign == HorizontalOrientation.Left)
                        iconImage.Offsets.Left = iconMargin;
                    else
                        iconImage.Offsets.Right = iconMargin;
                    iconImage.Offsets.Width = iconSize;
                    iconImage.Build(transform);
                }
                else
                {
                    iconIcon = new UILabel(buttonIcon.Icon);
                    iconIcon.HOrientation = Skin.Button.IconAlign;
                    iconIcon.VOrientation = VerticalOrientation.Center;
                    iconIcon.Offsets.Top = iconIcon.Offsets.Bottom = iconMargin;
                    if (Skin.Button.IconAlign == HorizontalOrientation.Left)
                        iconIcon.Offsets.Left = iconMargin;
                    else
                        iconIcon.Offsets.Right = iconMargin;
                    iconIcon.Offsets.Width = iconSize;
                    iconIcon.Build(transform);
                    iconIcon.TextAlignH = HorizontalOrientation.Center;
                    iconIcon.TextAlignV = VerticalOrientation.Center;
                    iconIcon.FontSize = iconIcon.FontSize * 0.8f;
                    iconIcon.Color = Skin.Button.TextColor;
                }
            }
        }

        

        private void ButtonClick()
        {
            EventSystem.current.SetSelectedGameObject(null);

            if (Clicked != null)
                Clicked.Invoke(this, new ClickedEventArgs(ID, buttonLabel));
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

            if (iconIcon != null)
                iconIcon.Color = label.Color;
        }

        public class ClickedEventArgs : EventArgs
        {
            public int ID;
            public string Label;

            public ClickedEventArgs(int ID, string Label)
            {
                this.ID = ID;
                this.Label = Label;
            }
        }

        public class UIButtonIcon
        {
            public enum UIButtonIconType
            {
                None = 0,
                Image = 1,
                Icon = 2
            }

            public Sprite image = null;
            public string icon = null;

            public UIButtonIcon(Sprite image)
            {
                this.image = image;
                this.icon = null;
            }
            public UIButtonIcon(string icon)
            {
                this.icon = icon;
                this.image = null;
            }

            public Sprite Image
            {
                get => image;
                set
                {
                    image = value;
                    icon = null;
                }
            }

            public string Icon
            {
                get => icon;
                set
                {
                    icon = value;
                    image = null;
                }
            }

            public UIButtonIconType Type
            {
                get
                {
                    if (image == null && icon == null)
                        return UIButtonIconType.None;
                    if (image == null)
                        return UIButtonIconType.Icon;
                    return UIButtonIconType.Image;
                }
            }
        }
    }
}
