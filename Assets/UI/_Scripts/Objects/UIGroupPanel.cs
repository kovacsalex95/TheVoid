using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI._Scripts.Objects
{
    class UIGroupPanel : UIPanel
    {
        public GroupOrientation GroupOrientation = GroupOrientation.Vertical;
        public float GroupSpacing = -1;

        ScrollRect scrollArea;
        Scrollbar scrollBar;

        protected override string ElementName => "GroupPanel";

        protected override void CustomComponents(GameObject elementObject)
        {
            base.CustomComponents(elementObject);

            float spacing = GroupSpacing < 0 ? Skin.GroupPanel.ElementSpacing : GroupSpacing;

            // Scroll area
            scrollArea = elementObject.AddComponent<ScrollRect>();
            scrollArea.vertical = GroupOrientation == GroupOrientation.Vertical;
            scrollArea.horizontal = GroupOrientation == GroupOrientation.Horizontal;
            scrollArea.movementType = ScrollRect.MovementType.Elastic;
            scrollArea.inertia = true;
            scrollArea.decelerationRate = 0.135f;
            scrollArea.scrollSensitivity = 6f;

            // Viewport
            GameObject viewPortObject = new GameObject("Viewport");
            viewPortObject.transform.parent = transform;
            viewPortObject.transform.localPosition = viewPortObject.transform.localEulerAngles = Vector3.zero;
            viewPortObject.transform.localScale = Vector3.one;

            RectTransform viewPortRect = viewPortObject.AddComponent<RectTransform>();
            viewPortRect.SetTop(0); viewPortRect.SetBottom(0); viewPortRect.SetLeft(0); viewPortRect.SetRight(0);
            viewPortRect.anchorMin = Vector2.zero;
            viewPortRect.anchorMax = Vector2.one;

            Image viewPortBackground = viewPortObject.AddComponent<Image>();
            viewPortBackground.color = Color.white;
            viewPortBackground.maskable = true;

            Mask viewPortMask = viewPortObject.AddComponent<Mask>();
            viewPortMask.showMaskGraphic = false;

            // Content
            GameObject contentObject = new GameObject("Content");
            contentObject.transform.parent = viewPortObject.transform;
            contentObject.transform.localPosition = contentObject.transform.localEulerAngles = Vector3.zero;
            contentObject.transform.localScale = Vector3.one;

            RectTransform contentRect = contentObject.AddComponent<RectTransform>();
            contentRect.pivot = GroupOrientation == GroupOrientation.Horizontal ? new Vector2(0, 0.5f) : new Vector2(0.5f, 1);
            if (GroupOrientation == GroupOrientation.Vertical)
            {
                contentRect.SetLeft(0); contentRect.SetTop(0); contentRect.SetRight(0); contentRect.SetHeight(100);
                contentRect.anchorMin = new Vector2(0, 1);
                contentRect.anchorMax = new Vector2(1, 1);
            }
            else
            {
                contentRect.SetLeft(0); contentRect.SetTop(0); contentRect.SetBottom(0); contentRect.SetWidth(100);
                contentRect.anchorMin = new Vector2(0, 0);
                contentRect.anchorMax = new Vector2(0, 1);
            }

            // Layout group
            if (GroupOrientation == GroupOrientation.Vertical)
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

            // Size fitter
            ContentSizeFitter sizeFitter = contentObject.AddComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = GroupOrientation == GroupOrientation.Horizontal ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;
            sizeFitter.verticalFit = GroupOrientation == GroupOrientation.Vertical ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;

            // Assign content rect to scrollrect
            scrollArea.content = contentRect;

            this.Container = contentObject.transform;

            // Scrollbar
            scrollBar = elementObject.AddComponent<Scrollbar>();
            //scrollBar.
        }
    }

    public enum GroupOrientation
    {
        Horizontal = 0,
        Vertical = 1
    }
}
