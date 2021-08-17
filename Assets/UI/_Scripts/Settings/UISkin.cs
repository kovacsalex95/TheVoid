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
    public ScrollbarSkin Scrollbar;

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
public class ScrollbarSkin
{
    public Color NormalColor = Color.black;
    public Color HoverColor = Color.black;
    public Color PressedColor = Color.black;
    public Color DisabledColor = Color.black;
    public float FadeDuration = 0.1f;
    public Sprite HandleImage;
    public Image.Type HandleSlicing;
}

[System.Serializable]
public class GroupPanelSkin
{
    public float ElementSpacing = 30;
}