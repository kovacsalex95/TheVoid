using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkin : MonoBehaviour
{
    public ImageIcon[] icons;
    public ImageSkin Panel;
    public ButtonSkin Button;
    public GroupPanelSkin GroupPanel;

    Dictionary<string, Sprite> iconsDict = null;
    public Sprite GetIcon(string iconName)
    {
        if (iconsDict == null)
        {
            iconsDict = new Dictionary<string, Sprite>();

            foreach (ImageIcon icon in icons)
                iconsDict.Add(icon.name, icon.image);
        }

        if (iconsDict.ContainsKey(iconName))
            return iconsDict[iconName];

        return null;
    }

    private void Awake()
    {
        
    }
}

[System.Serializable]
public struct ImageIcon
{
    public string name;
    public Sprite image;
}

[System.Serializable]
public class ImageSkin
{
    public Sprite BackgroundImage;
    public Image.Type BackgroundSlicing;
    public Color BackgroundColor = Color.black;
}

[System.Serializable]
public class ButtonSkin : ImageSkin
{
    public Color TextColor = Color.white;
    public Color HoverBackgroundColor = Color.black;
    public Color HoverTextColor = Color.white;
    public Color PressedBackgroundColor = Color.black;
    public Color PressedTextColor = Color.white;
    public Color DisabledBackgroundColor = Color.black;
    public Color DisabledTextColor = Color.white;
    public HorizontalOrientation TextAlign = HorizontalOrientation.Center;
    public HorizontalOrientation IconAlign = HorizontalOrientation.Left;
    public float FadeDuration = 0.1f;
    [Range(0f, 1f)]
    public float Gaps = 0.1f;
}

[System.Serializable]
public class GroupPanelSkin
{
    public float ElementSpacing = 30;
}

public static class UIUtil
{
    static UISkin uiSkin = null;

    public static UISkin Skin
    {
        get {
            if (uiSkin == null)
                uiSkin = GameObject.FindObjectOfType<UISkin>();

            return uiSkin;
        }
    }

    public static Color ColorOpacity(Color source, float opacity = 1)
    {
        return new Color(source.r, source.g, source.b, opacity);
    }

    public static RectOffset Padding(float left, float right, float top, float bottom)
    {
        return new RectOffset(Mathf.RoundToInt(left), Mathf.RoundToInt(right), Mathf.RoundToInt(top), Mathf.RoundToInt(bottom));
    }
    public static RectOffset Padding(float all = 0)
    {
        return Padding(all, all, all, all);
    }
}