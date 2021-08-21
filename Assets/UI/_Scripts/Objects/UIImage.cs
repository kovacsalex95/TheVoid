using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI._Scripts.Objects
{
    class UIImage : UIElement
    {
        UnityEngine.UI.Image imageComponent;
        Sprite image;
        UnityEngine.UI.Image.Type imageType;

        public UIImage(Sprite image, UnityEngine.UI.Image.Type imageType = UnityEngine.UI.Image.Type.Simple)
        {
            this.image = image;
            this.imageType = imageType;
        }

        public Sprite Image { get => imageComponent.sprite; set { imageComponent.sprite = value; } }
        public UnityEngine.UI.Image.Type ImageType { get => imageComponent.type; set { imageComponent.type = value; } }

        public override string ElementName => "Image";

        protected override void CustomComponents(GameObject elementObject)
        {
            imageComponent = elementObject.AddComponent<UnityEngine.UI.Image>();
            imageComponent.type = imageType;
            imageComponent.sprite = image;
        }
    }
}
