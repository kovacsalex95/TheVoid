using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI._Scripts.Objects
{
    public class UIPanel : UIElement
    {
        UnityEngine.UI.Image backgroundImage;

        public Color BackColor { get => backgroundImage.color; set { backgroundImage.color = value; } }
        public Sprite BackImage { get => backgroundImage.sprite; set { backgroundImage.sprite = value; } }
        public UnityEngine.UI.Image.Type BackImageType { get => backgroundImage.type; set { backgroundImage.type = value; } }

        public Transform Container = null;

        public override string ElementName => "Panel";

        protected override void CustomComponents(GameObject elementObject)
        {
            backgroundImage = elementObject.AddComponent<UnityEngine.UI.Image>();
            backgroundImage.color = Skin.Panel.BackgroundColor;
            backgroundImage.sprite = Skin.Panel.BackgroundImage;
            backgroundImage.type = Skin.Panel.BackgroundSlicing;
            backgroundImage.raycastTarget = false;

            Container = transform;
        }
    }
}
