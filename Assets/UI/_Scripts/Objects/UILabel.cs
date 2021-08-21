using Assets.UI._Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI._Scripts.Objects
{
    class UILabel : UIElement
    {
        string labelText;
        TMPro.TextMeshProUGUI labelComponent;

        public UILabel(string text)
        {
            labelText = text;
        }

        public string Text { get => labelComponent.text; set { labelComponent.SetText(value); labelComponent.UpdateIcons();  } }
        public Color Color { get => labelComponent.color; set { labelComponent.color = value; } }
        public float FontSize { get => labelComponent.fontSize; set { labelComponent.fontSize = value; } }

        public HorizontalOrientation TextAlignH
        {
            get
            {
                if (labelComponent.horizontalAlignment == TMPro.HorizontalAlignmentOptions.Left)
                    return HorizontalOrientation.Left;
                
                if (labelComponent.horizontalAlignment == TMPro.HorizontalAlignmentOptions.Right)
                    return HorizontalOrientation.Right;

                return HorizontalOrientation.Center;
            }
            set
            {
                if (value == HorizontalOrientation.Left)
                    labelComponent.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Left;

                else if (value == HorizontalOrientation.Right)
                    labelComponent.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Right;

                else
                    labelComponent.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
            }
        }
        public VerticalOrientation TextAlignV
        {
            get
            {
                if (labelComponent.verticalAlignment == TMPro.VerticalAlignmentOptions.Top)
                    return VerticalOrientation.Top;

                if (labelComponent.verticalAlignment == TMPro.VerticalAlignmentOptions.Bottom)
                    return VerticalOrientation.Bottom;

                return VerticalOrientation.Center;
            }
            set
            {
                if (value == VerticalOrientation.Top)
                    labelComponent.verticalAlignment = TMPro.VerticalAlignmentOptions.Top;

                else if (value == VerticalOrientation.Bottom)
                    labelComponent.verticalAlignment = TMPro.VerticalAlignmentOptions.Bottom;

                else
                    labelComponent.verticalAlignment = TMPro.VerticalAlignmentOptions.Middle;
            }
        }

        public override string ElementName => "Label";

        protected override void CustomComponents(GameObject elementObject)
        {
            labelComponent = elementObject.AddComponent<TMPro.TextMeshProUGUI>();
            Text = labelText;
            FontSize = Skin.FontSize;
        }
    }
}
