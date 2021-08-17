using Assets.UI._Scripts.Misc;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI._Scripts.Objects
{
    class UIGroupPanel : UIScrollPanel
    {
        public float Spacing = -1;

        protected override void CustomContentComponents(GameObject contentObject)
        {
            float spacing = Spacing < 0 ? Skin.GroupPanel.ElementSpacing : Spacing;

            if (Orientation == GroupOrientation.Vertical)
            {
                VerticalLayoutGroup verticalLayoutGroup = contentObject.AddComponent<VerticalLayoutGroup>();
                verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
                verticalLayoutGroup.childControlHeight = false;
                verticalLayoutGroup.childControlWidth = true;
                verticalLayoutGroup.childForceExpandHeight = false;
                verticalLayoutGroup.padding = UIUtil.Padding(spacing);
                verticalLayoutGroup.spacing = spacing;
            }
            else
            {
                HorizontalLayoutGroup horizontalLayoutGroup = contentObject.AddComponent<HorizontalLayoutGroup>();
                horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
                horizontalLayoutGroup.childControlHeight = true;
                horizontalLayoutGroup.childControlWidth = false;
                horizontalLayoutGroup.childForceExpandWidth = false;
                horizontalLayoutGroup.padding = UIUtil.Padding(spacing);
                horizontalLayoutGroup.spacing = spacing;
            }
        }
    }
}
