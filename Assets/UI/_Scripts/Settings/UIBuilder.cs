using Assets.UI._Scripts.Misc;
using Assets.UI._Scripts.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas)), RequireComponent(typeof(CanvasScaler))]
public abstract class UIBuilder : MonoBehaviour
{
    protected Canvas canvas { get; private set; }
    protected CanvasScaler scaler { get; private set; }

    Vector2 uiSize = Vector2.zero;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        scaler = GetComponent<CanvasScaler>();

        UIIcons.UpdateAllTMText();

        BuildUI();
    }

    protected float UIWidth
    {
        get
        {
            if (uiSize.magnitude == 0)
                uiSize = scaler.referenceResolution;

            return uiSize.x;
        }
    }
    protected float UIHeight
    {
        get
        {
            if (uiSize.magnitude == 0)
                uiSize = scaler.referenceResolution;

            return uiSize.y;
        }
    }

    protected Vector2 ScaledPoint(Vector2 point)
    {
        return new Vector2(point.x * UIWidth, point.y * UIHeight);
    }
    protected Vector2 ScaledPoint(float x, float y)
    {
        return ScaledPoint(new Vector2(x, y));
    }

    protected abstract void BuildUI();
}
