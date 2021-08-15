using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode(), RequireComponent(typeof(RectTransform))]
public abstract class UIElement : MonoBehaviour
{
    public UISkin Skin { get; private set; }

    static float[] ORIENTATION_PIVOTS = { 0f, 0.5f, 1f };
    static float[] ORIENTATION_ANCHOR_MINMAX = { 0f, 0.5f, 1f };

    RectTransform Transform;

    HorizontalOrientation hOrientation = HorizontalOrientation.Center;
    VerticalOrientation vOrientation = VerticalOrientation.Center;
    public HorizontalOrientation HOrientation { get => hOrientation; protected set { hOrientation = value; RefreshTransform(); } }
    public VerticalOrientation VOrientation { get => vOrientation; protected set { vOrientation = value; RefreshTransform(); } }

    public Offset Offsets { get; protected set; }
    public Vector2 Size { get; protected set; }

    public virtual void Awake()
    {
        Skin = UISkinHelper.Skin;

        Transform = GetComponent<RectTransform>();
        Offsets = new Offset(Vector2.zero, Vector2.one * 100);
        Offsets.Changed += OffsetChanged;

        RefreshTransform();
    }

    void OffsetChanged(object sender, EventArgs e)
    {
        RefreshTransform();
    }

    public virtual void RefreshTransform()
    {
        Transform.pivot = new Vector2(ORIENTATION_PIVOTS[(int)hOrientation], 1f - ORIENTATION_PIVOTS[(int)vOrientation]);

        Vector2 anchorMin = Transform.anchorMin;
        Vector2 anchorMax = Transform.anchorMax;

        anchorMin.x = anchorMax.x = ORIENTATION_ANCHOR_MINMAX[(int)hOrientation];
        anchorMin.y = anchorMax.y = 1f - ORIENTATION_ANCHOR_MINMAX[(int)vOrientation];

        if (Offsets.HAbsolute)
        {
            Transform.SetPositionLeft(Offsets.Left);
            Transform.SetWidth(Offsets.Width);
        }
        else
        {
            Transform.SetLeft(Offsets.Left);
            Transform.SetRight(Offsets.Right);

            anchorMin.x = 0;
            anchorMax.x = 1;
        }

        if (Offsets.VAbsolute)
        {
            Transform.SetPositionTop(Offsets.Top);
            Transform.SetHeight(Offsets.Height);
        }
        else
        {
            Transform.SetTop(Offsets.Top);
            Transform.SetBottom(Offsets.Bottom);

            anchorMin.y = 0;
            anchorMax.y = 1;
        }

        Transform.anchorMin = anchorMin;
        Transform.anchorMax = anchorMax;
    }
}
public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static void SetPositionLeft(this RectTransform rt, float left)
    {
        rt.anchoredPosition = new Vector2(left, rt.anchoredPosition.y);
    }

    public static void SetPositionTop(this RectTransform rt, float top)
    {
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -top);
    }

    public static void SetWidth(this RectTransform rt, float width)
    {
        rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
    }

    public static void SetHeight(this RectTransform rt, float height)
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
    }
}