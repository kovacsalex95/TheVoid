using System;
using UnityEngine;

public class Offset
{
    float left;
    float top;
    float right;
    float bottom;
    float width;
    float height;

    public EventHandler Changed;

    public float Left { get => left; set { left = value; fieldChange(); } }
    public float Top { get => top; set { top = value; fieldChange(); } }
    public float Right { get => right; set { right = value; fieldChange(); } }
    public float Bottom { get => bottom; set { bottom = value; fieldChange(); } }
    public float Width { get => width; set { width = value; fieldChange(false); } }
    public float Height { get => height; set { height = value; fieldChange(false); } }

    public bool HAbsolute => width != -1 && right == -1;
    public bool VAbsolute => height != -1 && bottom == -1;

    public Offset()
    {
        left = right = top = bottom = 0;
        width = height = -1;
    }
    public Offset(float all)
    {
        left = right = top = bottom = all;
        width = height = -1;
    }
    public Offset(float horizontal, float vertical)
    {
        left = right = horizontal;
        top = bottom = vertical;
        width = height = -1;
    }
    public Offset(float left, float top, float right, float bottom)
    {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
        width = height = -1;
    }
    public Offset(Vector2 position, Vector2 size)
    {
        left = position.x;
        top = position.y;
        width = size.x;
        height = size.y;
        right = bottom = -1;
    }

    private void fieldChange(bool offset = true)
    {
        if (width != -1 && right != -1)
        {
            if (offset)
                width = -1;
            else
                right = -1;
        }
        if (height != -1 && bottom != -1)
        {
            if (offset)
                height = -1;
            else
                bottom = -1;
        }

        if (Changed != null)
            Changed.Invoke(this, new EventArgs());
    }
}
