using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI._Scripts.Objects
{
    class UIScrollPanel : UIPanel
    {
        public GroupOrientation Orientation = GroupOrientation.Vertical;

        ScrollRect scrollArea;

        public override string ElementName => "GroupPanel";

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
            viewPortObject.transform.parent = elementObject.transform;
            viewPortObject.transform.localPosition = viewPortObject.transform.localEulerAngles = Vector3.zero;
            viewPortObject.transform.localScale = Vector3.one;

            RectTransform viewPortRect = viewPortObject.AddComponent<RectTransform>();
            viewPortRect.SetTop(0); viewPortRect.SetBottom(0); viewPortRect.SetLeft(0); viewPortRect.SetRight(0);
            viewPortRect.pivot = new Vector2(0, 1f);
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

            // Scrollbar
            GameObject scrollbarObject = new GameObject("Scrollbar");
            scrollbarObject.transform.parent = elementObject.transform;
            scrollbarObject.transform.localPosition = scrollbarObject.transform.localEulerAngles = Vector3.zero;
            scrollbarObject.transform.localScale = Vector3.one;

            RectTransform scrollbarRect = scrollbarObject.AddComponent<RectTransform>();
            scrollbarRect.pivot = Orientation == GroupOrientation.Horizontal ? new Vector2(0.5f, 0f) : new Vector2(1f, 1f);
            if (Orientation == GroupOrientation.Vertical)
            {
                scrollbarRect.SetRight(0); scrollbarRect.SetTop(0); scrollbarRect.SetBottom(0); scrollbarRect.SetWidth(20);
                scrollbarRect.anchorMin = new Vector2(1, 0);
                scrollbarRect.anchorMax = new Vector2(1, 1);
            }
            else
            {
                scrollbarRect.SetLeft(0); scrollbarRect.SetTop(0); scrollbarRect.SetBottom(0); scrollbarRect.SetHeight(20);
                scrollbarRect.anchorMin = new Vector2(0, 0);
                scrollbarRect.anchorMin = new Vector2(1, 0);
            }

            Scrollbar scrollbarComponent = scrollbarObject.AddComponent<Scrollbar>();
            scrollbarComponent.transition = Selectable.Transition.ColorTint;

            ColorBlock scrollbarColors = new UnityEngine.UI.ColorBlock();
            scrollbarColors.normalColor = Skin.Scrollbar.NormalColor;
            scrollbarColors.highlightedColor = scrollbarColors.selectedColor = Skin.Scrollbar.HoverColor;
            scrollbarColors.pressedColor = Skin.Scrollbar.PressedColor;
            scrollbarColors.disabledColor = Skin.Scrollbar.DisabledColor;
            scrollbarColors.fadeDuration = Skin.Scrollbar.FadeDuration;
            scrollbarColors.colorMultiplier = 5f;

            scrollbarComponent.colors = scrollbarColors;

            scrollbarComponent.direction = Orientation == GroupOrientation.Horizontal ? Scrollbar.Direction.LeftToRight : Scrollbar.Direction.BottomToTop;

            // Scrollbar sliding area
            GameObject slidingAreaObject = new GameObject("Sliding area");
            slidingAreaObject.transform.parent = scrollbarObject.transform;
            slidingAreaObject.transform.localPosition = slidingAreaObject.transform.localEulerAngles = Vector3.zero;
            slidingAreaObject.transform.localScale = Vector3.one;

            RectTransform slidingAreaRect = slidingAreaObject.AddComponent<RectTransform>();
            slidingAreaRect.pivot = new Vector2(0.5f, 0.5f);
            slidingAreaRect.anchorMin = Vector2.zero;
            slidingAreaRect.anchorMax = Vector2.one;
            slidingAreaRect.SetLeft(0); slidingAreaRect.SetRight(0); slidingAreaRect.SetTop(0); slidingAreaRect.SetBottom(0);

            // Scrollbar handle
            GameObject handleObject = new GameObject("Handle");
            handleObject.transform.parent = slidingAreaObject.transform;
            handleObject.transform.localPosition = handleObject.transform.localEulerAngles = Vector3.zero;
            handleObject.transform.localScale = Vector3.one;

            RectTransform handleRect = handleObject.AddComponent<RectTransform>();
            handleRect.pivot = new Vector2(0.5f, 0.5f);
            handleRect.SetWidth(Orientation == GroupOrientation.Vertical ? 20 : 40);
            handleRect.SetHeight(Orientation == GroupOrientation.Vertical ? 40 : 20);

            Image handleImage = handleObject.AddComponent<Image>();
            handleImage.sprite = Skin.Scrollbar.HandleImage;
            handleImage.type = Skin.Scrollbar.HandleSlicing;
            handleImage.color = Skin.Scrollbar.NormalColor;

            // Assign things
            scrollArea.content = contentRect;
            scrollArea.viewport = viewPortRect;

            scrollbarComponent.handleRect = handleRect;
            scrollbarComponent.targetGraphic = handleImage;

            if (Orientation == GroupOrientation.Horizontal)
            {
                scrollArea.horizontalScrollbar = scrollbarComponent;
                scrollArea.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
                scrollArea.horizontalScrollbarSpacing = 0;
                scrollArea.horizontalScrollbar.LayoutComplete();
            }
            else
            {
                scrollArea.verticalScrollbar = scrollbarComponent;
                scrollArea.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
                scrollArea.verticalScrollbarSpacing = 0;
                scrollArea.verticalScrollbar.LayoutComplete();
            }

            handleRect.SetLeft(0); handleRect.SetRight(0); handleRect.SetTop(0); handleRect.SetBottom(0);

            this.Container = contentObject.transform;
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
