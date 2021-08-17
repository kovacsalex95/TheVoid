using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI._Scripts.Objects
{
    class UIScrollPanel : UIPanel
    {
        public GroupOrientation Orientation = GroupOrientation.Vertical;

        ScrollRect scrollArea;
        //Scrollbar scrollBar;

        protected override string ElementName => "GroupPanel";

        protected override void CustomComponents(GameObject elementObject)
        {
            base.CustomComponents(elementObject);

            // Scroll area
            scrollArea = elementObject.AddComponent<ScrollRect>();
            scrollArea.vertical = Orientation == GroupOrientation.Vertical;
            scrollArea.horizontal = Orientation == GroupOrientation.Horizontal;
            scrollArea.movementType = ScrollRect.MovementType.Clamped;
            scrollArea.inertia = false;
            scrollArea.decelerationRate = 0f;
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
            contentRect.pivot = Orientation == GroupOrientation.Horizontal ? new Vector2(0, 0.5f) : new Vector2(0.5f, 1);
            if (Orientation == GroupOrientation.Vertical)
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
            CustomContentComponents(contentObject);

            // Size fitter
            ContentSizeFitter sizeFitter = contentObject.AddComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = Orientation == GroupOrientation.Horizontal ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;
            sizeFitter.verticalFit = Orientation == GroupOrientation.Vertical ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;

            // Assign content rect to scrollrect
            scrollArea.content = contentRect;

            this.Container = contentObject.transform;

            // Scrollbar
            //scrollBar = elementObject.AddComponent<Scrollbar>();
        }

        protected virtual void CustomContentComponents(GameObject contentObject)
        {

        }
    }

    public enum GroupOrientation
    {
        Horizontal = 0,
        Vertical = 1
    }
}
