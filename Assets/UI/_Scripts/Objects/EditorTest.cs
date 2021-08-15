using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTest : UIElement
{
    public HorizontalOrientation horizontalOrientation;
    public VerticalOrientation verticalOrientation;

    HorizontalOrientation _horizontalOrientation;
    VerticalOrientation _verticalOrientation;

    public float left;
    public float right;
    public float top;
    public float bottom;
    public float width;
    public float height;

    float _left;
    float _right;
    float _top;
    float _bottom;
    float _width;
    float _height;

    public override void Awake()
    {
        base.Awake();
        ReadValues();
    }

    public void Update()
    {
        if (left != _left)
            _left = base.Offsets.Left = left;

        if (right != _right)
            _right = base.Offsets.Right = right;

        if (top != _top)
            _top = base.Offsets.Top = top;

        if (bottom != _bottom)
            _bottom = base.Offsets.Bottom = bottom;

        if (width != _width)
            _width = base.Offsets.Width = width;

        if (height != _height)
            _height = base.Offsets.Height = height;

        if (horizontalOrientation != _horizontalOrientation)
            _horizontalOrientation = base.HOrientation = horizontalOrientation;

        if (verticalOrientation != _verticalOrientation)
            _verticalOrientation = base.VOrientation = verticalOrientation;
    }

    public override void RefreshTransform()
    {
        Debug.Log("Update");

        base.RefreshTransform();

        ReadValues();
    }

    void ReadValues()
    {
        horizontalOrientation = base.HOrientation;
        verticalOrientation = base.VOrientation;
        left = _left = base.Offsets.Left;
        right = _right = base.Offsets.Right;
        top = _top = base.Offsets.Top;
        bottom = _bottom = base.Offsets.Bottom;
        width = _width = base.Offsets.Width;
        height = _height = base.Offsets.Height;
    }
}
